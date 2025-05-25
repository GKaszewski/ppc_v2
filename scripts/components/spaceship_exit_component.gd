class_name SpaceshipExitComponent
extends Node

@export var area2d: Area2D
signal spaceship_exited


func _ready() -> void:
	if not area2d:
		printerr("SpaceshipExitComponent: area2d is not set.")
		return

	area2d.body_entered.connect(_on_area2d_body_entered)


func _on_area2d_body_entered(body: Node2D) -> void:
	if not body is PlayerController:
		printerr("SpaceshipExitComponent: body is not a PlayerController.")
		return
	print("SpaceshipExitComponent: PlayerController exited spaceship.")
	spaceship_exited.emit()