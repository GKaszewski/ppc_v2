class_name PlayerDeathComponent
extends Node2D

@export var death_sfx: AudioStreamPlayer2D
@export var death_effect: PackedScene

@onready var gm: GM = $"/root/GameManager"


func reset_scene() -> void:
	get_tree().reload_current_scene()


func _on_health_component_on_death() -> void:
	if death_sfx:
		death_sfx.play()
	if death_effect:
		var effect: Node2D = death_effect.instantiate()
		get_parent().add_child(effect)
		effect.global_position = global_position
		effect.scale = Vector2(1.5, 1.5)

	if gm.get_lives() == 1:
		gm.reset_player_state()
	else:
		gm.remove_lives(1)
		gm.set_coins(0)

	call_deferred("reset_scene")
