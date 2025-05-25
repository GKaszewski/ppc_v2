extends Node

@export var main_menu_control: Control
@export var new_game_button: Button
@export var continue_button: Button
@export var settings_button: Button
@export var credits_button: Button
@export var exit_button: Button
@export var version_label: Label
@export var settings_control: Control
@export var credits_control: Control

@onready var gm: GM = $"/root/GameManager"


func _ready() -> void:
	if new_game_button:
		new_game_button.pressed.connect(_on_new_game_button_pressed)
	if continue_button:
		continue_button.pressed.connect(_on_continue_button_pressed)
	if settings_button:
		settings_button.pressed.connect(_on_settings_button_pressed)
	if credits_button:
		credits_button.pressed.connect(_on_credits_button_pressed)
	if exit_button:
		exit_button.pressed.connect(quit_game)
	if version_label:
		version_label.text = "v. " + ProjectSettings.get_setting("application/config/version")

	if not SaveSystem.check_save_exists() and continue_button:
		continue_button.disabled = true
	else:
		continue_button.disabled = false


func _on_new_game_button_pressed() -> void:
	if gm:
		gm.start_new_game()
	else:
		printerr("GameManager not found. Cannot start new game.")


func _on_continue_button_pressed() -> void:
	if gm:
		gm.continue_game()
	else:
		printerr("GameManager not found. Cannot continue game.")


func quit_game() -> void:
	if gm:
		gm.quit_game()


func _on_settings_button_pressed() -> void:
	if settings_control:
		UiManager.push_screen(settings_control)


func _on_credits_button_pressed() -> void:
	if credits_control:
		UiManager.push_screen(credits_control)
