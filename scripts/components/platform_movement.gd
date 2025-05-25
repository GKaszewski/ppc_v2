class_name PlatformMovement
extends PlayerMovement

@export var speed: float = 300.0
@export var jump_height: float = 100
@export var jump_time_to_peak: float = 0.5
@export var jump_time_to_descent: float = 0.4
@export var coyote_frames: int = 6
@export var jump_sfx: AudioStreamPlayer2D
@export var rotation_target: Node2D
@export var body: CharacterBody2D

var gravity        =  ProjectSettings.get_setting("physics/2d/default_gravity")
var was_last_floor := false
var coyote_mode    := false
var coyote_timer: Timer
var last_direction := Vector2.RIGHT

@onready var jump_velocity: float = ((2.0 * jump_height) / jump_time_to_peak) * -1.0
@onready var jump_gravity: float = ((-2.0 * jump_height) / (jump_time_to_peak * jump_time_to_peak)) * -1.0
@onready var fall_gravity: float = ((-2.0 * jump_height) / (jump_time_to_descent * jump_time_to_descent)) * -1.0


func _ready() -> void:
	if not body:
		return

	coyote_timer = Timer.new()
	coyote_timer.one_shot = true
	coyote_timer.wait_time = coyote_frames / 60.0
	coyote_timer.timeout.connect(on_coyote_timer_timeout)
	add_child(coyote_timer)


func _process(_delta: float) -> void:
	if not body or not enabled:
		return

	if body.velocity.x > 0.0:
		rotation_target.rotation = deg_to_rad(-10)
	elif body.velocity.x < 0.0:
		rotation_target.rotation = deg_to_rad(10)
	else:
		rotation_target.rotation = 0


func _physics_process(delta) -> void:
	if not body or not enabled:
		return

	if body.is_on_floor():
		was_last_floor = true
		coyote_mode = false  # Reset coyote mode when back on the floor
		coyote_timer.stop()  # Stop timer when grounded
	else:
		if was_last_floor: # Start coyote timer only once
			coyote_mode = true
			coyote_timer.start()
		was_last_floor = false

	if not body.is_on_floor():
		body.velocity.y += calculate_gravity() * delta

	if Input.is_action_pressed("jump") and (body.is_on_floor() or coyote_mode):
		jump()

	if Input.is_action_just_pressed("down"):
		body.position.y += 1

	var direction := Input.get_axis("left", "right")
	if direction != 0:
		last_direction = handle_direction(direction)

	if direction:
		body.velocity.x = direction * speed
	else:
		body.velocity.x = move_toward(body.velocity.x, 0, speed)

	previous_velocity = body.velocity
	body.move_and_slide()


func jump() -> void:
	if not body:
		return

	body.velocity.y = jump_velocity
	coyote_mode = false
	if jump_sfx:
		jump_sfx.play()


func calculate_gravity() -> float:
	return jump_gravity if body.velocity.y < 0.0 else fall_gravity


func on_coyote_timer_timeout() -> void:
	coyote_mode = false


func handle_direction(input_dir: float) -> Vector2:
	if input_dir > 0:
		return Vector2.RIGHT
	elif input_dir < 0:
		return Vector2.LEFT
	return last_direction
