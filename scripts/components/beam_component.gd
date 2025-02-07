class_name BeamComponent
extends Node2D

@export var expansion_speed: float = 16.0
@export var max_length: float = 512.0
@export var direction: Vector2 = Vector2.DOWN

var current_length: float = 16.0

@export var root: Node2D
@export var sprite2d: Sprite2D
@export var collision_shape: CollisionShape2D


func _ready() -> void:
	collision_shape.shape.extents = Vector2(current_length / 2.0, current_length / 2.0)
	sprite2d.scale = Vector2(1, 1)
	collision_shape.position = Vector2.ZERO


func _process(delta: float) -> void:
	var new_length = current_length + expansion_speed * delta
	if new_length > max_length:
		new_length = max_length

	if not check_for_obstacle(new_length):
		expand_beam(new_length)


func expand_beam(new_length: float) -> void:
	current_length = new_length

	if direction == Vector2.UP:
		sprite2d.scale.y = current_length / 16.0
		sprite2d.position.y = -current_length / 2.0
		collision_shape.shape.extents = Vector2(8.0, current_length / 2.0)
		collision_shape.position.y = -current_length / 2.0
	elif direction == Vector2.DOWN:
		sprite2d.scale.y = current_length / 16.0
		sprite2d.position.y = current_length / 2.0
		collision_shape.shape.extents = Vector2(8.0, current_length / 2.0)
		collision_shape.position.y = current_length / 2.0
	elif direction == Vector2.LEFT:
		sprite2d.scale.y = current_length / 16.0
		sprite2d.position.x = -current_length / 2.0
		collision_shape.shape.extents = Vector2(current_length / 2.0, 8.0)
		collision_shape.position.x = -current_length / 2.0
	elif direction == Vector2.RIGHT:
		sprite2d.scale.y = current_length / 16.0
		sprite2d.position.x = current_length / 2.0
		collision_shape.shape.extents = Vector2(current_length / 2.0, 8.0)
		collision_shape.position.x = current_length / 2.0


func check_for_obstacle(new_length: float) -> bool:
	var space_state: PhysicsDirectSpaceState2D = get_world_2d().direct_space_state
	var query_start: Vector2                   = global_position
	var query_end: Vector2                     = global_position + direction * new_length
	var query: PhysicsRayQueryParameters2D     = PhysicsRayQueryParameters2D.create(query_start, query_end)
	query.collide_with_areas = false
	query.collide_with_bodies = true

	var result: Dictionary = space_state.intersect_ray(query)
	return result.size() > 0
