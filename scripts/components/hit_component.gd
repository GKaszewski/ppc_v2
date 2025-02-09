class_name HitComponent
extends Node

@export var sprite: Sprite2D
@export var health_component: HealthComponent
@export var hit_duration: float = 0.1


func _ready() -> void:
	if health_component:
		health_component.on_health_change.connect(on_health_change)

	if not sprite:
		printerr("No sprite assigned!")
		return

	if sprite.material:
		sprite.material = sprite.material.duplicate()


func activate() -> void:
	sprite.material.set_shader_parameter("enabled", true)


func deactivate() -> void:
	sprite.material.set_shader_parameter("enabled", false)


func on_health_change(delta: float, _total_health: float) -> void:
	if delta < 0:
		activate()
		await get_tree().create_timer(hit_duration).timeout
		deactivate()
