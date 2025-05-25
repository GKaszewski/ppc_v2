class_name PlayerController
extends CharacterBody2D

@export var default_movement_type: String = "platform"
@export var movement_types: Dictionary = {}

var current_movement: PlayerMovement = null


func _ready() -> void:
	for movement_type in movement_types:
		var movement_node: Node = get_node_or_null(movement_types[movement_type])
		if movement_node and movement_node is PlayerMovement:
			movement_node.enabled = false

	switch_movement(default_movement_type)


func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventKey:
		if event.is_action_pressed("switch_movement"):
			var next_movement_type: String = get_next_movement_type()
			switch_movement(next_movement_type)


func switch_movement(movement_type: String) -> void:
	if current_movement:
		current_movement.enabled = false

	if movement_type in movement_types:
		current_movement = get_node_or_null(movement_types[movement_type])
		if not current_movement:
			push_error("Movement type '%s' not found in movement_types." % movement_type)
			return
		current_movement.enabled = true
	else:
		push_error("Movement type '%s' not found in movement_types." % movement_type)

	if not current_movement:
		push_error("No current movement set after switching.")


func get_next_movement_type() -> String:
	var keys: Array = movement_types.keys()
	var current_index: int = keys.find(current_movement.type)
	if current_index == -1:
		return default_movement_type

	current_index = (current_index + 1) % keys.size()
	return keys[current_index]
