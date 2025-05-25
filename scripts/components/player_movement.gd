class_name PlayerMovement
extends Node

@export var type: String = ""

var enabled: bool              = true
var previous_velocity: Vector2 = Vector2.ZERO


func _process(_delta: float) -> void:
	if not enabled:
		return


func _physics_process(_delta: float) -> void:
	if not enabled:
		return