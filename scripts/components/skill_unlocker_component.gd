class_name SkillUnlockerComponent
extends Node

@export var skill_manager: SkillManager

@onready var game_manager: GM = $"/root/GameManager"


func try_unlock_skill(skill_data: SkillData) -> bool:
	if not game_manager:
		return false

	if game_manager.is_skill_unlocked(skill_data.name):
		return false

	if game_manager.get_coins() < skill_data.cost:
		return false

	game_manager.remove_coins(skill_data.cost)
	game_manager.unlock_skill(skill_data.name)
	skill_manager.add_skill(skill_data)
	return true


func unlock_all_skills() -> void:
	var available_skills: Array[SkillData] = skill_manager.available_skills
	var skills: Array[String]              = []

	for skill in available_skills:
		skills.append(skill.name)

	game_manager.unlock_skills(skills)
	skill_manager.apply_unlocked_skills()


func _input(event: InputEvent) -> void:
	if event.is_action_pressed("unlock_skills"):
		unlock_all_skills()