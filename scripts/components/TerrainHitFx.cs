using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class TerrainHitFx : Node
{
    private List<GpuParticles2D> _gpuParticles = [];

    public override void _Ready()
    {
        if (Owner is GpuParticles2D gpuParticle) _gpuParticles.Add(gpuParticle);

        foreach (var child in GetChildren())
        {
            if (child is GpuParticles2D p)
            {
                _gpuParticles.Add(p);
            }
        }
    }

    public void TriggerFx()
    {
        foreach (var fx in _gpuParticles.Where(fx => fx != null))
        {
            fx.Restart();
            fx.Emitting = true;
        }
    }
}