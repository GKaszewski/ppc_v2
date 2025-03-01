class_name FireEffectComponent
extends Node

@export var duration: float = 1.0
@export var damage_per_second: float = 1.0
@export var health_component: HealthComponent
@export var root: Node2D

var timer: Timer
var should_deal_damage: bool = false
var time_elapsed: float      = 0.0


func _ready() -> void:
	if not health_component:
		health_component = root.get_node("HealthComponent")

	if not health_component:
		printerr("No HealthComponent assigned!")
		return

	timer = Timer.new()
	timer.timeout.connect(on_timer_timeout)
	prepare_timer()
	add_child(timer)


func _process(delta: float) -> void:
	if not should_deal_damage or not health_component:
		return

	time_elapsed += delta
	if time_elapsed >= 1.0:
		health_component.decrease_health(damage_per_second)
		time_elapsed = 0.0


func on_timer_timeout() -> void:
	deactivate()


func activate() -> void:
	should_deal_damage = true
	timer.start()


func deactivate() -> void:
	should_deal_damage = false
	timer.stop()


func prepare_timer() -> void:
	timer.set_wait_time(duration)
	timer.set_one_shot(true)
	timer.stop()
	time_elapsed = 0.0
