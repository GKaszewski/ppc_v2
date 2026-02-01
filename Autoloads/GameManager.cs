using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Game orchestrator - handles scene management and game flow.
/// State is delegated to GameStateStore for better separation of concerns.
/// </summary>
public partial class GameManager : Node
{
    [Export] public Array<PackedScene> LevelScenes { get; set; } = [];

    public PlayerController Player
    {
        get => GetPlayer();
        private set => _player = value;
    }

    private List<Node> _sceneNodes = [];
    private PlayerController _player;
    private SpeedRunManager _speedRunManager;

    /// <summary>
    /// Lazy accessor for GameStateStore - avoids initialization order issues.
    /// </summary>
    private GameStateStore Store => GameStateStore.Instance;

    public static GameManager Instance { get; private set; }

    public override void _EnterTree()
    {
        GetTree().NodeAdded += OnNodeAdded;
        GetTree().NodeRemoved += OnNodeRemoved;
    }

    public override void _ExitTree()
    {
        if (Instance == this) Instance = null;
        GetTree().NodeAdded -= OnNodeAdded;
        GetTree().NodeRemoved -= OnNodeRemoved;
        _sceneNodes.Clear();
    }

    public override void _Ready()
    {
        Instance = this;
        _speedRunManager = GetNode<SpeedRunManager>(Constants.SpeedRunManagerPath);
    }

    private void OnNodeAdded(Node node)
    {
        _sceneNodes.Add(node);
    }

    private void OnNodeRemoved(Node node)
    {
        _sceneNodes.Remove(node);
        if (node == _player)
        {
            _player = null;
        }
    }

    #region Coin Operations

    public void AddCoins(int amount)
    {
        if (Store != null)
        {
            Store.Player.Coins += amount;
            EventBus.EmitCoinsChanged(Store.GetTotalCoins());
        }
    }

    public void SetCoins(int amount)
    {
        if (Store != null)
        {
            Store.Player.Coins = Mathf.Max(0, amount);
            EventBus.EmitCoinsChanged(Store.GetTotalCoins());
        }
    }

    public int GetCoins() => Store?.GetTotalCoins() ?? 0;

    public void RemoveCoins(int amount) => Store?.RemoveCoins(amount);

    #endregion

    #region Lives Operations

    public void AddLives(int amount) => Store?.AddLives(amount);
    public void RemoveLives(int amount) => Store?.RemoveLife();
    public void SetLives(int amount)
    {
        if (Store != null)
        {
            Store.Player.Lives = amount;
            EventBus.EmitLivesChanged(amount);
        }
    }
    public int GetLives() => Store?.Player.Lives ?? 0;

    #endregion

    #region Skill Operations

    public bool IsSkillUnlocked(SkillData skill) => Store?.IsSkillUnlocked(skill) ?? false;

    public void UnlockSkill(SkillData skill)
    {
        if (Store != null && !Store.IsSkillUnlocked(skill))
        {
            Store.Player.UnlockedSkills.Add(skill);
        }
    }

    public void RemoveSkill(string skillName)
    {
        if (Store == null) return;
        var skills = Store.Player.UnlockedSkills;
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].Name == skillName)
            {
                skills.RemoveAt(i);
                break;
            }
        }
    }

    public void UnlockSkills(Array<SkillData> skills)
    {
        foreach (var s in skills)
            UnlockSkill(s);
    }

    public Array<SkillData> GetUnlockedSkills()
    {
        if (Store == null) return new Array<SkillData>();

        var skills = Store.GetAllUnlockedSkills();
        var result = new Array<SkillData>();
        foreach (var s in skills) result.Add(s);
        return result;
    }

    #endregion

    #region Level Operations

    public void UnlockLevel(int levelIndex) => Store?.UnlockLevel(levelIndex);

    public void MarkLevelComplete(int levelIndex) => Store?.MarkLevelComplete(levelIndex);

    public void TryToGoToNextLevel()
    {
        if (Store == null) return;

        var next = Store.Session.CurrentLevel + 1;
        if (next < LevelScenes.Count && Store.IsLevelUnlocked(next))
        {
            Store.Session.CurrentLevel = next;
            GetTree().ChangeSceneToPacked(LevelScenes[next]);
            EventBus.EmitLevelStarted(next, GetTree().CurrentScene);
        }
    }

    #endregion

    #region State Reset

    public void ResetPlayerState() => Store?.ResetAll();

    public void ResetCurrentSessionState() => Store?.ResetSession();

    #endregion

    #region Game Flow

    public void RestartGame()
    {
        Store?.ResetAll();
        GetTree().ChangeSceneToPacked(LevelScenes[0]);
        GetNode<SaveSystem>(Constants.SaveSystemPath).SaveGame();
    }

    public void QuitGame() => GetTree().Quit();

    public void PauseGame()
    {
        Engine.TimeScale = 0;
        EventBus.EmitGamePaused();
    }

    public void ResumeGame()
    {
        Engine.TimeScale = 1;
        EventBus.EmitGameResumed();
    }

    public void StartNewGame()
    {
        Store?.ResetAll();
        _speedRunManager?.StartTimer();
        GetTree().ChangeSceneToPacked(LevelScenes[0]);
        GetNode<SaveSystem>(Constants.SaveSystemPath).SaveGame();
        EventBus.EmitGameStarted();
    }

    public void ContinueGame()
    {
        var save = GetNode<SaveSystem>(Constants.SaveSystemPath);
        if (!save.LoadGame())
        {
            GD.PrintErr("Failed to load game. Starting a new game instead.");
            StartNewGame();
            return;
        }

        var idx = Store?.Session.CurrentLevel ?? 0;
        if (idx < LevelScenes.Count)
        {
            GetTree().ChangeSceneToPacked(LevelScenes[idx]);
            EventBus.EmitGameContinued();
        }
        else
        {
            GD.PrintErr("No levels unlocked to continue.");
        }
    }

    public void OnLevelComplete()
    {
        if (Store == null) return;

        var levelIndex = Store.Session.CurrentLevel;
        Store.MarkLevelComplete(levelIndex);
        Store.CommitSessionCoins();
        Store.CommitSessionSkills();

        var completionTime = _speedRunManager?.GetCurrentLevelTime() ?? 0.0;
        EventBus.EmitLevelCompleted(levelIndex, GetTree().CurrentScene, completionTime);

        Store.ResetSession();
        TryToGoToNextLevel();
        GetNode<SaveSystem>(Constants.SaveSystemPath).SaveGame();
    }

    #endregion

    #region Player Lookup

    public PlayerController GetPlayer()
    {
        if (_player != null && IsInstanceValid(_player)) return _player;

        _player = null;

        foreach (var node in _sceneNodes)
        {
            if (node is not PlayerController player) continue;

            _player = player;
            return _player;
        }

        GD.PrintErr("PlayerController not found in the scene tree.");
        return null;
    }

    #endregion
}