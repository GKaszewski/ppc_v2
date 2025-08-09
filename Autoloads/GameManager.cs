using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.Autoloads;

public partial class GameManager : Node
{
    [Export] public Array<PackedScene> LevelScenes { get; set; } = new();

    public Dictionary PlayerState { get; set; } = new()
    {
        { "coins", 0 },
        { "lives", 3 },
        { "current_level", 0 },
        { "completed_levels", new Array<int>() },
        { "unlocked_levels", new Array<int>() {0}},
        { "unlocked_skills", new Array<SkillData>() }
    };

    private Dictionary _currentSessionState = new()
    {
        { "coins_collected", 0 },
        { "skills_unlocked", new Array<SkillData>() }
    };

    public void AddCoins(int amount)
    {
        PlayerState["coins"] = Mathf.Max(0, (int)PlayerState["coins"] + amount);
    }
    
    public void SetCoins(int amount) => PlayerState["coins"] = Mathf.Max(0, amount);
    
    public int GetCoins() => (int)PlayerState["coins"] + (int)_currentSessionState["coins_collected"];
    
    public void RemoveCoins(int amount)
    {
        var sessionCoins = (int)_currentSessionState["coins_collected"];
        if (amount <= sessionCoins)
        {
            _currentSessionState["coins_collected"] = sessionCoins - amount;
        }
        else
        {
            var remaining = amount - sessionCoins;
            _currentSessionState["coins_collected"] = 0;
            PlayerState["coins"] = Mathf.Max(0, (int)PlayerState["coins"] - remaining);
        }
        PlayerState["coins"] = Mathf.Max(0, (int)PlayerState["coins"]);
    }
    
    public void AddLives(int amount) => PlayerState["lives"] = (int)PlayerState["lives"] + amount;
    public void RemoveLives(int amount) => PlayerState["lives"] = (int)PlayerState["lives"] - amount;
    public void SetLives(int amount) => PlayerState["lives"] = amount;
    public int GetLives() => (int)PlayerState["lives"];
    
    public bool IsSkillUnlocked(SkillData skill)
    {
        return ((Array)PlayerState["unlocked_skills"]).Contains(skill)
               || ((Array)_currentSessionState["skills_unlocked"]).Contains(skill);
    }

    public void UnlockSkill(SkillData skill)
    {
        if (!IsSkillUnlocked(skill))
            ((Array)PlayerState["unlocked_skills"]).Add(skill);
    }

    public void RemoveSkill(string skillName)
    {
        var arr = (Array)PlayerState["unlocked_skills"];
        foreach (SkillData s in arr)
        {
            if (s.Name != skillName) continue;
            
            arr.Remove(s);
            break;
        }
    }

    public void UnlockSkills(Array<SkillData> skills)
    {
        foreach (var s in skills)
            UnlockSkill(s);
    }

    public void ResetPlayerState()
    {
        PlayerState = new Dictionary
        {
            { "coins", 0 },
            { "lives", 3 },
            { "current_level", 0 },
            { "completed_levels", new Array<int>() },
            { "unlocked_levels", new Array<int>() {0}},
            { "unlocked_skills", new Array<SkillData>() }
        };
    }
    
    public void UnlockLevel(int levelIndex)
    {
        var unlocked = (Array)PlayerState["unlocked_levels"];
        if (!unlocked.Contains(levelIndex)) unlocked.Add(levelIndex);
    }

    public void TryToGoToNextLevel()
    {
        var next = (int)PlayerState["current_level"] + 1;
        var unlocked = (Array)PlayerState["unlocked_levels"];
        if (next < LevelScenes.Count && unlocked.Contains(next))
        {
            PlayerState["current_level"] = next;
            GetTree().ChangeSceneToPacked(LevelScenes[next]);
        }
    }
    
    public void MarkLevelComplete(int levelIndex)
    {
        UnlockLevel(levelIndex + 1);
        var completed = (Array)PlayerState["completed_levels"];
        if (!completed.Contains(levelIndex)) completed.Add(levelIndex);
    }

    public void ResetCurrentSessionState()
    {
        _currentSessionState = new Dictionary
        {
            { "coins_collected", 0 },
            { "skills_unlocked", new Array<SkillData>() }
        };
    }
    
    public void RestartGame()
    {
        ResetPlayerState();
        ResetCurrentSessionState();
        GetTree().ChangeSceneToPacked(LevelScenes[0]);
        GetNode<SaveSystem>("/root/SaveSystem").SaveGame();
    }
    
    public void QuitGame() => GetTree().Quit();

    public void PauseGame() => Engine.TimeScale = 0;
    public void ResumeGame() => Engine.TimeScale = 1;
    
    public void StartNewGame()
    {
        ResetPlayerState();
        ResetCurrentSessionState();
        GetTree().ChangeSceneToPacked(LevelScenes[0]);
        GetNode<SaveSystem>("/root/SaveSystem").SaveGame();
    }

    public void ContinueGame()
    {
        var save = GetNode<SaveSystem>("/root/SaveSystem");
        if (!save.LoadGame())
        {
            GD.PrintErr("Failed to load game. Starting a new game instead.");
            StartNewGame();
            return;
        }

        var idx = (int)PlayerState["current_level"];
        if (idx < LevelScenes.Count)
            GetTree().ChangeSceneToPacked(LevelScenes[idx]);
        else
            GD.PrintErr("No levels unlocked to continue.");
    }
    
    public void OnLevelComplete()
    {
        var levelIndex = (int)PlayerState["current_level"];
        MarkLevelComplete(levelIndex);
        AddCoins((int)_currentSessionState["coins_collected"]);
        foreach (var s in (Array)_currentSessionState["skills_unlocked"])
            UnlockSkill((SkillData)s);

        ResetCurrentSessionState();
        TryToGoToNextLevel();
        GetNode<SaveSystem>("/root/SaveSystem").SaveGame();
    }

    public Array GetUnlockedSkills()
    {
        var unlocked = (Array<SkillData>)PlayerState["unlocked_skills"];
        var session = (Array<SkillData>)_currentSessionState["skills_unlocked"];
        if ((((Array)session)!).Count == 0) return (Array)unlocked;
        if ((((Array)unlocked)!).Count == 0) return (Array)session;
        var joined = new Array();
        joined.AddRange((Array)unlocked ?? new Array());
        joined.AddRange((Array)session ?? new Array());
        return joined;
    }
}