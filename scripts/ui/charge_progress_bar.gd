extends Node

@export var progress_bar: ProgressBar
@export var charge_throw_component: ChargeThrowComponent


func _ready() -> void:
	if not charge_throw_component:
		return

	if not progress_bar:
		return

	setup_progress_bar()
	progress_bar.hide()

	charge_throw_component.charge_started.connect(on_charge_started)
	charge_throw_component.charge_updated.connect(on_charge_updated)
	charge_throw_component.charge_stopped.connect(on_charge_stopped)


func on_charge_updated(charge_ratio: float) -> void:
	if not progress_bar:
		return

	progress_bar.value = charge_ratio
	progress_bar.show()


func on_charge_stopped() -> void:
	if not progress_bar:
		return

	progress_bar.hide()


func on_charge_started() -> void:
	if not progress_bar:
		return

	progress_bar.show()


func setup_progress_bar() -> void:
	if not progress_bar:
		return

	progress_bar.min_value = charge_throw_component.min_power
	progress_bar.max_value = charge_throw_component.max_power
	progress_bar.value = charge_throw_component.min_power
	progress_bar.step = 0.01
	progress_bar.show()