class_name ShipShooter
extends Node

@export var bullet_scene: PackedScene
@export var fire_rate: float = 0.2
@export var bullet_spawn: Marker2D
@export var shoot_sfx: AudioStreamPlayer2D


var can_shoot: bool = false


func _ready() -> void:
	set_process(false)


func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("attack") and can_shoot:
		shoot()


func shoot() -> void:
	if not can_shoot:
		return

	var bullet: Node2D =  bullet_scene.instantiate()
	var init           := bullet.get_node_or_null("ProjectileInitComponent") as ProjectileInitComponent
	if init:
		init.initialize({
			"position": bullet_spawn.global_position,
		})
	get_tree().current_scene.add_child(bullet)
	if shoot_sfx:
		shoot_sfx.play()

	can_shoot = false
	await get_tree().create_timer(fire_rate).timeout
	can_shoot = true


func on_ship_entered():
	can_shoot = true
	set_process(true)


func on_ship_exited():
	can_shoot = false
	set_process(false)
	if shoot_sfx:
		shoot_sfx.stop()