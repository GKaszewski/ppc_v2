class_name FlashingComponent
extends Node

@export var sprite: Node2D
@export var flash_duration: float = 0.5
@export var flash_time: float = 0.1
@export var use_modulate: bool = true
@export var health_component: HealthComponent

var tween: Tween


func _ready() -> void:
	if health_component:
		health_component.on_health_change.connect(on_health_change)
		health_component.on_death.connect(on_death)

	if not sprite:
		printerr("No sprite assigned!")
		return


func start_flashing() -> void:
	if not sprite:
		return

	if tween:
		tween.kill()

	tween = create_tween()
	tween.set_parallel(false)

	var flashes: int = int(flash_duration / flash_time)
	for i in range(flashes):
		var opacity: float = 0.3 if i % 2 == 0 else 1.0
		tween.tween_property(sprite, "modulate:a" if use_modulate else "visible", opacity if use_modulate else float(i % 2 == 0), flash_time)

	tween.tween_callback(stop_flashing)


func stop_flashing() -> void:
	if use_modulate:
		sprite.modulate.a = 1.0
	else:
		sprite.visible = true
		

func on_health_change(delta: float, _total_health: float) -> void:
	if delta < 0:
		start_flashing()


func on_death() -> void:
	stop_flashing()
					