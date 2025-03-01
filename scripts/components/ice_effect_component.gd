class_name IceEffectComponent
extends Node

@export var duration: float = 1.0
@export var side_to_side_movement: SideToSideMovement
@export var periodic_shooting: PeriodicShootingComponent

var timer: Timer
var is_frozen: bool       = false
var movement_speed: float = 0.0


func _ready() -> void:
	timer = Timer.new()
	timer.timeout.connect(on_timer_timeout)
	prepare_timer()
	add_child(timer)

	if side_to_side_movement:
		movement_speed = side_to_side_movement.speed


func _process(_delta: float) -> void:
	if not side_to_side_movement or not periodic_shooting:
		return

	if is_frozen:
		side_to_side_movement.process_mode = PROCESS_MODE_DISABLED
		periodic_shooting.process_mode = PROCESS_MODE_DISABLED
	else:
		side_to_side_movement.process_mode = PROCESS_MODE_ALWAYS
		periodic_shooting.process_mode = PROCESS_MODE_ALWAYS


func on_timer_timeout() -> void:
	deactivate()


func activate() -> void:
	is_frozen = true
	timer.start()


func deactivate() -> void:
	is_frozen = false
	timer.stop()


func prepare_timer() -> void:
	timer.set_wait_time(duration)
	timer.set_one_shot(true)
	timer.stop()
