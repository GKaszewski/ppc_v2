using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class FootstepGfx : Node2D
{
    [Export] private PackedScene _particles;
    [Export] private PlayerController _controller;
    [Export] private Marker2D _marker;
    
    [ExportGroup("Footstep Settings")]
    [Export] private double _stepInterval = 0.5; // Time in seconds between steps
    [Export] private double _stepIntervalRandomness = 0.1; // Randomness factor for step interval
    [Export] private double _minMoveSpeed = 10.0; // Minimum speed to trigger footsteps
    [Export] private double _randomOffsetRange = 5.0; // Range for random offset
    
    private double _timeSinceLastStep = 0.0;
    
    public override void _PhysicsProcess(double delta)
    {
        if (_controller == null || _particles == null) return;

        var canMakeFootstep = _controller.IsOnFloor() && _controller.Velocity.Length() >= _minMoveSpeed;
        
        if (canMakeFootstep)
        {
            _timeSinceLastStep += delta;
            var randomFactor = GD.RandRange(-_stepIntervalRandomness, _stepIntervalRandomness);
            var adjustedStepInterval = _stepInterval + randomFactor;
            if (_timeSinceLastStep >= adjustedStepInterval)
            {
                SpawnFootstep();
                _timeSinceLastStep = 0.0;
            }
        }
        else
        {
            _timeSinceLastStep = 0.0;
        }
    }
    
    private void SpawnFootstep()
    {
        if (_marker == null) return;
        var randomOffset = new Vector2(
            (float)GD.RandRange(-_randomOffsetRange, _randomOffsetRange),
            (float)GD.RandRange(-_randomOffsetRange, _randomOffsetRange)
        );
        var newParticles = _particles.Instantiate<GpuParticles2D>();
        newParticles.GlobalPosition = _marker.GlobalPosition + randomOffset;
        GetTree().CurrentScene.AddChild(newParticles);
        newParticles.Emitting = true;
    }
}