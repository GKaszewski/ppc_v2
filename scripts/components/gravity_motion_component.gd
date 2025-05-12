class_name GravityMotionComponent
extends Node2D

@export var character_body: CharacterBody2D
@export var launch_component: LaunchComponent
@export var gravity: Vector2 = Vector2(0, 980.0)
@export var target_direction: Vector2 = Vector2(1.0, -1.0)

var velocity: Vector2 = Vector2.ZERO


func _ready() -> void:
	if not launch_component:
		return
	var direction := target_direction if launch_component.initial_direction.x > 0 else Vector2(-target_direction.x, target_direction.y)
	direction = direction.normalized()
	velocity = direction * launch_component.speed


func _physics_process(delta: float) -> void:
	if not character_body:
		return

	velocity += gravity * delta
	character_body.velocity = velocity

	character_body.move_and_slide()

	if velocity.length_squared() > 0.01:
		character_body.rotation = velocity.angle()