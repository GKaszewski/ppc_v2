class_name FlipPlayerComponent
extends Node2D

@export var eye_left: Sprite2D
@export var eye_right: Sprite2D
@export var player_controller: PlayerController

func _process(_delta: float) -> void:
	var velocity := player_controller.last_direction
	if velocity.x < 0:
		eye_left.frame = 1
		eye_right.frame = 1
		eye_left.flip_h = true
		eye_right.flip_h = true
	elif velocity.x > 0:
		eye_left.frame = 1
		eye_right.frame = 1
		eye_left.flip_h = false
		eye_right.flip_h = false
	else:
		eye_left.frame = 0
		eye_right.frame = 0
		
