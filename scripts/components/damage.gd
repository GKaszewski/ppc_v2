class_name DamageComponent
extends Node2D

@export var damage: float = 0.25

func deal_damage(target: HealthComponent):
	target.decrease_health(damage)
