class_name DamageComponent
extends Node

@export var damage: float = 0.25
@export var area2d: Area2D
@export var is_fire_brick: bool = false
@export var is_ice_brick: bool = false


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
		var fire_effect_component: FireEffectComponent          = body.get_node_or_null("FireEffectComponent")
		var ice_effect_component: IceEffectComponent            = body.get_node_or_null("IceEffectComponent")

		if invulnerability_component and invulnerability_component.is_invulnerable():
			return

		if fire_effect_component and is_fire_brick:
			fire_effect_component.activate()
			return

		if ice_effect_component and is_ice_brick:
			ice_effect_component.activate()
			return

		deal_damage(health_component)

		if invulnerability_component:
			invulnerability_component.activate()
