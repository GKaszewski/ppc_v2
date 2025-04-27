class_name DamageComponent
extends Node

@export var damage: float = 0.25
@export var area2d: Area2D
@export var status_effect_data: StatusEffectDataResource
@export var damage_timer: Timer

var current_target: Node = null
signal effect_inflicted(target: Node2D, effect: StatusEffectDataResource)


func _ready() -> void:
	if not area2d:
		printerr("No area2d assigned!")
		return

	area2d.body_entered.connect(on_area2d_body_entered)
	area2d.body_exited.connect(on_area2d_body_exited)
	area2d.area_entered.connect(on_area2d_area_entered)

	if damage_timer:
		damage_timer.timeout.connect(on_damage_timer_timeout)


func _process(_delta: float) -> void:
	if not current_target:
		return
	if damage_timer:
		return
	process_entity_and_apply_damage(current_target)


func deal_damage(target: HealthComponent) -> void:
	target.decrease_health(damage)


func on_damage_timer_timeout() -> void:
	if not current_target:
		return

	process_entity_and_apply_damage(current_target)


func process_entity_and_apply_damage(body: Node2D) -> void:
	if body.has_node("HealthComponent"):
		var health_component: HealthComponent                   = body.get_node("HealthComponent")
		var invulnerability_component: InvulnerabilityComponent = body.get_node_or_null("InvulnerabilityComponent")

		if invulnerability_component and invulnerability_component.is_invulnerable():
			return

		if status_effect_data and status_effect_data.effect_type != StatusEffectComponent.EffectType.NONE:
			effect_inflicted.emit(body, status_effect_data)

		deal_damage(health_component)

		if invulnerability_component:
			invulnerability_component.activate()


func on_area2d_body_entered(body: Node2D) -> void:
	current_target = body

	if not check_if_processing_is_on():
		return

	if damage_timer:
		damage_timer.start()

	process_entity_and_apply_damage(body)


func on_area2d_body_exited(body: Node2D) -> void:
	if body == current_target:
		current_target = null
		if damage_timer:
			damage_timer.stop()


func on_area2d_area_entered(area: Area2D) -> void:
	if not check_if_processing_is_on():
		return

	if area == area2d:
		return

	var parent := area.get_parent()
	if parent.has_node("DamageComponent"):
		process_entity_and_apply_damage(parent)


func check_if_processing_is_on() -> bool:
	return self.process_mode == PROCESS_MODE_INHERIT or self.process_mode == PROCESS_MODE_ALWAYS