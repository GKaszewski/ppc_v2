class_name SteamIntegration
extends Node

var app_id: String = "3575090"


func _init() -> void:
	OS.set_environment("STEAM_APP_ID", app_id)
	OS.set_environment("STEAM_GAME_ID", app_id)


func _ready() -> void:
	pass