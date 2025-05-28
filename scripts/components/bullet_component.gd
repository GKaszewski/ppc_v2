class_name BulletComponent
extends Node

@export var root: Node2D
@export var area2d: Area2D
@export var hit_terrain_fx: TerrainHitFx
@export var bullet_sprite: Sprite2D


func _ready() -> void:
	area2d.body_entered.connect(on_area2d_body_entered)
	area2d.area_entered.connect(on_area2d_area_entered)



func on_area2d_body_entered(body: Node2D) -> void:
	if body is TileMapLayer:
		if bullet_sprite:
			bullet_sprite.visible = false
		play_terrain_hit_fx()
		return

	root.queue_free()


func on_area2d_area_entered(_area: Area2D) -> void:
	root.queue_free()



func play_terrain_hit_fx() -> void:
	if not hit_terrain_fx:
		return

	await hit_terrain_fx.trigger_fx()

	root.queue_free()
