class_name PlayerDeathComponent
extends Node2D

@export var death_sfx: AudioStreamPlayer2D
@export var death_effect: PackedScene

@onready var gm: GM = $"/root/GameManager"


func _on_health_component_on_death() -> void:
	if death_sfx:
		death_sfx.play()
	if death_effect:
		var effect: Node2D = death_effect.instantiate()
		get_parent().add_child(effect)
		effect.global_position = global_position
		effect.scale = Vector2(1.5, 1.5)

	gm.remove_lives(1)
	gm.reset_current_session_state()
		
