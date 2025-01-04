class_name FadeAwayComponent
extends Node

@export var sprite2d: Sprite2D
@export var fade_duration: float = 1.0
@export var speed: float = 10.0
@export var direction: Vector2 = Vector2.UP
@export var root: Node2D
@export var area2d: Area2D


func _ready():
	root = get_parent()
	if area2d:
		area2d.body_entered.connect(on_area2d_body_entered)


func fade_away() -> void:
	var fade_tween := create_tween().set_parallel(true)
	fade_tween.tween_property(sprite2d, "modulate:a", 0, fade_duration)
	fade_tween.tween_property(sprite2d, "position", sprite2d.position + (direction * speed), fade_duration)
	await (fade_tween.finished)
	root.queue_free()


func on_area2d_body_entered(_body: Node2D) -> void:
	fade_away()
