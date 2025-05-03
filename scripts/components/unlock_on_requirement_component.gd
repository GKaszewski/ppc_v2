class_name UnlockOnRequirementComponent
extends Node

@export var requirement_component: RequirementComponent
@export var unlock_target: Node


func _ready() -> void:
	if not requirement_component:
		printerr("UnlockOnRequirementComponent: requirement_component is not set.")
		return

	if not unlock_target:
		printerr("UnlockOnRequirementComponent: unlock_target is not set.")
		return

	requirement_component.requirement_met.connect(on_requirement_met)


func on_requirement_met(requirement_type: CollectableResource.CollectableType) -> void:
	if requirement_type == requirement_component.requirement_type:
		if unlock_target.has_method("unlock"):
			unlock_target.unlock()
		else:
			printerr("UnlockOnRequirementComponent: unlock_target does not have an unlock method.")

