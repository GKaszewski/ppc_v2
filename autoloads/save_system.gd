extends Node

@export var save_path: String = "user://savegame.save"
@export var version: int = 1

@onready var gm: GM = $"/root/GameManager"


func save_game():
	var save_data := {
						 "player_state": gm.player_state,
						 "version": version,
					 }
	var file      := FileAccess.open(save_path, FileAccess.WRITE)
	file.store_var(save_data)
	file.close()
	print("Game saved to: ", save_path)


func load_game() -> bool:
	if not FileAccess.file_exists(save_path):
		return false
	var file                  := FileAccess.open(save_path, FileAccess.READ)
	var save_data: Dictionary =  file.get_var()
	file.close()

	if save_data.has("version") and save_data["version"] != version:
		print("Save file version mismatch. Expected: ", version, ", Found: ", save_data["version"])
		return false
	print("Game state loaded from: ", save_path)
	print("Player state: ", save_data["player_state"])
	gm.player_state = save_data["player_state"]
	var skills: Array[String] = []
	for skill_name in gm.player_state["unlocked_skills"]:
		skills.append(skill_name)

	gm.unlock_skills(skills)
	return true


func check_save_exists() -> bool:
	return FileAccess.file_exists(save_path)
