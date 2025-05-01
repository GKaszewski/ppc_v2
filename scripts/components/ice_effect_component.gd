class_name IceEffectComponent
extends Node

@export var components_to_disable: Array = []
@export var status_effect_component: StatusEffectComponent
@export var ice_fx: Node2D

var data: StatusEffectDataResource = null
var ice_effects_applied: int       = 0


func _ready() -> void:
	if not status_effect_component:
		status_effect_component = get_node("StatusEffectComponent")

	if not status_effect_component:
		printerr("No StatusEffectComponent assigned!")
		return

	status_effect_component.effect_applied.connect(on_effect_applied)
	status_effect_component.effect_removed.connect(on_effect_removed)


func on_effect_applied(effect_data: StatusEffectDataResource) -> void:
	if effect_data.effect_type == StatusEffectComponent.EffectType.ICE:
		data = effect_data
		ice_effects_applied += 1
		apply_freeze()


func on_effect_removed(effect_type: StatusEffectComponent.EffectType) -> void:
	if effect_type == StatusEffectComponent.EffectType.ICE:
		data = null
		ice_effects_applied -= 1
		remove_freeze()


func apply_freeze() -> void:
	if ice_fx:
		ice_fx.visible = true

	for component_path in components_to_disable:
		var component: Node = get_node_or_null(component_path)
		if not component or ice_effects_applied == 0:
			continue

		component.process_mode = PROCESS_MODE_DISABLED


func remove_freeze() -> void:
	if ice_effects_applied > 0:
		return

	if ice_fx:
		ice_fx.visible = false

	for component_path in components_to_disable:
		var component: Node = get_node_or_null(component_path)
		if not component:
			continue

		component.process_mode = PROCESS_MODE_ALWAYS