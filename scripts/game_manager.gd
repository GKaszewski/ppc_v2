class_name GM
extends Node

@export var level_scenes: Array[PackedScene]

@export var player_state := {
								"coins": 0,
								"lives": 3,
								"unlocked_skills": [],
								"current_level": 0,
								"unlocked_levels": [],
								"completed_levels": [],
							}

var nodes_in_scene := []


var current_session_state := {
								 "coins_collected": 0,
								 "skills_unlocked": [],
							 }


func _enter_tree() -> void:
	get_tree().node_added.connect(on_node_added)
	get_tree().node_removed.connect(on_node_removed)


func on_node_added(node: Node) -> void:
	nodes_in_scene.append(node)


func on_node_removed(node: Node) -> void:
	if node in nodes_in_scene:
		nodes_in_scene.erase(node)


func get_colllectable_nodes() -> Array[CollectableComponent]:
	var collectable_nodes: Array[CollectableComponent] = []
	for node in nodes_in_scene:
		var collectable_component: CollectableComponent = node.get_node_or_null("Collectable")
		if not collectable_component:
			collectable_component = node.get_node_or_null("CollectableComponent")
			if not collectable_component:
				continue
		collectable_nodes.append(collectable_component)
	return collectable_nodes


func get_coin_nodes() -> Array[CollectableComponent]:
	var coin_nodes := []
	for node in nodes_in_scene:
		var collectable_component: CollectableComponent = node.get_node_or_null("Collectable")
		if not collectable_component:
			continue
		if collectable_component.collectable_data.type == CollectableResource.CollectableType.COIN:
			coin_nodes.append(collectable_component)
	return coin_nodes


func get_kid_nodes() -> Array[CollectableComponent]:
	var kid_nodes := []
	for node in nodes_in_scene:
		var collectable_component: CollectableComponent = node.get_node_or_null("Collectable")
		if not collectable_component:
			continue
		if collectable_component.collectable_data.type == CollectableResource.CollectableType.KID:
			kid_nodes.append(collectable_component)
	return kid_nodes


func add_coins(amount: int) -> void:
	player_state["coins"] += amount
	player_state["coins"] = max(0, player_state["coins"])


func set_coins(amount: int) -> void:
	player_state["coins"] = amount


func get_coins() -> int:
	return player_state["coins"] + current_session_state["coins_collected"]


func remove_coins(amount: int) -> void:
	var session_coins = current_session_state["coins_collected"]

	if amount <= session_coins:
		current_session_state["coins_collected"] -= amount
	else:
		var remaining_amount = amount - session_coins
		current_session_state["coins_collected"] = 0
		player_state["coins"] = max(0, player_state["coins"] - remaining_amount)

	player_state["coins"] = max(0, player_state["coins"])


func add_lives(amount: int) -> void:
	player_state["lives"] += amount


func remove_lives(amount: int) -> void:
	player_state["lives"] -= amount


func set_lives(amount: int) -> void:
	player_state["lives"] = amount


func get_lives() -> int:
	return player_state["lives"]


func is_skill_unlocked(skill_name: String) -> bool:
	return skill_name in player_state["unlocked_skills"] or skill_name in current_session_state["skills_unlocked"]


func unlock_skill(skill_name: String) -> void:
	if not is_skill_unlocked(skill_name):
		player_state["unlocked_skills"].append(skill_name)


func remove_skill(skill_name: String) -> void:
	if is_skill_unlocked(skill_name):
		player_state["unlocked_skills"].erase(skill_name)


func unlock_skills(skill_names: Array[String]) -> void:
	for skill_name in skill_names:
		unlock_skill(skill_name)


func reset_player_state() -> void:
	player_state = {
		"coins": 0,
		"lives": 3,
		"unlocked_skills": [],
		"completed_levels": [],
	}


func unlock_level(level_index: int) -> void:
	if level_index not in player_state["unlocked_levels"]: player_state["unlocked_levels"].append(level_index)


func try_to_go_to_next_level() -> void:
	var next_level = player_state["current_level"] + 1
	if next_level < level_scenes.size() and next_level in player_state["unlocked_levels"]:
		player_state["current_level"] += 1
		get_tree().change_scene_to_packed(level_scenes[next_level])



func mark_level_complete(level_index: int) -> void:
	unlock_level(level_index + 1)
	if level_index not in player_state["completed_levels"]: player_state["completed_levels"].append(level_index)


func reset_current_session_state() -> void:
	current_session_state = {
		"coins_collected": 0,
		"skills_unlocked": [],
	}


func quit_game() -> void:
	get_tree().quit()


func pause_game() -> void:
	Engine.time_scale = 0


func resume_game() -> void:
	Engine.time_scale = 1


func start_new_game() -> void:
	reset_player_state()
	player_state["current_level"] = 0
	player_state["unlocked_levels"] = [0]  # Start with the first level unlocked
	get_tree().change_scene_to_packed(level_scenes[0])


func continue_game() -> void:
	if not SaveSystem.load_game():
		printerr("Failed to load game. Starting a new game instead.")
		start_new_game()
		return

	if player_state["current_level"] < level_scenes.size():
		get_tree().change_scene_to_packed(level_scenes[player_state["current_level"]])
	else:
		printerr("No levels unlocked to continue.")


func on_level_complete() -> void:
	var level_index = player_state["current_level"]
	mark_level_complete(level_index)
	add_coins(current_session_state["coins_collected"])
	for skill_name in current_session_state["skills_unlocked"]:
		unlock_skill(skill_name)

	reset_current_session_state()
	try_to_go_to_next_level()
	SaveSystem.save_game()
