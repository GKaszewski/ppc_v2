using System.Collections.Generic;
using Godot;

namespace Mr.BrickAdventures.scripts;

[GlobalClass]
public partial class GhostPlayer : Node2D
{
    private List<GhostFrame> _playbackData = [];
    private double _startTime = 0;
    private int _currentFrameIndex = 0;
    private bool _isPlaying = false;
    
    public void StartPlayback(List<GhostFrame> playbackData)
    {
        _playbackData = playbackData;
        _startTime = Time.GetTicksMsec() / 1000.0;
        _currentFrameIndex = 0;
        _isPlaying = true;
        SetProcess(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_isPlaying || _playbackData.Count == 0) return;

        var currentTime = (Time.GetTicksMsec() / 1000.0) - _startTime;

        while (_currentFrameIndex + 1 < _playbackData.Count && _playbackData[_currentFrameIndex + 1].Timestamp <= currentTime)
        {
            _currentFrameIndex++;
        }

        if (_currentFrameIndex + 1 >= _playbackData.Count)
        {
            GlobalPosition = _playbackData[_currentFrameIndex].Position;
            _isPlaying = false;
            QueueFree();
            return;
        }
        
        var frameA = _playbackData[_currentFrameIndex];
        var frameB = _playbackData[_currentFrameIndex + 1];
        var t = (currentTime - frameA.Timestamp) / (frameB.Timestamp - frameA.Timestamp);

        GlobalPosition = frameA.Position.Lerp(frameB.Position, (float)t);
    }
}