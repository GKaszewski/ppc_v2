class_name ProjectileComponent
extends Node2D

@export var speed: float = 16.0
@export var dir: float
@export var spawn_position: Vector2
@export var spawn_rotation: float
@export var character_body: CharacterBody2D


func _ready() -> void:
	global_position = spawn_position
	global_rotation = spawn_rotation


func _physics_process(delta: float) -> void:
	if not character_body:
		return

	character_body.velocity = Vector2(0, -speed).rotated(dir)
	character_body.move_and_slide()