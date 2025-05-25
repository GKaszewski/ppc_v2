class_name SpaceshipEnterComponent
extends Node

@export var area2d: Area2D
signal spaceship_entered


func _ready() -> void:
	if not area2d:
		printerr("SpaceshipEnterComponent: area2d is not set.")
		return

	area2d.body_entered.connect(_on_area2d_body_entered)


func _on_area2d_body_entered(body: Node2D) -> void:
	if not body is PlayerController:
		return

	spaceship_entered.emit()
	owner.queue_free()