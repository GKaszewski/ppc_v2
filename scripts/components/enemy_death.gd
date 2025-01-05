class_name EnemyDeathComponent
extends Node

@export var root: Node2D
@export var tween_duration: float = 0.5
@export var collision_shape_2d: CollisionShape2D
@export var health_component: HealthComponent

func _ready() -> void:
	if not collision_shape_2d:
		printerr("No CollisionShape2D assigned!")
		return
	if not health_component:
		printerr("No HealthComponent assigned!")
		return
		
	health_component.on_death.connect(_on_health_component_on_death)
		
func _on_health_component_on_death() -> void:
	call_deferred("die")
	
	
func die() -> void:
	collision_shape_2d.disabled = true
	var tween := create_tween()
	tween.tween_property(root, "scale", Vector2(0, 0), tween_duration)
	await (tween.finished)
	root.queue_free()