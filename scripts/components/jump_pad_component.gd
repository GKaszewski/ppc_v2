class_name JumpPadComponent
extends Node

@export var jump_force: float = 10.0
@export var area2d: Area2D
@export var sprite2d: Sprite2D
@export var start_animation_index: int = 0
@export var animation_duration: float = 0.5


func _ready() -> void:
	if not area2d:
		printerr("JumpPadComponent: area2d is not set.")
		return

	if not sprite2d:
		printerr("JumpPadComponent: sprite2d is not set.")
		return

	area2d.body_entered.connect(_on_body_entered)


func _on_body_entered(body: Node2D) -> void:
	var can_be_launched: CanBeLaunchedComponent = body.get_node_or_null("CanBeLaunchedComponent")
	if not can_be_launched:
		return

	if body is PlayerController and body.current_movement is PlatformMovement:
		handle_launchpad_animation()
		body.velocity.y = -jump_force
		if body.current_movement.jump_sfx:
			body.current_movement.jump_sfx.play()


func handle_launchpad_animation() -> void:
	if not sprite2d:
		return

	var timer := get_tree().create_timer(animation_duration)
	sprite2d.frame = start_animation_index + 1
	await timer.timeout
	sprite2d.frame = start_animation_index
