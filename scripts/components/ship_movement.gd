class_name ShipMovement
extends PlayerMovement

@export var max_speed: float = 200.0
@export var acceleration: float = 100.0
@export var friction: float = 50.0
@export var body: CharacterBody2D

var velocity: Vector2 = Vector2.ZERO


func _physics_process(delta: float) -> void:
	if not body or not enabled:
		return

	var input_vector := Vector2(
							Input.get_action_strength("right") - Input.get_action_strength("left"),
							Input.get_action_strength("down") - Input.get_action_strength("up")
						).normalized()

	if input_vector != Vector2.ZERO:
		velocity = velocity.move_toward(input_vector * max_speed, acceleration * delta)
	else:
		velocity = velocity.move_toward(Vector2.ZERO, friction * delta)

	velocity = velocity.limit_length(max_speed)
	body.velocity = velocity
	previous_velocity = body.velocity
	body.move_and_slide()
	