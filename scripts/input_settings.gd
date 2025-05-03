class_name InputSettings
extends Control

@export var input_button_scene: PackedScene = preload("res://objects/ui/input_button.tscn")
@export var action_list: Container
@export var reset_to_default_button: Button

@export var input_actions: Dictionary = {
											'left': 'Move left',
											'right': 'Move right',
											'up': 'Move up',
											'down': 'Move down',
											'jump': 'Jump',
											'attack': 'Attack',
											'show_marketplace': "Toggle marketplace",
										}

var is_remapping: bool     = false
var action_to_remap        = null
var remapping_button       = null
var buttons: Array[Button] = []

@onready var config_file_handler = $'/root/ConfigFileHandler'


func _ready() -> void:
	create_action_list()

	if buttons.size() > 0:
		buttons[0].grab_focus()

	if reset_to_default_button:
		reset_to_default_button.pressed.connect(on_reset_button_pressed)


func _input(event: InputEvent) -> void:
	if is_remapping:
		if event is InputEventKey or (event is InputEventMouseButton and event.pressed) or event is InputEventJoypadButton:
			if event is InputEventMouseButton and event.double_click:
				event.double_click = false

			InputMap.action_erase_events(action_to_remap)
			InputMap.action_add_event(action_to_remap, event)
			update_action_list(remapping_button, event)

			is_remapping = false
			action_to_remap = null
			remapping_button = null

			accept_event()


func create_action_list() -> void:
	InputMap.load_from_project_settings()
	for item in action_list.get_children():
		item.queue_free()

	for action in input_actions:
		var button       := input_button_scene.instantiate() as Button
		var action_label := button.find_child("LabelAction") as Label
		var input_label  := button.find_child("LabelInput") as Label
		action_label.text  = input_actions[action]

		var events := InputMap.action_get_events(action)
		if events.size() > 0:
			input_label.text = events[0].as_text().trim_suffix(" (Physical)")
		else:
			input_label.text = "None"

		action_list.add_child(button)
		button.pressed.connect(on_input_button_pressed.bind(button, action))
		buttons.append(button)


func on_input_button_pressed(button: Button, action) -> void:
	if is_remapping:
		return

	is_remapping = true
	action_to_remap = action
	remapping_button = button
	button.find_child("LabelInput").text = "Press any key..."


func update_action_list(button: Button, event: InputEvent) -> void:
	button.find_child("LabelInput").text = event.as_text().trim_suffix(" (Physical)")


func on_reset_button_pressed() -> void:
	create_action_list()


func save_settings() -> void:
	config_file_handler.settings_config.set_value("input_settings", "input_actions", input_actions)
	config_file_handler.settings_config.save(config_file_handler.SETTINGS_FILE_PATH)


func load_settings() -> void:
	pass