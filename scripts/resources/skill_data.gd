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