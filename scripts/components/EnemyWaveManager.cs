using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class EnemyWaveManager : Node
{
    [Export] public Path2D EnemyPath { get; set; }
    [Export] public Array<PackedScene> EnemyScenes { get; set; } = [];
    [Export] public float SpawnInterval { get; set; } = 1.0f; // Time between each enemy spawn
    
    private int _enemiesToSpawn;
    private int _activeEnemies;
    private Timer _spawnTimer;
    
    [Signal]
    public delegate void WaveCompletedEventHandler();
    
    public override void _Ready()
    {
        _spawnTimer = new Timer();
        _spawnTimer.WaitTime = SpawnInterval;
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += SpawnNextEnemy;
        AddChild(_spawnTimer);
    }
    
    public void StartWave()
    {
        if (EnemyScenes.Count == 0 || EnemyPath == null)
        {
            GD.PrintErr("EnemyWaveManager: Enemy scenes or path not set!");
            return;
        }

        _enemiesToSpawn = EnemyScenes.Count;
        _activeEnemies = 0;
        _spawnTimer.Start();
    }
    
    private void SpawnNextEnemy()
    {
        if (_enemiesToSpawn <= 0)
        {
            _spawnTimer.Stop();
            return;
        }

        var enemyIndex = EnemyScenes.Count - _enemiesToSpawn;
        var enemyScene = EnemyScenes[enemyIndex];
        
        var pathFollowNode = new PathFollow2D();
        EnemyPath.AddChild(pathFollowNode);
        
        var enemyInstance = enemyScene.Instantiate<Node2D>();
        pathFollowNode.AddChild(enemyInstance);
        
        var pathFollowerComponent = enemyInstance.GetNodeOrNull<PathFollowerComponent>("PathFollowerComponent");
        if (pathFollowerComponent == null)
        {
            GD.PrintErr($"Enemy scene '{enemyScene.ResourcePath}' is missing a PathFollowerComponent.");
            pathFollowNode.QueueFree();
            _enemiesToSpawn--;
            return;
        }
        
        pathFollowNode.Rotates = pathFollowerComponent.ShouldRotate;
        pathFollowerComponent.Initialize(pathFollowNode);
        pathFollowerComponent.StartFollowing();
        pathFollowerComponent.EnemyDestroyed += OnEnemyDestroyed;

        _enemiesToSpawn--;
        _activeEnemies++;
    }

    private void OnEnemyDestroyed()
    {
        _activeEnemies--;
        
        if (_enemiesToSpawn == 0 && _activeEnemies == 0)
        {
            EmitSignalWaveCompleted();
        }
    }
}