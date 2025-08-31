using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class WaveTriggerComponent : Node
{
    [Export] public Area2D TriggerArea { get; set; }
    [Export] public EnemyWaveManager WaveManager { get; set; }
    [Export] public ChaseLevelComponent ChaserToPause { get; set; } // Optional
    [Export] public bool PauseChaser { get; set; } = true;
    
    private bool _hasBeenTriggered = false;
    
    public override void _Ready()
    {
        if (TriggerArea == null || WaveManager == null)
        {
            GD.PrintErr("WaveTriggerComponent is missing a TriggerArea or WaveManager.");
            SetProcess(false);
            return;
        }

        TriggerArea.BodyEntered += OnPlayerEntered;
        WaveManager.WaveCompleted += OnWaveCompleted;
    }
    
    private void OnPlayerEntered(Node2D body)
    {
        if (body is PlayerController && !_hasBeenTriggered)
        {
            _hasBeenTriggered = true;
            
            if (PauseChaser && ChaserToPause != null)
            {
                ChaserToPause.SetChasing(false);
            }
            
            WaveManager.StartWave();
            TriggerArea.QueueFree();
        }
    }

    private void OnWaveCompleted()
    {
        GD.Print("Wave completed!");
        if (PauseChaser && ChaserToPause != null)
        {
            ChaserToPause.SetChasing(true);
        }
    }
}