class_name Achievements
extends Node

func unlock_achievement(achievement_name: String) -> bool:
	if not Steam.setAchievement(achievement_name):
		print("Failed to unlock achievement: ", achievement_name)
		return false

	return true


func reset_achievement(achievement_name: String) -> bool:
	if not Steam.clearAchievement(achievement_name):
		print("Failed to reset achievement: ", achievement_name)
		return false

	return true