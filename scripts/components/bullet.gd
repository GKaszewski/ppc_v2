class_name BulletComponent
extends Node

@export var root: Node2D
@export var direction: Vector2 = Vector2.RIGHT
@export var speed: float = 10.0
@export var area2d: Area2D
@export var visibility_notifier: VisibleOnScreenNotifier2D
@export var life_time: float = 5.0
@export var timer: Timer


func _ready() -> void:
	root = get_parent()
	visibility_notifier.screen_exited.connect(_on_screen_exited)
	area2d.body_entered.connect(on_area2d_body_entered)

	timer.wait_time = life_time
	timer.timeout.connect(on_timer_timeout)


func _physics_process(delta: float) -> void:
	root.position += direction * speed * delta


func _on_screen_exited() -> void:
	root.queue_free()


func on_area2d_body_entered(_body: Node2D) -> void:
	root.queue_free()


func on_timer_timeout() -> void:
	root.queue_free()