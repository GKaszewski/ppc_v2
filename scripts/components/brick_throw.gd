class_name BrickThrowComponent
extends Node

@export var brick_scene: PackedScene
@export var fire_rate: float = 1.0
@export var player_controller: PlayerController
@export var timer: Timer

var can_throw: bool = true

func _ready() -> void:
	setup_timer()
	can_throw = true
	
	
func _input(event: InputEvent) -> void:
	if event.is_action_pressed("attack") and can_throw:
		throw_brick()
	
func setup_timer() -> void:
	timer.wait_time = fire_rate
	timer.one_shot = false
	timer.autostart = false
	timer.timeout.connect(on_timer_timeout)

	
func on_timer_timeout() -> void:
	can_throw = true
	
	
func throw_brick() -> void:
	var brick_instance: Node2D = brick_scene.instantiate()
	var brick: BulletComponent = brick_instance.get_node("BulletComponent")
	brick_instance.position = player_controller.position
	brick.direction = Vector2.RIGHT if player_controller.velocity.x >= 0.0 else Vector2.LEFT
	get_tree().current_scene.add_child(brick_instance)
	
	can_throw = false
	timer.start()
