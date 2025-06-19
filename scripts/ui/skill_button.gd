class_name SkillButton
extends Button

@export var skill_data: SkillData


func setup() -> void:
	if not skill_data:
		return

	icon = skill_data.icon
	text = tr(skill_data.name)


func activate() -> void:
	set("theme_override_colors/font_color", Color("#49aa10"))


func deactivate() -> void:
	set("theme_override_colors/font_color", Color("#ffffff"))
	
	
