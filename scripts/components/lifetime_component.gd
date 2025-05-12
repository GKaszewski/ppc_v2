class_name LifetimeComponent
extends Node

@export var root: Node
@export var life_time: float = 5.0
@export var timer: Timer


func _ready() -> void:
	if not root:
		printerr("Root node not set.")
		return

	if not timer:
		printerr("Timer node not set.")
		return
		
	timer.timeout.connect(on_timer_timeout)


func on_timer_timeout() -> void:
	root.queue_free()
