class_name CleanUpComponent
extends Node

@export var root: Node


func clean_up() -> void:
	root.queue_free()