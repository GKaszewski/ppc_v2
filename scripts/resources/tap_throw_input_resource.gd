class_name TapThrowInputResource
extends ThrowInputResource

func process_input(event: InputEvent) -> void:
	if event.is_action_pressed("attack"):
		throw_requested.emit(1.0)