class_name RequirementComponent
extends Node

@export var requirement_type: CollectableResource.CollectableType
@export var required_amount: int = 1

var current_amount: int = 0
signal requirement_met(requirement_type: CollectableResource.CollectableType)
@onready var gm: GM = $"/root/GameManager"


func _ready() -> void:
	if not gm:
		printerr("RequirementComponent: GameManager not found.")
		return

	var collectables: Array[CollectableComponent] = gm.get_colllectable_nodes()
	for collectable in collectables:
		collectable.collected.connect(on_collected)


func on_collected(amount: int, type: CollectableResource.CollectableType) -> void:
	if type != requirement_type:
		return
	add_progress(amount)


func add_progress(amount: int = 1) -> void:
	current_amount += amount
	if current_amount >= required_amount:
		requirement_met.emit(requirement_type)
