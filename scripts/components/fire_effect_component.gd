class_name FireEffectComponent
extends Node

@export var health_component: HealthComponent
@export var status_effect_component: StatusEffectComponent
@export var root: Node2D

var data: StatusEffectDataResource = null
var should_deal_damage: bool       = false
var time_elapsed: float            = 0.0


func _ready() -> void:
	if not health_component:
		health_component = root.get_node("HealthComponent")
	if not status_effect_component:
		status_effect_component = root.get_node("StatusEffectComponent")

	if not health_component:
		printerr("No HealthComponent assigned!")
		return
	if not status_effect_component:
		printerr("No StatusEffectComponent assigned!")
		return

	status_effect_component.effect_applied.connect(on_effect_applied)
	status_effect_component.effect_removed.connect(on_effect_removed)


func _process(delta: float) -> void:
	if not should_deal_damage or not health_component or not data:
		return

	time_elapsed += delta
	if time_elapsed >= 1.0:
		health_component.decrease_health(data.damage_per_second)
		time_elapsed = 0.0


func on_effect_applied(effect_data: StatusEffectDataResource) -> void:
	if effect_data.effect_type == StatusEffectComponent.EffectType.FIRE:
		data = effect_data
		should_deal_damage = true


func on_effect_removed(effect_type: StatusEffectComponent.EffectType) -> void:
	if effect_type == StatusEffectComponent.EffectType.FIRE:
		data = null
		should_deal_damage = false
