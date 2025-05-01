class_name GM
extends Node

var player_state = {
					   "coins": 1000,
					   "lives": 3,
					   "unlocked_skills": [],
				   }


func add_coins(amount: int) -> void:
	player_state["coins"] += amount


func set_coins(amount: int) -> void:
	player_state["coins"] = amount


func get_coins() -> int:
	return player_state["coins"]


func remove_coins(amount: int) -> void:
	player_state["coins"] -= amount


func add_lives(amount: int) -> void:
	player_state["lives"] += amount


func remove_lives(amount: int) -> void:
	player_state["lives"] -= amount


func set_lives(amount: int) -> void:
	player_state["lives"] = amount


func get_lives() -> int:
	return player_state["lives"]


func is_skill_unlocked(skill_name: String) -> bool:
	return skill_name in player_state["unlocked_skills"]


func unlock_skill(skill_name: String) -> void:
	if not is_skill_unlocked(skill_name):
		player_state["unlocked_skills"].append(skill_name)


func unlock_skills(skill_names: Array[String]) -> void:
	for skill_name in skill_names:
		unlock_skill(skill_name)