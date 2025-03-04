class_name DestroyableComponent
extends Node

@export var root: Node
@export var health_component: HealthComponent
@export var destroy_effect: PackedScene


func _ready() -> void:
	if not health_component:
		printerr("No health component assigned!")
		return

	health_component.on_death.connect(on_health_component_death)


func on_health_component_death() -> void:
	if destroy_effect:
		var effect: Node2D = destroy_effect.instantiate()
		health_component.get_parent().add_child(effect)
		effect.global_position = health_component.global_position

	root.queue_free()
