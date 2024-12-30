class_name PlayerController
extends CharacterBody2D

@export var speed: float = 300.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

@onready var root = $Root
@onready var coyote_timer: Timer = $CoyoteTimer

@export var jump_height: float = 100
@export var jump_time_to_peak: float = 0.5
@export var jump_time_to_descent: float = 0.4

@export var coyote_frames: int = 6
@export var coyote_mode: bool = false
@export var was_last_floor: bool = false

@onready var jump_velocity: float = ((2.0 * jump_height) / jump_time_to_peak) * -1.0
@onready var jump_gravity: float = ((-2.0 * jump_height) / (jump_time_to_peak * jump_time_to_peak)) * -1.0
@onready var fall_gravity: float = ((-2.0 * jump_height) / (jump_time_to_descent * jump_time_to_descent)) * -1.0

func _ready() -> void:
	coyote_timer.timeout.connect(on_coyote_timer_timeout)
	coyote_timer.wait_time = coyote_frames / 60.0

func _process(_delta):
	if velocity.x > 0.0:
		root.rotation = deg_to_rad(-10)
	elif velocity.x < 0.0:
		root.rotation = deg_to_rad(10)
	else:
		root.rotation = 0
		
func _physics_process(delta):
	if is_on_floor():
		was_last_floor = true
		coyote_mode = false  # Reset coyote mode when back on the floor
		coyote_timer.stop()  # Stop timer when grounded
	else:
		if was_last_floor:  # Start coyote timer only once
			coyote_mode = true
			coyote_timer.start()
		was_last_floor = false
	
	if not is_on_floor():
		velocity.y += calculate_gravity() * delta

	if Input.is_action_pressed("jump") and (is_on_floor() or coyote_mode):
		jump()
	
	if Input.is_action_just_pressed("down"):
		position.y += 1
		
	var direction = Input.get_axis("left", "right")
	if direction:
		velocity.x = direction * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
	
	move_and_slide()
	
func jump():
	velocity.y = jump_velocity
	coyote_mode = false
	
func calculate_gravity() -> float:
	return jump_gravity if velocity.y < 0.0 else fall_gravity

func on_coyote_timer_timeout():
	coyote_mode = false
