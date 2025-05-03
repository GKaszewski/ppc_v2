class_name TooltipComponent
extends Node

@export var area2d: Area2D
@export var ui_root: Control
@export var text: String = ""
@export var tooltip_label: Label


func _ready() -> void:
	if not area2d:
		printerr("Tooltip node missing Area2D child.")
		return

	if not ui_root:
		printerr("Tooltip node missing UI root child.")
		return

	if not tooltip_label:
		printerr("Tooltip node missing tooltip label child.")
		return

	tooltip_label.text = text
	ui_root.visible = false
	area2d.body_entered.connect(_on_area2d_body_entered)
	area2d.body_exited.connect(_on_area2d_body_exited)


func show_tooltip() -> void:
	ui_root.visible = true


func hide_tooltip() -> void:
	ui_root.visible = false


func _on_area2d_body_entered(_body: Node) -> void:
	show_tooltip()


func _on_area2d_body_exited(_body: Node) -> void:
	hide_tooltip()