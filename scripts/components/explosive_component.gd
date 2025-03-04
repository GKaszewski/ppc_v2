class_name ExplosiveComponent
extends Node

@export var root: Node2D
@export var damage: DamageComponent
@export var area2d: Area2D
@export var explosion_area2d: Area2D
@export var explosion_effect: PackedScene
@export var time_to_explode: float = 9.0
signal on_explosion(body: Node2D)
var timer: Timer


func _ready() -> void:
	if not damage:
		printerr("No damage component assigned!")
		return

	if not explosion_area2d:
		printerr("No area2d assigned!")
		return

	area2d.body_entered.connect(on_area2d_body_entered)
	area2d.area_entered.connect(on_area2d_area_entered)

	prepare_timer()


func prepare_timer() -> void:
	timer = Timer.new()
	timer.set_wait_time(time_to_explode)
	timer.set_one_shot(true)
	timer.autostart = true
	timer.timeout.connect(explode)
	add_child(timer)


func explode() -> void:
	timer.stop()

	if explosion_effect:
		var effect: Node2D = explosion_effect.instantiate()
		root.get_parent().add_child(effect)
		effect.global_position = root.global_position

	var bodies: Array = explosion_area2d.get_overlapping_bodies()
	for body in bodies:
		var health_component: HealthComponent = body.get_node_or_null("HealthComponent")

		if damage and health_component:
			damage.deal_damage(health_component)

		on_explosion.emit(body)

	root.queue_free()


func on_area2d_body_entered(_body: Node2D) -> void:
	explode()


func on_area2d_area_entered(_area: Area2D) -> void:
	explode()
