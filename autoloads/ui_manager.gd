extends Node

@export var ui_stack: Array[Control] = []
signal screen_pushed(screen: Control)
signal screen_popped(screen: Control)


func push_screen(screen: Control) -> void:
	if not screen:
		push_error("Cannot push a null screen.")
		return

	ui_stack.append(screen)
	screen.show()
	screen.set_process_input(true)
	screen.set_focus_mode(Control.FOCUS_ALL)
	screen.grab_focus()
	screen_pushed.emit(screen)


func pop_screen() -> void:
	if ui_stack.is_empty():
		return

	var top: Control = ui_stack.pop_back()
	top.hide()
	top.set_process_input(false)
	screen_popped.emit(top)
	top.accept_event()

	if not ui_stack.is_empty():
		ui_stack.back().grab_focus()


func top_screen() -> Control:
	return ui_stack.back() if not ui_stack.is_empty() else null


func is_screen_on_top(screen: Control) -> bool:
	return not ui_stack.is_empty() and ui_stack.back() == screen


func is_visible_on_stack(screen: Control) -> bool:
	return ui_stack.has(screen) and screen.visible


func close_all() -> void:
	while not ui_stack.is_empty():
		pop_screen()


func hide_and_disable(screen: Control) -> void:
	screen.hide()
	screen.set_process_input(false)