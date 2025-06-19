class_name Marketplace
extends Node

@export var root: Control
@export var skill_data: Array[SkillData] = []
@export var to_unlock_grid: GridContainer
@export var unlocked_grid: GridContainer
@export var font: Font
@export var skill_unlocker: SkillUnlockerComponent
@export var components_to_disable: Array[Node] = []
@export var marketplace_button: PackedScene
@export var skill_button: PackedScene

@onready var game_manager: GM = $"/root/GameManager"


var unlock_buttons: Array[Button]     = []
var skill_buttons: Array[SkillButton] = []


func _ready() -> void:
	if not skill_unlocker:
		return

	var skills_to_unlock: Array[SkillData] = []

	for skill in skill_data:
		skills_to_unlock.append(skill)

	for skill in skills_to_unlock:
		create_upgrade_button(skill)

	var unlocked_skills := game_manager.get_unlocked_skills()
	for skill in unlocked_skills:
		create_skill_button(skill)

	skill_unlocker.skill_unlocked.connect(on_skill_unlocked)


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



func create_upgrade_button(skill: SkillData) -> void:
	var button := marketplace_button.instantiate() as MarketplaceButton
	button.text = get_button_text(skill)
	button.icon = skill.icon
	button.skill_data = skill

	button.pressed.connect(func () -> void: _on_button_pressed(skill))

	unlock_buttons.append(button)
	to_unlock_grid.add_child(button)
	to_unlock_grid.queue_sort()


func create_skill_button(skill: SkillData) -> void:
	var button := skill_button.instantiate() as SkillButton
	button.skill_data = skill
	button.setup()
	button.pressed.connect(func() -> void: on_skill_button_pressed(button))
	button.activate()

	skill_buttons.append(button)
	unlocked_grid.add_child(button)
	unlocked_grid.queue_sort()


func remove_button(skill: SkillData):
	for child in to_unlock_grid.get_children():
		if child.text == get_button_text(skill):
			child.queue_free()
			break


func _on_button_pressed(skill: SkillData) -> void:
	if not skill_unlocker:
		return

	if game_manager.is_skill_unlocked(skill):
		if skill.level < skill.max_level:
			skill_unlocker.try_upgrade_skill(skill)
			if not skill.is_active:
				skill_unlocker.skill_manager.toggle_skill_activation(skill)
		else:
			skill_unlocker.skill_manager.toggle_skill_activation(skill)
	else:
		skill_unlocker.try_unlock_skill(skill)


func on_skill_unlocked(skill: SkillData) -> void:
	# need to fix this method
	if not skill:
		return
	if skill_buttons.size() == 0:
		create_skill_button(skill)

	for button in skill_buttons:
		if button.skill_data.is_active:
			button.activate()
		else:
			button.deactivate()


func on_skill_button_pressed(button: SkillButton) -> void:
	if not skill_unlocker or not button.skill_data:
		return

	skill_unlocker.skill_manager.toggle_skill_activation(button.skill_data)
	button.activate()
	for other_button in skill_buttons:
		if other_button != button:
			other_button.deactivate()
		
