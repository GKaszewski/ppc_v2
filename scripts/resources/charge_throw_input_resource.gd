class_name ChargeThrowInputResource
extends ThrowInputResource

@export var min_power := 0.5
@export var max_power := 2.0
@export var max_charge_time := 1.5
@export var min_charge_duration := 0.1

var is_charging       := false
var charge_start_time := 0.0
signal charge_started
signal charge_updated(charge_ratio: float)
signal charge_stopped


func process_input(event: InputEvent) -> void:
	if event.is_action_pressed("attack"):
		is_charging = true
		charge_start_time = Time.get_ticks_msec() / 1000.0
		emit_signal("charge_started")

	if event.is_action_released("attack") and is_charging:
		var power := _calculate_power()
		is_charging = false
		emit_signal("throw_requested", power)
		emit_signal("charge_stopped")


func _calculate_power() -> float:
	var now       := Time.get_ticks_msec() / 1000.0
	var held_time := now - charge_start_time
	if held_time < min_charge_duration:
		return min_power
	var t = clamp(held_time / max_charge_time, 0.0, 1.0)
	return lerp(min_power, max_power, t)


func get_charge_ratio() -> float:
	if not is_charging:
		return min_power
	var now  := Time.get_ticks_msec() / 1000.0
	var held := now - charge_start_time
	var t    =  clamp(held / max_charge_time, 0.0, 1.0)
	return lerp(min_power, max_power, t)


func update(_delta: float) -> void:
	if not is_charging:
		return

	var t = clamp(get_charge_ratio(), min_power, max_power)
	emit_signal("charge_updated", t)


func supports_charging() -> bool:
	return true