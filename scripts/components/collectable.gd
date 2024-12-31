class_name CollectableComponent
extends Node

var root: Node

@export var area2d: Area2D
@export var collectable_data: Resource

signal collected(amount: int)

func _ready() -> void:
	if area2d:
		area2d.body_entered.connect(_on_area2d_body_entered)
	else:
		print("Collectable node missing Area2D child.")

	root = get_parent()

func _on_area2d_body_entered(body: Node2D) -> void:
	if body.has_node("CanPickUpComponent"):
		collected.emit(collectable_data.amount)
		root.queue_free()
