class_name ScoreComponent
extends Node

@onready var game_manager: GM = $"/root/GameManager"

func _ready():
	await get_tree().process_frame
	var coins = get_tree().get_nodes_in_group("coins")
	
	for coin in coins:
		coin.connect("collected", on_collected)

func on_collected(amount: int) -> void:
	if not game_manager:
		return

	game_manager.add_coins(amount)
	# todo: play sound
	# todo: update ui
