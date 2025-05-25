class_name BrickThrowComponent
extends Node

@export var brick_scene: PackedScene
@export var fire_rate: float = 1.0
@export var player_controller: PlayerController
@export var timer: Timer
@export var throw_input_behavior: ThrowInputResource

var can_throw: bool = true


func _ready() -> void:
	setup_timer()
	can_throw = true

	if throw_input_behavior:
		throw_input_behavior.throw_requested.connect(throw_brick)


func _input(event: InputEvent) -> void:
	if throw_input_behavior:
		throw_input_behavior.process_input(event)


func _process(delta: float) -> void:
	if throw_input_behavior:
		throw_input_behavior.update(delta)


func setup_timer() -> void:
	timer.wait_time = fire_rate
	timer.one_shot = false
	timer.autostart = false
	timer.timeout.connect(on_timer_timeout)


func on_timer_timeout() -> void:
	can_throw = true


func throw_brick(power_multiplier: float = 1.0) -> void:
	var instance: Node2D =  brick_scene.instantiate()
	var init             := instance.get_node_or_null("ProjectileInitComponent") as ProjectileInitComponent
	if init and player_controller.current_movement is PlatformMovement:
		init.initialize({
			"position": player_controller.global_position,
			"rotation": player_controller.rotation,
			"direction": player_controller.current_movement.last_direction,
			"power_multiplier": power_multiplier
		})

	get_tree().current_scene.add_child(instance)

	can_throw = false
	timer.start()
