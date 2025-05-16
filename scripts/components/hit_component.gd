class_name HitComponent
extends Node

@export var sprite: Sprite2D
@export var health_component: HealthComponent
@export var hit_duration: float = 0.1
@export var hit_fx: GPUParticles2D
@export var flash_mode: bool = true


func _ready() -> void:
	if health_component:
		health_component.on_health_change.connect(on_health_change)
		health_component.on_death.connect(on_death)

	if not sprite:
		printerr("No sprite assigned!")
		return

	if sprite.material and flash_mode:
		sprite.material = sprite.material.duplicate()


func activate() -> void:
	if not flash_mode:
		return
	sprite.material.set_shader_parameter("enabled", true)


func deactivate() -> void:
	if not flash_mode:
		return
	sprite.material.set_shader_parameter("enabled", false)


func on_health_change(delta: float, total_health: float) -> void:
	if delta < 0:
		activate()
		await get_tree().create_timer(hit_duration).timeout
		deactivate()

		if total_health > 0:
			handle_hit_fx()


func on_death() -> void:
	activate()
	await get_tree().create_timer(hit_duration).timeout
	deactivate()


func handle_hit_fx() -> void:
	if not hit_fx:
		return

	hit_fx.restart()
	hit_fx.emitting = true
