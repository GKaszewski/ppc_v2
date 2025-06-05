class_name PauseMenu
extends Node

@export var pause_menu_control: Control
@export var resume_button: Button
@export var quit_button: Button
@export var settings_button: Button
@export var exit_to_menu_button: Button
@export var settings_menu: Control
@export var exit_to_menu_scene: PackedScene

@onready var gm: GM = $"/root/GameManager"

var is_console_open: bool = false


func _ready() -> void:
	if not pause_menu_control:
		printerr("PauseMenu: Pause menu control not set.")
		return

	if not resume_button:
		printerr("PauseMenu: Resume button not set.")
		return

	if not quit_button:
		printerr("PauseMenu: Quit button not set.")
		return

	if not settings_button:
		printerr("PauseMenu: Settings button not set.")
		return

	if not exit_to_menu_button:
		printerr("PauseMenu: Exit to menu button not set.")
		return

	pause_menu_control.hide()

	resume_button.pressed.connect(_on_resume_button_pressed)
	quit_button.pressed.connect(_on_quit_button_pressed)
	settings_button.pressed.connect(_on_settings_button_pressed)
	exit_to_menu_button.pressed.connect(_on_exit_to_menu_button_pressed)
	Console.console_opened.connect(_on_console_open)
	Console.console_closed.connect(_on_console_close)


func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("pause") and not is_console_open:
		if UiManager.is_visible_on_stack(pause_menu_control):
			_on_resume_button_pressed()
		else:
			UiManager.push_screen(pause_menu_control)
			gm.pause_game()


func _on_resume_button_pressed() -> void:
	UiManager.pop_screen()
	gm.resume_game()


func _on_quit_button_pressed() -> void:
	gm.quit_game()


func _on_settings_button_pressed() -> void:
	if not settings_menu:
		printerr("PauseMenu: Settings menu scene not set.")
		return

	UiManager.push_screen(settings_menu)
	gm.pause_game()


func _on_exit_to_menu_button_pressed() -> void:
	if not exit_to_menu_scene:
		printerr("PauseMenu: Exit to menu scene not set.")
		return

	gm.resume_game()
	gm.reset_current_session_state()
	get_tree().change_scene_to_packed(exit_to_menu_scene)


func _on_console_open():
	is_console_open = true


func _on_console_close():
	is_console_open = false
