class_name DamageComponent
extends Node

@export var damage: float = 0.25
@export var area2d: Area2D
@export var status_effect_data: StatusEffectDataResource
signal effect_inflicted(target: Node2D, effect: StatusEffectDataResource)


func _ready() -> void:
	if not area2d:
		printerr("No area2d assigned!")
		return

	area2d.body_entered.connect(on_area2d_body_entered)


func deal_damage(target: HealthComponent) -> void:
	target.decrease_health(damage)


func on_area2d_body_entered(body: Node2D) -> void:
	if body.has_node("HealthComponent"):
		var health_component: HealthComponent                   = body.get_node("HealthComponent")
		var invulnerability_component: InvulnerabilityComponent = body.get_node_or_null("InvulnerabilityComponent")

		if invulnerability_component and invulnerability_component.is_invulnerable():
			return

		if status_effect_data.effect_type != StatusEffectComponent.EffectType.NONE:
			effect_inflicted.emit(body, status_effect_data)
			return

		deal_damage(health_component)

		if invulnerability_component:
			invulnerability_component.activate()
