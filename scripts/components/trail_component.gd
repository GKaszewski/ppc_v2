class_name TrailComponent
extends Line2D

@export var max_points: int = 100

var queue: Array[Vector2] = []


func _process(_delta: float) -> void:
	queue.push_front(owner.global_position)

	if queue.size() > max_points:
		queue.pop_back()

	clear_points()

	for point in queue:
		add_point(point)