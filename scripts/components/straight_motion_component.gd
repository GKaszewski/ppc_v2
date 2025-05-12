class_name StraightMotionComponent
extends Node

@export var root: Node2D
@export var launch_component: LaunchComponent


func _physics_process(delta: float) -> void:
	root.position += launch_component.get_initial_velocity() * delta
