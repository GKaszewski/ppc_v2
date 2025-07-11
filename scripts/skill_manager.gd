class_name SkillManager
extends Node

@export var available_skills: Array[SkillData] = []

@onready var gm: GM = $"/root/GameManager"

var active_components: Dictionary = {}


func _ready() -> void:
	apply_unlocked_skills()


func add_skill(skill_data: SkillData) -> void:
	if active_components.has(skill_data.name):
		return

	if skill_data.type == SkillData.SkillType.THROW:
		var unlocked_skills: Array = gm.get_unlocked_skills()
		for skill in unlocked_skills:
			var data = null
			for s in available_skills:
				if s == skill:
					data = s
					break
			if data and data.type == SkillData.SkillType.THROW:
				remove_skill(data.name)

	var skill_instance := skill_data.node.instantiate()
	for key in skill_data.config.keys():
		if key in skill_instance:
			var value  =  skill_data.config[key]
			var parent := get_parent()

			if value is NodePath:
				if parent.has_node(value):
					value = parent.get_node(value)
				elif skill_instance.has_node(value):
					value = skill_instance.get_node(value)
				else:
					continue

			skill_instance[key] = value

	owner.add_child(skill_instance)
	active_components[skill_data.name] = skill_instance
	

func remove_skill(skill_name: String) -> void:
	if not active_components.has(skill_name):
		return

	var skill_instance = active_components[skill_name]
	if is_instance_valid(skill_instance):
		skill_instance.queue_free()

	var skills: Array = gm.get_unlocked_skills()
	for s in skills:
		if s.name == skill_name:
			s.is_active = false
			break
	active_components.erase(skill_name)


func apply_unlocked_skills() -> void:
	for skill_data in available_skills:
		if gm.is_skill_unlocked(skill_data):
			print("Applying skill: ", skill_data.name)
			call_deferred("add_skill", skill_data)
		else:
			remove_skill(skill_data.name)


func get_skill_by_name(skill_name: String) -> SkillData:
	for skill_data in available_skills:
		if skill_data.name == skill_name:
			return skill_data
	return null


func activate_skill(skill: SkillData) -> void:
	if not active_components.has(skill.name):
		if skill:
			add_skill(skill)
			skill.is_active = true


func deactivate_skill(skill: SkillData) -> void:
	if active_components.has(skill.name):
		remove_skill(skill.name)
		if skill:
			skill.is_active = false


func toggle_skill_activation(skill: SkillData) -> void:
	if not skill:
		return

	if active_components.has(skill.name):
		deactivate_skill(skill)
	else:
		activate_skill(skill)
