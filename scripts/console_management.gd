class_name ConsoleManagement
extends Node

@export var player_health: HealthComponent
@export var skill_unlocker: SkillUnlockerComponent
@export var skill_manager: SkillManager

@onready var game_manager: GM = $"/root/GameManager"
@onready var achievements: Achievements = $"/root/AchievementsManager"


func _ready() -> void:
	Console.pause_enabled = true
	Console.add_command("add_coins", console_add_coins, ["amount"], 1, "Add coins to the player.")
	Console.add_command("set_coins", console_set_coins, ["amount"], 1, "Set the player's coins.")
	Console.add_command("set_lives", console_set_lives, ["amount"], 1, "Set the player's lives.")
	Console.add_command("set_health", console_set_health, ["amount"], 1, "Set the player's health.")
	Console.add_command("unlock_skill", console_unlock_skill, ["skill_name"], 1, "Unlock a skill for the player.")
	Console.add_command("remove_skill", console_remove_skill, ["skill_name"], 1, "Remove a skill from the player.")
	Console.add_command("remove_all_skills", console_remove_all_skills, [], 0, "Remove all skills from the player.")
	Console.add_command("unlock_all_skills", console_unlock_all_skills, [], 0, "Unlock all skills for the player.")
	Console.add_command("unlock_achievement", console_unlock_achievement, ["achievement_name"], 1, "Unlock an achievement for the player.")
	Console.add_command("reset_achievement", console_reset_achievement, ["achievement_name"], 1, "Reset an achievement for the player.")


func console_add_coins(amount: Variant) -> void:
	if not game_manager:
		return
	if not amount.is_valid_int():
		Console.print_error("Invalid amount: " + str(amount))
		return
	game_manager.add_coins(int(amount))
	Console.print_info("Added " + str(amount) + " coins.")


func console_set_coins(amount: Variant) -> void:
	if not game_manager:
		return
	if not amount.is_valid_int():
		Console.print_error("Invalid amount: " + str(amount))
		return
	game_manager.set_coins(int(amount))
	Console.print_info("Set coins to " + str(amount))


func console_set_lives(amount: Variant) -> void:
	if not game_manager:
		return
	if not amount.is_valid_int():
		Console.print_error("Invalid amount: " + str(amount))
		return
	game_manager.set_lives(int(amount))
	Console.print_info("Set lives to " + str(amount))


func console_set_health(amount: Variant) -> void:
	if not player_health:
		return
	if not amount.is_valid_float():
		Console.print_error("Invalid amount: " + str(amount))
		return
	player_health.set_health(float(amount))
	Console.print_info("Set health to " + str(amount))


func console_unlock_skill(skill_name: Variant) -> void:
	if not skill_manager or not skill_unlocker or not game_manager:
		return
	if not skill_name:
		Console.print_error("Invalid skill name: " + str(skill_name))
		return

	game_manager.unlock_skill(skill_name)
	var skill_data: SkillData = skill_manager.get_skill_by_name(skill_name)
	if not skill_data:
		Console.print_error("Skill not found: " + str(skill_name))
		return
	skill_manager.add_skill(skill_data)
	Console.print_info("Unlocked skill: " + str(skill_name))


func console_unlock_all_skills() -> void:
	if not skill_manager or not skill_unlocker or not game_manager:
		return

	skill_unlocker.unlock_all_skills()
	Console.print_info("Unlocked all skills.")


func console_remove_skill(skill_name: Variant) -> void:
	if not skill_manager or not skill_unlocker or not game_manager:
		return
	if not skill_name:
		Console.print_error("Invalid skill name: " + str(skill_name))
		return

	game_manager.remove_skill(skill_name)
	skill_manager.remove_skill(skill_name)
	Console.print_info("Removed skill: " + str(skill_name))


func console_remove_all_skills() -> void:
	if not skill_manager or not skill_unlocker or not game_manager:
		return

	for skill_name in skill_manager.active_components.keys():
		game_manager.remove_skill(skill_name)
		skill_manager.remove_skill(skill_name)


func console_unlock_achievement(achievement_name: Variant) -> void:
	if not achievement_name:
		Console.print_error("Invalid achievement name: " + str(achievement_name))
		return
	if not achievements:
		Console.print_error("Achievements manager not found.")
		return
	if not achievements.unlock_achievement(achievement_name):
		Console.print_error("Failed to unlock achievement: " + str(achievement_name))

	Console.print_info("Unlocked achievement: " + str(achievement_name))


func console_reset_achievement(achievement_name: Variant) -> void:
	if not achievement_name:
		Console.print_error("Invalid achievement name: " + str(achievement_name))
		return
	if not achievements:
		Console.print_error("Achievements manager not found.")
		return
	if not achievements.reset_achievement(achievement_name):
		Console.print_error("Failed to reset achievement: " + str(achievement_name))

	Console.print_info("Reset achievement: " + str(achievement_name))
