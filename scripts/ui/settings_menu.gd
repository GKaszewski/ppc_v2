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



func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if UiManager.is_screen_on_top(settings_menu_control):
			UiManager.pop_screen()


func _on_input_settings_button_pressed() -> void:
	if not input_settings:
		return

	UiManager.push_screen(input_settings)


func _on_audio_settings_button_pressed() -> void:
	if not audio_settings:
		return

	UiManager.push_screen(audio_settings)


func _on_display_settings_button_pressed() -> void:
	if not display_settings:
		return

	UiManager.push_screen(display_settings)


func _on_gameplay_settings_button_pressed() -> void:
	if not gameplay_settings:
		return

	UiManager.push_screen(gameplay_settings)
	
	
