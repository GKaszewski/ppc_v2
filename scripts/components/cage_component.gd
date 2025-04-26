class_name CageComponent
extends Node

@export var lever: LeverComponent
@export var root: Node2D
@export var move_value: Vector2 = Vector2(0, -100)
@export var tween_duration: float = 0.5
@export var should_free: bool = true


func _ready() -> void:
	await get_tree().process_frame
	if not lever:
		var levers_nodes := get_tree().get_nodes_in_group("levers")
		for lever_node in levers_nodes:
			var lever_component: LeverComponent = lever_node.get_node_or_null("LeverComponent")
			if lever_component:
				lever_component.activated.connect(on_lever_activated)
	else:
		lever.activated.connect(on_lever_activated)

	if not root:
		printerr("CageComponent: root is not set.")
		return


func on_lever_activated() -> void:
	var tween: Tween          = create_tween()
	var end_position: Vector2 = root.position + move_value
	tween.tween_property(root, "position", end_position, tween_duration)
	tween.tween_callback(_on_tween_completed)


func _on_tween_completed() -> void:
	if not should_free:
		return
	root.queue_free()