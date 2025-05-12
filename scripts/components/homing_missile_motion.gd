class_name HomingMissileMotion
extends Node

@export var launch_component: LaunchComponent
@export var root: Node2D
@export var max_speed: float = 16.0
@export var acceleration: float = 8.0
@export var detection_area: Area2D
@export var max_turn_rate: float = 180.0

var target: Node2D    = null
var velocity: Vector2 = Vector2.ZERO


func _ready() -> void:
	if not detection_area:
		printerr("No detection area assigned!")
		return

	if not launch_component:
		printerr("No launch component assigned!")
		return

	detection_area.body_entered.connect(on_detection_area_body_entered)
	velocity = launch_component.get_initial_velocity()


func _physics_process(delta: float) -> void:
	if not launch_component or not root:
		return

	if not target:
		root.position += velocity * delta
		return

	var to_target       := (target.global_position - root.global_position).normalized()
	var angle_to_target := velocity.angle_to(to_target)
	var max_angle       := deg_to_rad(max_turn_rate) * delta
	var clamped_angle   =  clamp(angle_to_target, - max_angle, max_angle)
	velocity = velocity.rotated(clamped_angle).normalized() * velocity.length()

	var desired_speed = min(max_speed, velocity.length() + acceleration * delta)
	velocity = velocity.normalized() * desired_speed

	root.position += velocity * delta
	root.rotation = velocity.angle()


func on_detection_area_body_entered(body: Node) -> void:
	print("Body entered detection area: ", body.name)
	if target != null:
		return

	if body == null:
		return

	target = body
	 
	
	