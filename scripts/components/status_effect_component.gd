class_name StatusEffectComponent
extends Node

enum EffectType {
	NONE,
	FIRE,
	ICE
}
var active_effects: Array = []
signal effect_applied(effect_data: StatusEffectDataResource)
signal effect_removed(effect_type: EffectType)


func apply_effect(effect_data: StatusEffectDataResource) -> void:
	var data: StatusEffectDataResource = effect_data.duplicate()

	var timer: Timer = create_timer(effect_data.duration, data)

	var new_effect: Dictionary = {
									 "data": data,
									 "elapsed_time": 0.0,
									 "timer": timer
								 }

	active_effects.append(new_effect)
	effect_applied.emit(effect_data)


func remove_effect(effect_data: StatusEffectDataResource) -> void:
	for i in range(active_effects.size()):
		if active_effects[i].data == effect_data:
			active_effects[i].timer.queue_free()
			active_effects.remove_at(i)
			effect_removed.emit(effect_data.effect_type)
			return


func create_timer(duration: float, effect_data: StatusEffectDataResource) -> Timer:
	var timer = Timer.new()
	timer.wait_time = duration
	timer.one_shot = true
	timer.autostart = true
	timer.timeout.connect(func(): remove_effect(effect_data))
	add_child(timer)
	return timer