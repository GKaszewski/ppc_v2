class_name SideToSideMovement
extends Node

@export var root: Node2D
@export var sprite2d: Sprite2D
@export var speed: float = 10.0
@export var wait_time: float = 1.0
@export var left_ray: RayCast2D
@export var right_ray: RayCast2D

var direction: Vector2 = Vector2.LEFT
var new_direction: Vector2 = Vector2.LEFT
var timer: Timer
var triggered_direction_change: bool = false

signal direction_changed()

func _ready() -> void:
	root = get_parent()
	
	if not sprite2d:
		print("SideToSideMovement node missing Sprite2D child.")
		return

	if not left_ray or not right_ray:
		print("SideToSideMovement node missing RayCast2D children.")
		return

	setup_timer()
	direction_changed.connect(on_direction_changed)
		

func _physics_process(delta: float) -> void:
	handle_direction()
	handle_sprite_flip()
	handle_movement(delta)
	
func handle_direction() -> void:
	# we are not colliding with anything, which means we don't have ground to walk on. Stop moving.
	if not left_ray.is_colliding() and not right_ray.is_colliding():
		new_direction = Vector2.ZERO
		return
		
	# If the left ray is not colliding and the right ray is colliding, that means we have ground to the right and we should change direction to the right.
	if not left_ray.is_colliding() and right_ray.is_colliding():
		new_direction = Vector2.RIGHT
		direction_changed.emit()
		return
	if not right_ray.is_colliding() and left_ray.is_colliding():
		new_direction = Vector2.LEFT
		direction_changed.emit()
		return
		
func handle_sprite_flip() -> void:
	if direction == Vector2.LEFT:
		sprite2d.flip_h = true
	else:
		sprite2d.flip_h = false

func handle_movement(delta: float) -> void:
	root.position += direction * speed * delta
		
func on_direction_changed() -> void:
	if direction == new_direction or triggered_direction_change:
		return
	triggered_direction_change = true
	direction = Vector2.ZERO
	timer.start()

func on_timer_timeout() -> void:
	timer.stop()
	direction = new_direction
	triggered_direction_change = false
	
	
func setup_timer() -> void:
	timer = Timer.new()
	add_child(timer)
	timer.wait_time = wait_time
	timer.one_shot = true
	timer.timeout.connect(on_timer_timeout)
	