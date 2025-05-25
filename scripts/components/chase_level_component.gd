class_name ChaseLevelComponent
extends Node

@export var chase_speed: float = 200.0
@export var chase_target: Marker2D
@export var phantom_camera: PhantomCamera2D
@export var minimum_distance: float = 10.0
signal chase_started
signal chase_stopped
var is_chasing: bool                      = false
var previous_camera_follow_target: Node2D = null


func _process(delta: float) -> void:
	if not is_chasing:
		return

	if not chase_target:
		printerr("ChaseLevelComponent: chase_target is not set.")
		return

	if check_if_reached_target():
		stop_chasing()
		return

	var target_position: Vector2  = chase_target.global_position
	var current_position: Vector2 = owner.global_position
	var direction: Vector2        = (target_position - current_position).normalized()

	owner.global_position += direction * chase_speed * delta


func on_ship_entered() -> void:
	if not chase_target:
		printerr("ChaseLevelComponent: chase_target is not set.")
		return

	if not phantom_camera:
		printerr("ChaseLevelComponent: phantom_camera is not set.")
		return

	previous_camera_follow_target = phantom_camera.get_follow_target()
	phantom_camera.set_follow_target(owner as Node2D)
	chase_started.emit()
	is_chasing = true


func on_ship_exited() -> void:
	stop_chasing()


func check_if_reached_target() -> bool:
	if not chase_target:
		printerr("ChaseLevelComponent: chase_target is not set.")
		return false

	var target_position: Vector2  = chase_target.global_position
	var current_position: Vector2 = owner.global_position
	return current_position.distance_to(target_position) < minimum_distance


func stop_chasing() -> void:
	if not phantom_camera:
		printerr("ChaseLevelComponent: phantom_camera is not set.")
		return

	phantom_camera.set_follow_target(previous_camera_follow_target)
	chase_stopped.emit()
	is_chasing = false