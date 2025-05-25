extends Node

@export var master_volume_slider: Slider
@export var music_volume_slider: Slider
@export var sfx_volume_slider: Slider
@export var audio_settings_control: Control
@export var mute_treshold: float = -20.0


func _ready() -> void:
	initialize()
	if master_volume_slider:
		master_volume_slider.value_changed.connect(_on_master_volume_slider_value_changed)
	if music_volume_slider:
		music_volume_slider.value_changed.connect(_on_music_volume_slider_value_changed)
	if sfx_volume_slider:
		sfx_volume_slider.value_changed.connect(_on_sfx_volume_slider_value_changed)

	load_settings()


func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if UiManager.is_screen_on_top(audio_settings_control):
			save_settings()
			UiManager.pop_screen()


func initialize() -> void:
	if master_volume_slider:
		var volume_db: float = AudioServer.get_bus_volume_db(AudioServer.get_bus_index("Master"))
		master_volume_slider.value = volume_db
		master_volume_slider.min_value = mute_treshold
		master_volume_slider.max_value = 0.0
	if music_volume_slider:
		var music_volume_db: float = AudioServer.get_bus_volume_db(AudioServer.get_bus_index("music"))
		music_volume_slider.value = music_volume_db
		music_volume_slider.min_value = mute_treshold
		music_volume_slider.max_value = 0.0
	if sfx_volume_slider:
		var sfx_volume_db: float = AudioServer.get_bus_volume_db(AudioServer.get_bus_index("sfx"))
		sfx_volume_slider.value = sfx_volume_db
		sfx_volume_slider.min_value = mute_treshold
		sfx_volume_slider.max_value = 0.0


func _handle_mute(bus_index: int, value: float) -> void:
	if AudioServer:
		if value == mute_treshold:
			AudioServer.set_bus_mute(bus_index, true)
		else:
			AudioServer.set_bus_mute(bus_index, false)


func _on_master_volume_slider_value_changed(value: float) -> void:
	if AudioServer:
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), value)
		_handle_mute(AudioServer.get_bus_index("Master"), value)


func _on_music_volume_slider_value_changed(value: float) -> void:
	if AudioServer:
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("music"), value)
		_handle_mute(AudioServer.get_bus_index("music"), value)


func _on_sfx_volume_slider_value_changed(value: float) -> void:
	if AudioServer:
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("sfx"), value)
		_handle_mute(AudioServer.get_bus_index("sfx"), value)


func save_settings() -> void:
	if not AudioServer:
		return

	var settings_config := ConfigFileHandler.settings_config
	settings_config.set_value("audio_settings", "master_volume", master_volume_slider.value)
	settings_config.set_value("audio_settings", "music_volume", music_volume_slider.value)
	settings_config.set_value("audio_settings", "sfx_volume", sfx_volume_slider.value)
	settings_config.set_value("audio_settings", "mute_treshold", mute_treshold)
	settings_config.save(ConfigFileHandler.SETTINGS_PATH)


func load_settings() -> void:
	if not AudioServer:
		return

	var settings_config := ConfigFileHandler.settings_config
	if not settings_config.has_section("audio_settings"):
		print("Audio settings section not found in config file.")
		return

	var master_volume: float = settings_config.get_value("audio_settings", "master_volume", 0.0)
	var music_volume: float  = settings_config.get_value("audio_settings", "music_volume", 0.0)
	var sfx_volume: float    = settings_config.get_value("audio_settings", "sfx_volume", 0.0)

	if master_volume_slider:
		master_volume_slider.value = master_volume
	if music_volume_slider:
		music_volume_slider.value = music_volume
	if sfx_volume_slider:
		sfx_volume_slider.value = sfx_volume
	mute_treshold = settings_config.get_value("audio_settings", "mute_treshold", -20.0)
