extends Control

func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if UiManager.is_screen_on_top(self):
			UiManager.pop_screen()