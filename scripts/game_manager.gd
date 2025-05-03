class_name GM
extends Node

@export var level_scenes: Array[PackedScene]

@export var player_state := {
								"coins": 0,
								"lives": 3,
								"unlocked_skills": [],
								"current_level": 0,
								"unlocked_levels": [],
							}

var nodes_in_scene := []


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


func reset_player_state() -> void:
	player_state = {
		"coins": 0,
		"lives": 3,
		"unlocked_skills": [],
	}


func unlock_level(level_index: int) -> void:
	if level_index not in player_state["unlocked_levels"]: player_state["unlocked_levels"].append(level_index)


func try_to_go_to_next_level() -> void:
	if player_state["current_level"] + 1 < level_scenes.size() and player_state["current_level"] + 1 in player_state["unlocked_levels"]:
		player_state["current_level"] += 1


func quit_game() -> void:
	get_tree().quit()


func pause_game() -> void:
	Engine.time_scale = 0


func resume_game() -> void:
	Engine.time_scale = 1