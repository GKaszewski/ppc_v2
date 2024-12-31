class_name KnockbackComponent
extends Node

@export var character_body: CharacterBody2D
@export var knockback_force: float = 25.0

var knockback_mode: bool = false
var knockback_frames: int = 0

func apply_knockback(force: float, delta: float) -> void:
	var velocity = character_body.velocity.normalized()
	var knockback_dir = Vector2(sign(velocity.x) * 1.0, 0.4)
	var knockback_vector = -knockback_dir * force * delta
	character_body.velocity += knockback_vector

func _on_health_component_on_health_change(delta: float, total_health: float) -> void:
	if total_health <= 0.0 and delta < 0.0:
		return
	knockback_mode = true
	
func _process(_delta: float) -> void:
	if knockback_mode:
		knockback_frames += 1
	if knockback_frames > 1:
		knockback_mode = false
		knockback_frames = 0
		
func _physics_process(delta: float) -> void:
	if knockback_mode:
		apply_knockback(knockback_force, delta)
		
	
