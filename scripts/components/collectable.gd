class_name CollectableComponent
extends Node

var root: Node
var has_fade_away: bool = false

@export var area2d: Area2D
@export var collectable_data: CollectableResource
@export var sfx: AudioStreamPlayer2D
signal collected(amount: int)


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
		collected.emit(collectable_data.amount)
		if sfx:
			sfx.play()
		if not has_fade_away:
			await sfx.finished
			root.queue_free()
