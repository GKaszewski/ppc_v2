using System.Collections.Generic;
using Godot;

namespace Mr.BrickAdventures.Autoloads;

public partial class SpeedRunManager : Node
{
    public bool IsRunning { get; private set; } = false;
    public bool IsVisible { get; private set; } = true;
    
    private double _startTime;
    private double _levelStartTime;
    private List<double> _splits = [];
    
    [Signal] public delegate void TimeUpdatedEventHandler(double totalTime, double levelTime);

    public override void _Process(double delta)
    {
        if (!IsRunning || !IsVisible) return;
        
        EmitSignalTimeUpdated(GetCurrentTotalTime(), GetCurrentLevelTime());
    }

    public void StartTimer()
    {
        _startTime = Time.GetTicksMsec() / 1000.0;
        _levelStartTime = _startTime;
        _splits.Clear();
        IsRunning = true;
        GD.Print("Speedrun timer started.");
    }
    
    public void StopTimer()
    {
        if (!IsRunning) return;
        IsRunning = false;
        var finalTime = GetCurrentTotalTime();
        GD.Print($"Speedrun finished. Final time: {FormatTime(finalTime)}");
        
        // Save personal best if applicable
    }

    public void Split()
    {
        if (!IsRunning) return;
        
        var now = Time.GetTicksMsec() / 1000.0;
        var splitTime = now - _levelStartTime;
        _splits.Add(splitTime);
        _levelStartTime = now;
        GD.Print($"Split recorded: {FormatTime(splitTime)}");
    }
    
    public double GetCurrentTotalTime() => IsRunning ? (Time.GetTicksMsec() / 1000.0) - _startTime : 0;
    public double GetCurrentLevelTime() => IsRunning ? (Time.GetTicksMsec() / 1000.0) - _levelStartTime : 0;
    
    public static string FormatTime(double time)
    {
        var span = System.TimeSpan.FromSeconds(time);
        return $"{(int)span.TotalMinutes:00}:{span.Seconds:00}.{span.Milliseconds:000}";
    }
}