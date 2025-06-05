class_name SkillData
extends Resource

enum SkillType {
	ATTACK,
	THROW,
	MISC,
}
@export var name: String = ""
@export var description: String = ""
@export var node: PackedScene
@export var config: Dictionary = {}
@export var cost: int = 0
@export var icon: Texture2D
@export var type: SkillType = SkillType.ATTACK
@export var is_active: bool = false
@export var level: int = 1
@export var max_level: int = 1