class_name Hud
extends Node

@export var player_health: HealthComponent
@export var coins_label: Label
@export var health_progressbar: ProgressBar
@export var lives_label: Label

@onready var game_manager: GM = $"/root/GameManager"


func _ready() -> void:
	if not player_health:
		var nodes := get_tree().get_nodes_in_group("player")
		for node in nodes:
			player_health = node.get_node_or_null("HealthComponent")
			if player_health:
				break
		return


func _process(_delta: float) -> void:
	if not game_manager:
		return

	set_health_progressbar()
	set_lives_label()
	set_coins_label()


func set_coins_label() -> void:
	if not game_manager:
		return

	#todo: set internationalized text
	coins_label.text = "Coins:" + str(game_manager.get_coins())


func set_lives_label() -> void:
	if not game_manager:
		return

	lives_label.text = "Lives:" + str(game_manager.get_lives())


func set_health_progressbar() -> void:
	if not player_health:
		return

	health_progressbar.value = player_health.health
	health_progressbar.max_value = player_health.max_health