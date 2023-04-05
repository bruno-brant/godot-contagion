@tool 
extends Node3D

## The current level of this piece.
@export_range(0, 4, 1) var powerLevel: int = 0:
	set(level):
		_set_visible_mesh(level)
		powerLevel = level

## The color of this player (piece)
@export var color: Color:
	set(value): 
		base_material.albedo_color = value
	get:
		return base_material.albedo_color

var _powerLevel: int = 0

var _currentPiece: MeshInstance3D = null

var base_material: BaseMaterial3D  = preload("res://Materials/SolicColorStandardMaterial.tres").duplicate()

## Maps each power level to a piece
@onready var _pieces: Array[MeshInstance3D] = [
	null, # level 0 has no piece
	$Piece/Level1 as MeshInstance3D,
	$Piece/Level2 as MeshInstance3D,
	$Piece/Level3 as MeshInstance3D,
	$Piece/Level4 as MeshInstance3D
]

func _ready():
	_assign_material_to_piece_meshes()
		
# assign the base material to all pieces
func _assign_material_to_piece_meshes():
	for piece in _pieces:
		if piece:
			piece.material_override = base_material

# set the visible mesh accordingly to the powerlevel
func _set_visible_mesh(level: int):
	if _currentPiece: 
		_currentPiece.visible = false
	
	_currentPiece = _pieces[level]
	
	if _currentPiece:
		_currentPiece.visible = true
