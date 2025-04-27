class_name SaveSystem
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


func load_game():
	if not FileAccess.file_exists(save_path):
		return
	var file                  := FileAccess.open(save_path, FileAccess.READ)
	var save_data: Dictionary =  file.get_var()
	file.close()

	if save_data.has("version") and save_data["version"] != version:
		print("Save file version mismatch. Expected: ", version, ", Found: ", save_data["version"])
		return

	gm.player_state = save_data["player_state"]
	gm.unlock_skills(gm.player_state["unlocked_skills"])