extends Node

@export var progress_bar: ProgressBar
@export var brick_throw_component: BrickThrowComponent

var throw_input: ChargeThrowInputResource


func _ready() -> void:
	owner.child_entered_tree.connect(on_nodes_changed)

	if progress_bar:
		progress_bar.hide()

	setup_dependencies()


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

	progress_bar.min_value = throw_input.min_power
	progress_bar.max_value = throw_input.max_power
	progress_bar.value = throw_input.min_power
	progress_bar.step = 0.01
	progress_bar.hide()


func setup_dependencies() -> void:
	if not brick_throw_component:
		return

	if brick_throw_component.throw_input_behavior is ChargeThrowInputResource:
		throw_input = brick_throw_component.throw_input_behavior as ChargeThrowInputResource
	else:
		throw_input = null

	if not throw_input:
		return

	if not progress_bar:
		return

	if not throw_input.supports_charging():
		progress_bar.hide()
		return

	setup_progress_bar()

	throw_input.charge_started.connect(on_charge_started)
	throw_input.charge_updated.connect(on_charge_updated)
	throw_input.charge_stopped.connect(on_charge_stopped)


func on_nodes_changed(node: Node) -> void:
	if node is BrickThrowComponent and brick_throw_component == null:
		brick_throw_component = node as BrickThrowComponent
		setup_dependencies()
		return