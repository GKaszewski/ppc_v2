class_name CollectableComponent
extends Node

var root: Node
var has_fade_away: bool = false

@export var area2d: Area2D
@export var collision_shape: CollisionShape2D
@export var collectable_data: CollectableResource
@export var sfx: AudioStreamPlayer2D
signal collected(amount: Variant, type: CollectableResource.CollectableType, body: Node2D)


func _ready() -> void:
	if area2d:
		area2d.body_entered.connect(_on_area2d_body_entered)
	else:
		printerr("Collectable node missing Area2D child.")

	root = get_parent()

	if root.has_node("FadeAwayComponent"):
		has_fade_away = true


func _on_area2d_body_entered(body: Node2D) -> void:
	if body.has_node("CanPickUpComponent"):
		collected.emit(collectable_data.amount, collectable_data.type, body)
		if collision_shape:
			collision_shape.call_deferred("set_disabled", true)
		if sfx:
			sfx.play()
		if not has_fade_away and sfx:
			await sfx.finished
			root.queue_free()
		elif not has_fade_away:
			root.queue_free()

