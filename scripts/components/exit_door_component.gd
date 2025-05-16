class_name ExitDoorComponent
extends Node

@export var locked: bool = true
@export var exit_area: Area2D
@export var door_sprite: Sprite2D
@export var opened_door_sfx: AudioStreamPlayer2D
@export var opened_door_frame: int = 0
signal exit_triggered
@onready var gm: GM = $"/root/GameManager"


func _ready() -> void:
	if not exit_area:
		printerr("ExitDoorComponent: exit_area is not set.")
		return

	exit_area.body_entered.connect(on_exit_area_body_entered)


func unlock() -> void:
	locked = false
	if door_sprite:
		door_sprite.frame = opened_door_frame
	if opened_door_sfx:
		opened_door_sfx.play()


func on_exit_area_body_entered(_body: Node2D) -> void:
	if locked:
		return

	exit_triggered.emit()
	gm.unlock_level(gm.player_state["current_level"] + 1)
	call_deferred("go_to_next_level")


func go_to_next_level() -> void:
	gm.try_to_go_to_next_level()