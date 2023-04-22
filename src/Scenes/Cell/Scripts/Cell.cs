// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Cell.Scripts;

using Contagion.Scenes.Board.Scripts;

/// <summary>
/// A cell in the game board.
/// </summary>
[Tool]
public partial class Cell : Node3D
{
	// The material used for the cell pieces.
	private readonly BaseMaterial3D _piece_material = (BaseMaterial3D)GD.Load("res://Scenes/Cell/Materials/SolidColorStandardMaterial.tres").Duplicate();

	// Current power level of the cell.
	private int _powerLevel = 0;

	// The pieces that make up the cell.
	private MeshInstance3D?[]? _pieces;

	// The current piece being displayed.
	private MeshInstance3D? _currentPiece;

	/// <summary>
	/// Initializes a new instance of the <see cref="Cell"/> class.
	/// </summary>
	public Cell()
	{
		PowerLevel.LevelChanged += newLevel => SetVisibleMesh(newLevel);
	}

	/// <summary>
	/// Gets the power level of the cell.
	/// </summary>
	public PowerLevel PowerLevel { get; } = new PowerLevel(PowerLevel.MinPowerLevel);

	/// <summary>
	/// Gets or sets the color of the cell.
	/// </summary>
	[Export]
	public Color Color
	{
		get { return _piece_material.AlbedoColor; }
		set { _piece_material.AlbedoColor = value; }
	}

	/// <inheritdoc/>
	public override void _Ready()
	{
		_pieces = new MeshInstance3D?[]
		{
			null,
			GetNode<MeshInstance3D>("Piece/Level1"),
			(MeshInstance3D)GetNode("Piece/Level2"),
			(MeshInstance3D)GetNode("Piece/Level3"),
			(MeshInstance3D)GetNode("Piece/Level4"),
		};

		AssignMaterialToPieceMeshes();
	}

	private void AssignMaterialToPieceMeshes()
	{
		if (_pieces == null)
		{
			return;
		}

		foreach (var piece in _pieces)
		{
			if (piece != null)
			{
				piece.MaterialOverride = _piece_material;
			}
		}
	}

	private void SetVisibleMesh(int level)
	{
		if (_pieces == null)
		{
			return;
		}

		if (_currentPiece != null)
		{
			_currentPiece.Visible = false;
		}

		_currentPiece = level < _pieces.Length ? _pieces[level] : null;

		if (_currentPiece != null)
		{
			_currentPiece.Visible = true;
		}
	}
}
