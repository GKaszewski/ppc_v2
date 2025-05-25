class_name EnemyWaveTrigger
extends Node

@export var area2d: Area2D
@export var path_follow_node: PathFollow2D
@export var speed: float = 100.0
@export var loop: bool = false
@export var activate_on_enter: bool = true

var active: bool = false


func _ready() -> void:
	area2d.body_entered.connect(_on_body_entered)
	if path_follow_node:
		path_follow_node.progress = 0
		path_follow_node.set_process(false)


func _process(delta: float) -> void:
	if not active or not path_follow_node:
		return
	path_follow_node.progress += speed * delta
	if path_follow_node.progress_ratio >= 1.0 and not loop:
		active = false
		path_follow_node.set_process(false)


func _on_body_entered(body: Node2D) -> void:
	if activate_on_enter:
		start_wave()


func start_wave() -> void:
	if path_follow_node:
		path_follow_node.set_process(true)
		active = true