class_name CollectableResource
extends Resource

enum CollectableType {
	COIN,
	KID,
	HEALTH,
}
@export var amount: int = 0
@export var type: CollectableType = CollectableType.COIN