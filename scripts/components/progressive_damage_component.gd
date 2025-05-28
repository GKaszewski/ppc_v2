class_name ProgressiveDamageComponent
extends Node

@export var health_component: HealthComponent
@export var sprite: Sprite2D
@export var platform_movement: PlatformMovement
@export var min_jump_height: float = 60.0
@export var jump_reduction_percentage: float = 0.1 # this is a percentage of the jump height per hit

@onready var max_health: float = health_component.max_health

var og_jump_height: float = 0.0


func _ready() -> void:
	if not health_component:
		printerr("ProgressiveDamageComponent: health_component is not set.")
		return
	if not sprite:
		printerr("ProgressiveDamageComponent: sprite is not set.")
		return

	health_component.on_health_change.connect(on_health_change)

	if platform_movement:
		og_jump_height = platform_movement.jump_height


func get_damage_frame() -> int:
	if not sprite or not health_component:
		return 0

	var frames_count := sprite.get_hframes()
	if frames_count == 0:
		return 0

	var current_health := health_component.health
	var health_ratio   := current_health / max_health
	return int(frames_count * (1.0 - health_ratio))


func get_jump_height() -> float:
	if not platform_movement:
		return 0.0

	var jump_height := og_jump_height
	if jump_height <= 0:
		return 0.0

	var damage_frame := get_damage_frame()
	if damage_frame < 0 or damage_frame >= sprite.get_hframes():
		return jump_height

	var reduction              := jump_reduction_percentage * jump_height
	var calculated_jump_height := jump_height - (damage_frame * reduction)
	return max(calculated_jump_height, min_jump_height)


func on_health_change(_delta: float, _total_health: float) -> void:
	var frame := get_damage_frame()
	if frame < 0 or frame >= sprite.get_hframes():
		return

	sprite.frame = frame
	if platform_movement:
		platform_movement.jump_height = get_jump_height()
		