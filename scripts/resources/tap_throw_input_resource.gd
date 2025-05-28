class_name TapThrowInputResource
extends ThrowInputResource

func update(_delta: float) -> void:
	if Input.is_action_pressed("attack"):
		throw_requested.emit(1.0)