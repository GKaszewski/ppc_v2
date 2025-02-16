class_name MagneticSkillComponent
extends Node

@export var magnetic_area: Area2D
@export var root: Node2D
@export var magnetic_move_duration: float = 1.25

var collectables_to_pickup: Array = []


func has_component_in_children(node: Node, component_name: String) -> bool:
	if not node:
		return false

	if node.has_node(component_name):
		return true

	for child in node.get_children():
		if has_component_in_children(child, component_name):
			return true

	return false


func _ready() -> void:
	if not magnetic_area:
		printerr("No magnetic_area assigned!")
		return

	magnetic_area.body_entered.connect(on_magnetic_area_body_entered)
	magnetic_area.area_entered.connect(on_magnetic_area_area_entered)


func _process(_delta: float) -> void:
	for collectable in collectables_to_pickup:
		if not is_instance_valid(collectable):
			collectables_to_pickup.erase(collectable)
			continue

		move_collectable_to_root(collectable)


func on_magnetic_area_body_entered(_body: Node2D) -> void:
	pass


func on_magnetic_area_area_entered(area: Area2D) -> void:
	if not has_component_in_children(area, "Collectable"):
		return

	if not collectables_to_pickup.has(area):
		collectables_to_pickup.append(area)


func move_collectable_to_root(collectable: Node2D) -> void:
	if not is_instance_valid(collectable):
		return

	var direction: Vector2 = (root.global_position - collectable.global_position).normalized()
	var speed: float       = direction.length() / magnetic_move_duration

	collectable.global_position += direction.normalized() * speed

