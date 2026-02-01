using Godot;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Global event bus for decoupled communication between game systems.
/// Use the static Instance property for easy access from anywhere.
/// </summary>
public partial class EventBus : Node
{
    /// <summary>
    /// Singleton instance. Available after the autoload is initialized.
    /// </summary>
    public static EventBus Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
            Instance = null;
    }

    #region Level Events

    [Signal] public delegate void LevelStartedEventHandler(int levelIndex, Node currentScene);
    [Signal] public delegate void LevelCompletedEventHandler(int levelIndex, Node currentScene, double completionTime);
    [Signal] public delegate void LevelRestartedEventHandler(int levelIndex);

    public static void EmitLevelStarted(int levelIndex, Node currentScene)
        => Instance?.EmitSignal(SignalName.LevelStarted, levelIndex, currentScene);

    public static void EmitLevelCompleted(int levelIndex, Node currentScene, double completionTime)
        => Instance?.EmitSignal(SignalName.LevelCompleted, levelIndex, currentScene, completionTime);

    public static void EmitLevelRestarted(int levelIndex)
        => Instance?.EmitSignal(SignalName.LevelRestarted, levelIndex);

    #endregion

    #region Player Events

    [Signal] public delegate void PlayerSpawnedEventHandler(PlayerController player);
    [Signal] public delegate void PlayerDiedEventHandler(Vector2 position);
    [Signal] public delegate void PlayerDamagedEventHandler(float damage, float remainingHealth, Vector2 position);
    [Signal] public delegate void PlayerHealedEventHandler(float amount, float newHealth, Vector2 position);

    public static void EmitPlayerSpawned(PlayerController player)
        => Instance?.EmitSignal(SignalName.PlayerSpawned, player);

    public static void EmitPlayerDied(Vector2 position)
        => Instance?.EmitSignal(SignalName.PlayerDied, position);

    public static void EmitPlayerDamaged(float damage, float remainingHealth, Vector2 position)
        => Instance?.EmitSignal(SignalName.PlayerDamaged, damage, remainingHealth, position);

    public static void EmitPlayerHealed(float amount, float newHealth, Vector2 position)
        => Instance?.EmitSignal(SignalName.PlayerHealed, amount, newHealth, position);

    #endregion

    #region Combat Events

    [Signal] public delegate void EnemyDefeatedEventHandler(Node enemy, Vector2 position);
    [Signal] public delegate void EnemyDamagedEventHandler(Node enemy, float damage, Vector2 position);

    public static void EmitEnemyDefeated(Node enemy, Vector2 position)
        => Instance?.EmitSignal(SignalName.EnemyDefeated, enemy, position);

    public static void EmitEnemyDamaged(Node enemy, float damage, Vector2 position)
        => Instance?.EmitSignal(SignalName.EnemyDamaged, enemy, damage, position);

    #endregion

    #region Collection Events

    [Signal] public delegate void CoinCollectedEventHandler(int amount, Vector2 position);
    [Signal] public delegate void ItemCollectedEventHandler(CollectableType itemType, float amount, Vector2 position);
    [Signal] public delegate void ChildRescuedEventHandler(Vector2 position);
    [Signal] public delegate void SkillCollectedEventHandler(SkillData skill, Vector2 position);

    public static void EmitCoinCollected(int amount, Vector2 position)
        => Instance?.EmitSignal(SignalName.CoinCollected, amount, position);

    public static void EmitItemCollected(CollectableType itemType, float amount, Vector2 position)
        => Instance?.EmitSignal(SignalName.ItemCollected, (int)itemType, amount, position);

    public static void EmitChildRescued(Vector2 position)
        => Instance?.EmitSignal(SignalName.ChildRescued, position);

    public static void EmitSkillCollected(SkillData skill, Vector2 position)
        => Instance?.EmitSignal(SignalName.SkillCollected, skill, position);

    #endregion

    #region Skill Events

    [Signal] public delegate void SkillUnlockedEventHandler(string skillName, int level);
    [Signal] public delegate void SkillActivatedEventHandler(string skillName);
    [Signal] public delegate void SkillDeactivatedEventHandler(string skillName);

    public static void EmitSkillUnlocked(string skillName, int level = 1)
        => Instance?.EmitSignal(SignalName.SkillUnlocked, skillName, level);

    public static void EmitSkillActivated(string skillName)
        => Instance?.EmitSignal(SignalName.SkillActivated, skillName);

    public static void EmitSkillDeactivated(string skillName)
        => Instance?.EmitSignal(SignalName.SkillDeactivated, skillName);

    #endregion

    #region Game State Events

    [Signal] public delegate void GamePausedEventHandler();
    [Signal] public delegate void GameResumedEventHandler();
    [Signal] public delegate void GameSavedEventHandler();
    [Signal] public delegate void GameStartedEventHandler();
    [Signal] public delegate void GameContinuedEventHandler();

    public static void EmitGamePaused()
        => Instance?.EmitSignal(SignalName.GamePaused);

    public static void EmitGameResumed()
        => Instance?.EmitSignal(SignalName.GameResumed);

    public static void EmitGameSaved()
        => Instance?.EmitSignal(SignalName.GameSaved);

    public static void EmitGameStarted()
        => Instance?.EmitSignal(SignalName.GameStarted);

    public static void EmitGameContinued()
        => Instance?.EmitSignal(SignalName.GameContinued);

    #endregion

    #region State Change Events

    [Signal] public delegate void CoinsChangedEventHandler(int totalCoins);
    [Signal] public delegate void LivesChangedEventHandler(int lives);

    public static void EmitCoinsChanged(int totalCoins)
        => Instance?.EmitSignal(SignalName.CoinsChanged, totalCoins);

    public static void EmitLivesChanged(int lives)
        => Instance?.EmitSignal(SignalName.LivesChanged, lives);

    #endregion
}