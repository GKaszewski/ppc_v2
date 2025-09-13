using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts;

namespace Mr.BrickAdventures.Autoloads;

public partial class GhostManager : Node
{
    [Export] private PackedScene GhostPlayerScene { get; set; }
    
    public bool IsRecording { get; private set; } = false;
    public bool IsPlaybackEnabled { get; private set; } = true;

    private List<GhostFrame> _currentRecording = [];
    private double _startTime = 0.0;
    private int _currentLevelIndex = -1;

    public void StartRecording(int levelIndex)
    {
        if (!IsPlaybackEnabled) return;
        
        _currentLevelIndex = levelIndex;
        _currentRecording.Clear();
        _startTime = Time.GetTicksMsec() / 1000.0;
        IsRecording = true;
        GD.Print("Ghost recording started.");
    }
    
    public void StopRecording(bool levelCompleted, double finalTime)
    {
        if (!IsRecording) return;
        IsRecording = false;

        if (levelCompleted)
        {
            var bestTime = LoadBestTime(_currentLevelIndex);
            if (finalTime < bestTime)
            {
                SaveGhostData(_currentLevelIndex, finalTime);
                GD.Print($"New best ghost saved for level {_currentLevelIndex}. Time: {finalTime}");
            }
        }
        _currentRecording.Clear();
    }
    
    public void RecordFrame(Vector2 position)
    {
        if (!IsRecording) return;

        var frame = new GhostFrame
        {
            Timestamp = (Time.GetTicksMsec() / 1000.0) - _startTime,
            Position = position
        };
        _currentRecording.Add(frame);
    }
    
    public void SpawnGhostPlayer(int levelIndex, Node parent)
    {
        if (!IsPlaybackEnabled || GhostPlayerScene == null) return;
        
        var ghostData = LoadGhostData(levelIndex);
        if (ghostData.Count > 0)
        {
            var ghostPlayer = GhostPlayerScene.Instantiate<GhostPlayer>();
            parent.AddChild(ghostPlayer);
            ghostPlayer.StartPlayback(ghostData);
            GD.Print($"Ghost player spawned for level {levelIndex}.");
        }
    }
    
    private void SaveGhostData(int levelIndex, double time)
    {
        var path = $"user://ghost_level_{levelIndex}.dat";
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        
        var dataToSave = new Godot.Collections.Dictionary
        {
            { "time", time },
            { "frames", _currentRecording.ToArray() }
        };
        file.StoreVar(dataToSave);
    }
    
    private List<GhostFrame> LoadGhostData(int levelIndex)
    {
        var path = $"user://ghost_level_{levelIndex}.dat";
        if (!FileAccess.FileExists(path)) return [];

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var savedData = (Dictionary)file.GetVar();
        var framesArray = (Array)savedData["frames"];
        
        var frames = new List<GhostFrame>();
        foreach (var obj in framesArray)
        {
            frames.Add((GhostFrame)obj);
        }
        return frames;
    }
    
    private double LoadBestTime(int levelIndex)
    {
        var path = $"user://ghost_level_{levelIndex}.dat";
        if (!FileAccess.FileExists(path)) return double.MaxValue;

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var data = (Dictionary)file.GetVar();
        return (double)data["time"];
    }
}