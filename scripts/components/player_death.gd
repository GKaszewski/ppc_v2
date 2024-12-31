class_name PlayerDeathComponent
extends Node

func reset_scene() -> void:
	get_tree().reload_current_scene()

func _on_health_component_on_death() -> void:
	# todo: play audio on death
	call_deferred("reset_scene")
