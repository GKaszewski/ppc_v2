class_name SteamIntegration
extends Node

var app_id: String         = "3575090"
var is_on_steam_deck: bool = false
var is_online: bool        = false
var has_bought_game: bool  = false


func _init() -> void:
	OS.set_environment("SteamAppId", app_id)
	OS.set_environment("SteamGameId", app_id)


func _ready() -> void:
	Steam.steamInit()
	Steam.enableDeviceCallbacks()
	SteamControllerInput.init()
	var is_running := Steam.isSteamRunning()

	if !is_running:
		print("Steam is not running.")
		return

	print("Steam is running.")

	is_on_steam_deck = Steam.isSteamRunningOnSteamDeck()
	is_online = Steam.loggedOn()
	has_bought_game = Steam.isSubscribed()

	if not has_bought_game:
		print("You have not bought the game.")
		get_tree().quit(69)
		