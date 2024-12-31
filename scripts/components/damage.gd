class_name DamageComponent
extends Node

@export var damage: float = 0.25
@export var area2d: Area2D

func _ready() -> void:
	if not area2d:
		printerr("No area2d assigned!")
		return
		
	area2d.body_entered.connect(on_area2d_body_entered)

func deal_damage(target: HealthComponent):
	target.decrease_health(damage)

func on_area2d_body_entered(body: Node2D):
	if body.has_node("HealthComponent"):
		var health_component: HealthComponent = body.get_node("HealthComponent")
		deal_damage(health_component)
