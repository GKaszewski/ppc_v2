extends CharacterBody2D

@export var speed: float = 300.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

@onready var root = $Root

@export var jump_height: float = 100
@export var jump_time_to_peak: float = 0.5
@export var jump_time_to_descent: float = 0.4

@onready var jump_velocity: float = ((2.0 * jump_height) / jump_time_to_peak) * -1.0
@onready var jump_gravity: float = ((-2.0 * jump_height) / (jump_time_to_peak * jump_time_to_peak)) * -1.0
@onready var fall_gravity: float = ((-2.0 * jump_height) / (jump_time_to_descent * jump_time_to_descent)) * -1.0

func _process(_delta):
	if velocity.x > 0.0:
		root.rotation = deg_to_rad(-10)
	elif velocity.x < 0.0:
		root.rotation = deg_to_rad(10)
	else:
		root.rotation = 0
		
func _physics_process(delta):
	if not is_on_floor():
		velocity.y += calculate_gravity() * delta

	if Input.is_action_pressed("jump") and is_on_floor():
		jump()
		
	var direction = Input.get_axis("left", "right")
	if direction:
		velocity.x = direction * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
	
	move_and_slide()
	
func jump():
	velocity.y = jump_velocity
	
func calculate_gravity() -> float:
	return jump_gravity if velocity.y < 0.0 else fall_gravity
