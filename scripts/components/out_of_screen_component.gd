class_name OutOfScreenComponent
extends Node

@export var visibility_notifier: VisibleOnScreenNotifier2D
@export var root: Node


func _ready() -> void:
	if not visibility_notifier:
		printerr("Visibility notifier not set.")
		return

	visibility_notifier.screen_exited.connect(_on_screen_exited)


func _on_screen_exited() -> void:
	if not root:
		printerr("Root node not set.")
		return

	root.queue_free()
