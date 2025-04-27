class_name PlayerDeathComponent
extends Node

@onready var gm: GM = $"/root/GameManager"


func reset_scene() -> void:
	get_tree().reload_current_scene()


func _on_health_component_on_death() -> void:
	# todo: play audio on death
	gm.remove_lives(1)
	call_deferred("reset_scene")
