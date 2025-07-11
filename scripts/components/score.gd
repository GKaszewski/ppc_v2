class_name ScoreComponent
extends Node

@onready var game_manager: GM = $"/root/GameManager"


func _ready():
	await get_tree().process_frame
	var coins := get_tree().get_nodes_in_group("coins")

	for coin in coins:
		coin.collected.connect(on_collected)



func on_collected(amount: int, type: CollectableResource.CollectableType, _body: Node2D) -> void:
	if not game_manager:
		return
	if type != CollectableResource.CollectableType.COIN:
		return
	game_manager.current_session_state["coins_collected"] += amount
