class_name EffectInflictorComponent
extends Node

@export var damage: DamageComponent


func _ready() -> void:
	if not damage:
		printerr("No damage component assigned!")
		return

	damage.effect_inflicted.connect(on_effect_inflicted)


func on_effect_inflicted(target: Node2D, effect: StatusEffectDataResource) -> void:
	var status_effect_component: StatusEffectComponent = target.get_node_or_null("StatusEffectComponent")
	if not status_effect_component:
		return

	status_effect_component.apply_effect(effect)
