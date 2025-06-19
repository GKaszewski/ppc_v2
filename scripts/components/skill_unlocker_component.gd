class_name SkillUnlockerComponent
extends Node

@export var skill_manager: SkillManager

@onready var game_manager: GM = $"/root/GameManager"
signal skill_unlocked(skill_data: SkillData)


func has_enough_coins(amount: int) -> bool:
	return game_manager and game_manager.get_coins() >= amount


func try_unlock_skill(skill_data: SkillData) -> bool:
	if not game_manager:
		return false

	if game_manager.is_skill_unlocked(skill_data):
		return false

	if not has_enough_coins(skill_data.cost):
		return false

	var skill: SkillData = skill_data
	skill.level = 1
	skill.is_active = true

	game_manager.remove_coins(skill.cost)
	game_manager.current_session_state["skills_unlocked"].append(skill)
	skill_manager.add_skill(skill)
	skill_unlocked.emit(skill)
	return true


func unlock_all_skills() -> void:
	var available_skills: Array[SkillData] = skill_manager.available_skills
	var skills: Array[String]              = []

	for skill in available_skills:
		skills.append(skill.name)
		skill_unlocked.emit(skill)

	game_manager.unlock_skills(available_skills)
	skill_manager.apply_unlocked_skills()



func try_upgrade_skill(skill_data: SkillData) -> bool:
	if not game_manager:
		return false

	if not game_manager.is_skill_unlocked(skill_data):
		return false

	if skill_data.level >= skill_data.max_level:
		return false

	if not has_enough_coins(skill_data.cost):
		return false

	game_manager.remove_coins(skill_data.cost)
	skill_data.level += 1
	skill_unlocked.emit(skill_data)
	return true
