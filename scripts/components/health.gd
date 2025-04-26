class_name HealthComponent
extends Node

@export var health: float = 1.0
@export var max_health: float = 1.0
@export var hurt_fx: AudioStreamPlayer2D
@export var heal_fx: AudioStreamPlayer2D
signal on_health_change(delta: float, total_health: float)
signal on_death


func _get_delta(value: float) -> float:
	var old_value = health
	return value - old_value


func set_health(new_value: float):
	var delta = _get_delta(new_value)

	if sign(delta) > 0:
		if heal_fx:
			heal_fx.play()
	elif sign(delta) < 0:
		if hurt_fx:
			hurt_fx.play()

	await hurt_fx.finished

	if new_value >= max_health:
		health = max_health
		on_health_change.emit(delta, health)
		return

	health = new_value

	if health <= 0:
		on_death.emit()
		return

	on_health_change.emit(delta, health)


func decrease_health(value: float):
	var delta = _get_delta(value)

	health -= value
	if hurt_fx:
		hurt_fx.play()
		await hurt_fx.finished

	if health <= 0:
		on_death.emit()
		return

	on_health_change.emit(delta, health)


func increase_health(value: float):
	var delta = _get_delta(value)

	health += value

	if health >= max_health:
		health = max_health
		on_health_change.emit(delta, health)
		return

	health += value
	if heal_fx:
		heal_fx.play()
	on_health_change.emit(delta, health)
	
