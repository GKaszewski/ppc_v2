class_name GameOverScreen
extends Node

@export var game_over_screen: Control
@export var restart_button: Button
@export var main_menu_button: Button
@export var main_menu_scene: PackedScene

@onready var gm: GM = $"/root/GameManager"


func _ready() -> void:
	if not gm:
		return

	game_over_screen.hide()
	restart_button.pressed.connect(on_restart_button_pressed)
	main_menu_button.pressed.connect(on_main_menu_button_pressed)


func on_restart_button_pressed() -> void:
	if not gm:
		return

	gm.reset_player_state()
	get_tree().reload_current_scene()


func on_main_menu_button_pressed() -> void:
	if not gm or not main_menu_scene:
		return

	gm.reset_player_state()
	get_tree().change_scene_to(main_menu_scene)


func on_player_death() -> void:
	if not gm or not gm.get_lives() == 0:
		return

	game_over_screen.show()