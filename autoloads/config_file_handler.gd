extends Node

var settings_config := ConfigFile.new()
const SETTINGS_PATH := "user://settings.ini"


func _ready() -> void:
	if !FileAccess.file_exists(SETTINGS_PATH):
		settings_config.save(SETTINGS_PATH)
	else:
		settings_config.load(SETTINGS_PATH)