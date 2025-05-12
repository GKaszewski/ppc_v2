class_name ThrowInputResource
extends Resource

signal throw_requested(power_multiplier: float)


func process_input(event: InputEvent) -> void:
	pass


func update(delta: float) -> void:
	pass


func supports_charging() -> bool:
	return false