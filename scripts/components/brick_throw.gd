class_name BrickThrowComponent
extends Node

@export var brick_scene: PackedScene
@export var fire_rate: float = 1.0
@export var player_controller: PlayerController
@export var timer: Timer
@export var charge_throw_component: ChargeThrowComponent

var can_throw: bool = true


func _ready() -> void:
	setup_timer()
	can_throw = true


func _input(event: InputEvent) -> void:
	if event.is_action_pressed("attack") and can_throw and charge_throw_component:
		charge_throw_component.start_charging()

	if event.is_action_released("attack") and can_throw and charge_throw_component:
		var power_multiplier: float = charge_throw_component.stop_charging()
		throw_brick(power_multiplier)


func setup_timer() -> void:
	timer.wait_time = fire_rate
	timer.one_shot = false
	timer.autostart = false
	timer.timeout.connect(on_timer_timeout)


func on_timer_timeout() -> void:
	can_throw = true


func throw_brick(power_multiplier: float = 1.0) -> void:
	var brick_instance: Node2D = brick_scene.instantiate()

	var launch_component    := brick_instance.get_node_or_null("LaunchComponent") as LaunchComponent
	var chargable_component := brick_instance.get_node_or_null("ChargableComponent") as ChargableComponent
	if launch_component:
		launch_component.initial_direction = player_controller.last_direction
		launch_component.spawn_position = player_controller.global_position
		launch_component.spawn_rotation = player_controller.rotation
		launch_component.speed *= power_multiplier if chargable_component else 1.0

	brick_instance.global_position = player_controller.global_position
	get_tree().current_scene.add_child(brick_instance)

	can_throw = false
	timer.start()
