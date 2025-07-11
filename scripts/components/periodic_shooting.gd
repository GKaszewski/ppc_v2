class_name PeriodicShootingComponent
extends Node

@export var bullet_scene: PackedScene = preload("res://objects/entities/bullet.tscn")
@export var shoot_interval: float = 1.0
@export var shoot_direction: Vector2 = Vector2.RIGHT
@export var side_to_side_movement: SideToSideMovement
@export var root: Node2D
@export var bullet_spawn_right: Node2D
@export var bullet_spawn_left: Node2D
@export var shooting_interval_variation: float = 0.0

var timer: Timer


func _ready() -> void:
	root = get_parent()
	setup_timer()


func _process(_delta: float) -> void:
	if side_to_side_movement:
		shoot_direction = side_to_side_movement.direction if side_to_side_movement.direction != Vector2.ZERO else Vector2.RIGHT


func shoot() -> void:
	if shoot_direction == Vector2.ZERO:
		return

	var bullet_instance: Node2D           = bullet_scene.instantiate()
	var launch_component: LaunchComponent = bullet_instance.get_node_or_null("LaunchComponent")
	var spawn_position: Vector2           = bullet_spawn_right.global_position if shoot_direction == Vector2.RIGHT else bullet_spawn_left.global_position
	if launch_component:
		launch_component.initial_direction = shoot_direction
		launch_component.spawn_position = spawn_position
		launch_component.spawn_rotation = root.rotation

	bullet_instance.position = spawn_position
	get_tree().current_scene.add_child(bullet_instance)


func on_timer_timeout() -> void:
	shoot()
	timer.start()



func get_shoot_interval() -> float:
	if shooting_interval_variation > 0.0:
		return shoot_interval + randf_range(-shooting_interval_variation, shooting_interval_variation)
	return shoot_interval


func setup_timer() -> void:
	timer = Timer.new()
	timer.wait_time = get_shoot_interval()
	timer.one_shot = false
	timer.autostart = true
	timer.timeout.connect(on_timer_timeout)
	add_child(timer)
