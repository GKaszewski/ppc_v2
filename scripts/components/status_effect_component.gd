class_name StatusEffectComponent
extends Node

enum EffectType {
	NONE,
	FIRE,
	ICE
}
var active_effect: StatusEffectDataResource = null
var timer: Timer
var is_active: bool                         = false
var time_elapsed: float                     = 0.0
signal effect_applied(effect_data: StatusEffectDataResource)
signal effect_removed(effect_type: EffectType)


func _ready() -> void:
	timer = Timer.new()
	timer.timeout.connect(on_timer_timeout)
	add_child(timer)


func apply_effect(effect_data: StatusEffectDataResource) -> void:
	if is_active:
		return

	is_active = true
	active_effect = effect_data
	prepare_timer()
	effect_applied.emit(active_effect)
	timer.start()


func remove_effect() -> void:
	is_active = false
	effect_removed.emit(active_effect.effect_type)


func on_timer_timeout() -> void:
	remove_effect()


func prepare_timer() -> void:
	timer.set_wait_time(active_effect.duration)
	timer.set_one_shot(true)
	timer.stop()
