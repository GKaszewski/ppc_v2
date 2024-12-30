class_name GM
extends Node

var player_state = {
	"coins": 0,
}

func add_coins(amount: int) -> void:
	player_state["coins"] += amount

func set_coins(amount: int) -> void:
	player_state["coins"] = amount

func get_coins() -> int:
	return player_state["coins"]

func remove_coins(amount: int) -> void:
	player_state["coins"] -= amount