class_name CollapsableComponent
extends Node

@export var to_collapse_timer: Timer
@export var reset_timer: Timer
@export var sprite2d: Sprite2D
@export var collision_shape: CollisionShape2D

@export var collapse_time: float = 0.5
@export var reset_time: float = 1.0

@export var anim_time: float = 0.25

func _ready() -> void:
	reset_timers()

func _on_to_collapse_timer_timeout() -> void:
	collapse_bridge()

func _on_reset_timer_timeout() -> void:
	reactivate_bridge()
	
func collapse_bridge():
	to_collapse_timer.stop()
	to_collapse_timer.wait_time = collapse_time
	
	var bridge_tween = create_tween()
	bridge_tween.tween_property(sprite2d, "modulate:a", 0, anim_time)
	await bridge_tween.finished
	
	collision_shape.disabled = true
	reset_timer.start()
	
func reactivate_bridge():
	reset_timer.stop()
	reset_timer.wait_time = reset_time
	
	var bridge_tween = create_tween()
	bridge_tween.tween_property(sprite2d, "modulate:a", 1, anim_time)
	await bridge_tween.finished
	
	collision_shape.disabled = false

func _on_collapsable_detector_body_entered(_body: Node2D) -> void:
	to_collapse_timer.start()

func reset_timers():
	to_collapse_timer.stop()
	to_collapse_timer.wait_time = collapse_time


func _on_collapsable_detector_body_exited(_body: Node2D) -> void:
	var collapse_time_left: float = abs(to_collapse_timer.time_left - collapse_time)
	if collapse_time_left < (0.1 * collapse_time):
		reset_timers()
