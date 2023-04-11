@tool
extends Node3D
class_name Cell

## The current level of this piece.
@export_range(0, 4, 1) var power_level: int = 0:
	set(level):
		assert (0 <= power_level and power_level <= 4, "power_level must be between 0 and 4")

		_set_visible_mesh(level)

		power_level = level

## The color of this player (piece)
@export var color: Color:
	set(value):
		_piece_material.albedo_color = value
	get:
		return _piece_material.albedo_color

## Return true if the current cell is at its max power
var is_max_power: bool:
	get: return power_level == len(_pieces) - 1

## Returns true if the current cell is at its min power
var is_min_power: bool:
	get: return power_level == 0

## The current displayed piece
var _current_piece: MeshInstance3D = null

## The material applied to the pieces in this cell
var _piece_material: BaseMaterial3D  = preload("res://Scenes/Cell/Materials/SolidColorStandardMaterial.tres").duplicate()

## Maps each power level to a piece
@onready var _pieces: Array[MeshInstance3D] = [
	null, # level 0 has no piece
	$Piece/Level1 as MeshInstance3D,
	$Piece/Level2 as MeshInstance3D,
	$Piece/Level3 as MeshInstance3D,
	$Piece/Level4 as MeshInstance3D
]

## Increase the cell power
func increase_power():
	if not is_max_power:
		power_level += 1

## Decrease the cell power
func decrease_power():
	if not is_min_power:
		power_level -= 1

# run when the node is ready
func _ready():
	_assign_material_to_piece_meshes()

# assign the base material to all pieces
func _assign_material_to_piece_meshes():
	for piece in _pieces:
		if piece:
			piece.material_override = _piece_material

# set the visible mesh accordingly to the power_level
func _set_visible_mesh(level: int):
	if _current_piece:
		_current_piece.visible = false

	_current_piece = _pieces[level] if len(_pieces) > level else null

	if _current_piece:
		_current_piece.visible = true
