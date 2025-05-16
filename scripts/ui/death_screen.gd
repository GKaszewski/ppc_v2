class_name DeathScreen
extends Node

@export var death_screen_root: Control
@export var current_level: LevelResource
@export var current_level_label: Label
@export var lives_left_label: Label
@export var timeout_time: float = 2.0
@export var nodes_to_disable: Array[Node] = []

@onready var gm: GM = $"/root/GameManager"

var timer: Timer


func _ready() -> void:
	set_lables()


func set_lables() -> void:
	if not gm:
		return

	current_level_label.text = current_level.level_name
	lives_left_label.text = " x " + str(gm.get_lives())


func setup_timer() -> void:
	timer = Timer.new()
	timer.wait_time = timeout_time
	timer.one_shot = true
	timer.timeout.connect(on_timeout)
	add_child(timer)
	timer.start()


func toggle_nodes() -> void:
	for node in nodes_to_disable:
		if node.process_mode == PROCESS_MODE_DISABLED:
			node.process_mode = PROCESS_MODE_INHERIT
		else:
			node.process_mode = PROCESS_MODE_DISABLED


func on_player_death() -> void:
	if not gm:
		return

	toggle_nodes()
	set_lables()
	death_screen_root.show()
	setup_timer()


func on_timeout() -> void:
	if not gm:
		return

	if gm.get_lives() == 0:
		return

	get_tree().reload_current_scene()
