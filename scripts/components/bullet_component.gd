class_name BulletComponent
extends Node

@export var root: Node2D
@export var area2d: Area2D


func _ready() -> void:
	area2d.body_entered.connect(on_area2d_body_entered)
	area2d.area_entered.connect(on_area2d_area_entered)


func on_area2d_body_entered(_body: Node2D) -> void:
	root.queue_free()


func on_area2d_area_entered(_area: Area2D) -> void:
	root.queue_free()
