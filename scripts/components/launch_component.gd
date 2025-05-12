class_name LaunchComponent
extends Node2D

@export var root: Node2D
@export var initial_direction: Vector2 = Vector2.RIGHT
@export var speed: float = 16.0
@export var spawn_position: Vector2 = Vector2.ZERO
@export var spawn_rotation: float = 0.0


func _ready() -> void:
	root.global_position = spawn_position
	root.global_rotation = spawn_rotation


func get_initial_velocity() -> Vector2:
	return initial_direction.normalized() * speed
