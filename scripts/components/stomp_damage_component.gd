class_name StompDamageComponent
extends Node

@export var damage: float = 0.25
@export var area2d: Area2D
@export var root: Node2D

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if not area2d:
		printerr("No area2d assigned!")
		return
	if not root:
		printerr("No root assigned!")
		return
		
	area2d.body_entered.connect(on_area2d_body_entered)


func deal_damage(target: HealthComponent) -> void:
	target.decrease_health(damage)
	
	
func on_area2d_body_entered(body: Node2D) -> void:
	if body.has_node("HealthComponent"):
		var health_component: HealthComponent = body.get_node("HealthComponent")
		if not health_component:
			printerr("No HealthComponent assigned!")
			return
		
		if root.global_position.y < body.global_position.y:
			if root is PlayerController:
				var velocity: Vector2 = root.previous_velocity
				print("Velocity: ", velocity)
				if velocity.y > 0.0:
					deal_damage(health_component)
					print("Dealt damage to ", body)
		