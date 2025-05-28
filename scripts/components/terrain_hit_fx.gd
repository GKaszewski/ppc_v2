class_name TerrainHitFx
extends Node

var gpu_particles: Array[GPUParticles2D] = []


func _ready() -> void:
	if owner is GPUParticles2D:
		gpu_particles.append(owner)

	for child in get_children():
		if child is GPUParticles2D:
			gpu_particles.append(child)


func trigger_fx() -> Signal:
	for fx in gpu_particles:
		if fx:
			fx.restart()
			fx.emitting = true
	return gpu_particles[0].finished
	

			