class_name Marketplace
extends Node

@export var root: Control
@export var skill_data: Array[SkillData] = []
@export var grid: GridContainer
@export var font: Font
@export var skill_unlocker: SkillUnlockerComponent
@export var components_to_disable: Array[Node] = []
@export var marketplace_button: PackedScene

@onready var game_manager: GM = $"/root/GameManager"

var buttons: Array[Button] = []


func _ready() -> void:
	if not skill_unlocker:
		return

	var skills_to_unlock: Array[SkillData] = []

	for skill in skill_data:
		if skill.name in game_manager.player_state['unlocked_skills']:
			continue
		skills_to_unlock.append(skill)

	for skill in skills_to_unlock:
		create_upgrade_button(skill)


func _input(event: InputEvent) -> void:
	if event.is_action_pressed("show_marketplace"):
		if root.is_visible():
			root.hide()
			for component in components_to_disable:
				component.process_mode = PROCESS_MODE_INHERIT
		else:
			root.show()
			for component in components_to_disable:
				component.process_mode = PROCESS_MODE_DISABLED


func get_button_text(skill: SkillData) -> String:
	return tr(skill.name) + " " + str(skill.cost)


func create_upgrade_button(skill: SkillData):
	var button := marketplace_button.instantiate() as Button
	button.text = get_button_text(skill)
	button.icon = skill.icon

	button.pressed.connect(func () -> void: _on_button_pressed(skill))

	buttons.append(button)
	grid.add_child(button)
	grid.queue_sort()


func remove_button(skill: SkillData):
	for child in grid.get_children():
		if child.text == get_button_text(skill):
			child.queue_free()
			break


func _on_button_pressed(skill: SkillData) -> void:
	if not skill_unlocker:
		return

	if skill_unlocker.try_unlock_skill(skill):
		remove_button(skill)
		
