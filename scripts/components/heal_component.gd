class_name HealComponent
extends Node

@export var heal_fx: GPUParticles2D
@export var collectable: CollectableComponent


func _ready() -> void:
	if not collectable:
		printerr("HealComponent: No CollectableComponent assigned.")
		return

	collectable.collected.connect(on_collected)


func on_collected(amount: float, type: CollectableResource.CollectableType, _body: Node2D) -> void:
	if type != CollectableResource.CollectableType.HEALTH:
		return

	if not collectable:
		printerr("HealComponent: No CollectableComponent assigned.")
		return

	var health_component := _body.get_node_or_null("HealthComponent") as HealthComponent
	if not health_component or not health_component is HealthComponent:
		printerr("HealComponent: No HealthComponent found on collected body.")
		return
	health_component.increase_health(amount)
	if heal_fx:
		play_heal_fx()
	else:
		owner.queue_free()


func play_heal_fx() -> void:
	if not heal_fx:
		return

	heal_fx.restart()
	heal_fx.emitting = true