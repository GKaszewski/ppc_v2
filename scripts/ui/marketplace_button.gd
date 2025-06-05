class_name MarketplaceButton
extends Button

@export var skill_data: SkillData
@export var unlocked_skill_icon: Texture2D
@export var locked_skill_icon: Texture2D
@export var skill_level_container: Container

@onready var gm: GM = $"/root/GameManager"


func _ready() -> void:
	if not skill_data:
		printerr("MarketplaceButton: skill_data is not set.")
	if not unlocked_skill_icon or not locked_skill_icon:
		printerr("MarketplaceButton: unlocked_skill_icon or locked_skill_icon is not set.")
		return
	if not skill_level_container:
		printerr("MarketplaceButton: skill_level_container is not set.")
		return

	setup()

	var player                   := gm.get_player_node()
	var skill_unlocker_component := player.get_node_or_null("SkillUnlockerComponent") as SkillUnlockerComponent
	if skill_unlocker_component:
		skill_unlocker_component.skill_unlocked.connect(_on_skill_unlock)


func setup() -> void:
	if not skill_data:
		return

	for i in range(skill_data.max_level):
		var _icon := TextureRect.new()
		_icon.texture = unlocked_skill_icon if i < skill_data.level else locked_skill_icon
		skill_level_container.add_child(_icon)


func _on_skill_unlock(skill: SkillData) -> void:
	if skill.name == skill_data.name:
		for i in range(skill_data.max_level):
			var icon := skill_level_container.get_child(i) as TextureRect
			if i < skill.level:
				icon.texture = unlocked_skill_icon
			else:
				icon.texture = locked_skill_icon
		disabled = skill.level >= skill_data.max_level