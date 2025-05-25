class_name SpinComponent
extends Node2D

@export var root: Node2D
@export var speed: float = 8.0
@export var rotation_direction: Vector2 = Vector2.RIGHT


func _process(delta: float) -> void:
	spin(delta)


func spin(delta: float) -> void:
	if not root:
		return

	var rotation_speed: float = speed * rotation_direction.x
	root.rotation += rotation_speed * delta