class_name SettingsMenu
extends Node

@export var input_settings: Control
@export var audio_settings: Control
@export var display_settings: Control
@export var gameplay_settings: Control
@export var settings_menu_control: Control
@export var input_settings_button: Button
@export var audio_settings_button: Button
@export var display_settings_button: Button
@export var gameplay_settings_button: Button


func _ready() -> void:
	if not settings_menu_control or not input_settings_button or not audio_settings_button or not display_settings_button or not gameplay_settings_button:
		printerr("No settings menu control or buttons found.")
		return

	if input_settings:
		input_settings_button.pressed.connect(_on_input_settings_button_pressed)
		input_settings.hide()

	if audio_settings:
		audio_settings_button.pressed.connect(_on_audio_settings_button_pressed)
		audio_settings.hide()

	if display_settings:

		display_settings_button.pressed.connect(_on_display_settings_button_pressed)
		display_settings.hide()

	if gameplay_settings:
		gameplay_settings_button.pressed.connect(_on_gameplay_settings_button_pressed)
		gameplay_settings.hide()


func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if settings_menu_control.is_visible():
			settings_menu_control.hide()


func _on_input_settings_button_pressed() -> void:
	if not input_settings:
		return

	if input_settings.is_visible():
		input_settings.hide()
	else:
		input_settings.show()
		audio_settings.hide()
		display_settings.hide()
		gameplay_settings.hide()


func _on_audio_settings_button_pressed() -> void:
	if not audio_settings:
		return

	if audio_settings.is_visible():
		audio_settings.hide()
	else:
		audio_settings.show()
		input_settings.hide()
		display_settings.hide()
		gameplay_settings.hide()


func _on_display_settings_button_pressed() -> void:
	if not display_settings:
		return

	if display_settings.is_visible():
		display_settings.hide()
	else:
		display_settings.show()
		input_settings.hide()
		audio_settings.hide()
		gameplay_settings.hide()


func _on_gameplay_settings_button_pressed() -> void:
	if not gameplay_settings:
		return

	if gameplay_settings.is_visible():
		gameplay_settings.hide()
	else:
		gameplay_settings.show()
		input_settings.hide()
		audio_settings.hide()
		display_settings.hide()
	
	
