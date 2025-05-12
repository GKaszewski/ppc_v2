class_name ChargeThrowComponent
extends Node

@export var min_charge_duration: float = 0.1
@export var min_power: float = 0.5
@export var max_power: float = 2.0
@export var max_charge_time: float = 1.5

var charge_start_time: float = 0.0
var is_charging: bool        = false
signal charge_started
signal charge_updated(charge_ratio: float)
signal charge_stopped


func _process(_delta: float) -> void:
	if not is_charging:
		return

	var charge_ratio := get_charge_ratio()
	charge_updated.emit(charge_ratio)


func start_charging() -> void:
	is_charging = true
	charge_start_time = Time.get_ticks_msec() / 1000.0
	call_deferred("emit_charge_started")


func get_charge_ratio() -> float:
	if not is_charging:
		return 0.0

	var held_time := (Time.get_ticks_msec() / 1000.0) - charge_start_time
	var t         =  clamp(held_time / max_charge_time, 0.0, 1.0)

	return lerp(min_power, max_power, t)


func stop_charging() -> float:
	if not is_charging:
		return min_power

	var held_time := (Time.get_ticks_msec() / 1000.0) - charge_start_time
	is_charging = false
	charge_start_time = 0.0
	charge_stopped.emit()

	if held_time < min_charge_duration:
		return min_power

	var t = clamp(held_time / max_charge_time, 0.0, 1.0)
	return lerp(min_power, max_power, t)


func emit_charge_started() -> void:
	if not is_charging:
		return
	charge_started.emit()