class_name ProjectileInitComponent
extends Node

@export var launch_component: LaunchComponent


func initialize(params: Dictionary) -> void:
	var position  := params.get("position", Vector2.ZERO) as Vector2
	var direction := params.get("direction", Vector2.RIGHT) as Vector2
	var rotation  := params.get("rotation", 0.0) as float
	var power     := params.get("power_multiplier", 1.0) as float

	owner.global_position = position
	owner.global_rotation = rotation

	if launch_component:
		launch_component.initial_direction = direction
		launch_component.spawn_position = position
		launch_component.spawn_rotation = rotation
		launch_component.speed *= power