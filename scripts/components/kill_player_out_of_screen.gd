class_name KillPlayerOutOfScreen
extends Node

@export var screen_notifier: VisibleOnScreenNotifier2D
@export var health_component: HealthComponent


func _ready() -> void:
	if not screen_notifier:
		printerr("KillPlayerOutOfScreen: screen_notifier is not set.")
		return

	if not health_component:
		printerr("KillPlayerOutOfScreen: health_component is not set.")
		return

	screen_notifier.screen_exited.connect(out_of_screen)


func out_of_screen() -> void:
	if not health_component:
		return

	health_component.decrease_health(6000)
