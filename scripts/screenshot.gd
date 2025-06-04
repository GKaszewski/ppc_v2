extends Node

func _process(_delta: float) -> void:
	if OS.is_debug_build() and Input.is_action_just_pressed("screenshot"):
		var img  := get_viewport().get_texture().get_image()
		var id   := OS.get_unique_id() + "_" + Time.get_datetime_string_from_system()
		var path := "user://screenshot_" + str(id) + ".png"
		img.save_png(path)
