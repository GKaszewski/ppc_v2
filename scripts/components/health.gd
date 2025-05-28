class_name HealthComponent
extends Node

@export var health: float = 1.0
@export var max_health: float = 1.0
@export var hurt_fx: AudioStreamPlayer2D
@export var heal_fx: AudioStreamPlayer2D
signal on_health_change(delta: float, total_health: float)
signal on_death



func _get_delta(new_value: float) -> float:
	return new_value - health


func set_health(new_value: float):
	_apply_health_change(new_value)


func decrease_health(value: float):
	_apply_health_change(health - value)


func increase_health(value: float):
	_apply_health_change(health + value)


func _apply_health_change(new_health: float, play_fx: bool = true) -> void:
	new_health = clamp(new_health, 0.0, max_health)
	var delta := new_health - health

	if delta == 0.0:
		return # No change

	if play_fx:
		if delta > 0 and heal_fx:
			heal_fx.play()
		elif delta < 0 and hurt_fx:
			hurt_fx.play()
			await hurt_fx.finished

	health = new_health

	if health <= 0:
		on_death.emit()
	else:
		on_health_change.emit(delta, health)
