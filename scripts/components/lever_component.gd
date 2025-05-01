class_name LeverComponent
extends Node

@export var area2d: Area2D
@export var sprite2d: Sprite2D
@export var start_animation_index: int = 0
@export var animation_duration: float = 0.5
@export var sfx: AudioStreamPlayer2D
signal activated


func _ready() -> void:
	if not area2d:
		printerr("LeverComponent: area2d is not set.")
		return

	if not sprite2d:
		printerr("LeverComponent: sprite2d is not set.")
		return

	area2d.body_entered.connect(_on_body_entered)


func _on_body_entered(body: Node2D) -> void:
	var trigger_lever: TriggerLeverComponent = body.get_node_or_null("TriggerLeverComponent")
	if not trigger_lever:
		return

	activate()


func activate() -> void:
	activated.emit()
	if sfx:
		sfx.play()
	sprite2d.frame = start_animation_index + 1
	var timer := get_tree().create_timer(animation_duration)
	await timer.timeout
	sprite2d.frame = start_animation_index
