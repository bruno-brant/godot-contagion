// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Cell.Scripts;

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
	private MeshInstance3D[] _pieces;

	// The current piece being displayed.
	private MeshInstance3D _current_piece;

	/// <summary>
	/// Gets or sets the power level of the cell.
	/// </summary>
	[Export]
	public int PowerLevel
	{
		get => _powerLevel;

		set
		{
			if (value is < 0 or > 4)
			{
				throw new Exception("power_level must be between 0 and 4");
			}

			SetVisibleMesh(value);
			_powerLevel = value;
		}
	}

	/// <summary>
	/// Gets or sets the color of the cell.
	/// </summary>
	[Export]
	public Color Color
	{
		get { return _piece_material.AlbedoColor; }
		set { _piece_material.AlbedoColor = value; }
	}

	/// <summary>
	/// Gets a value indicating whether the cell is at maximum power.
	/// </summary>
	public bool IsMaxPower => _powerLevel == _pieces.Length - 1;

	/// <summary>
	/// Gets a value indicating whether the cell is at minimum power.
	/// </summary>
	public bool IsMinPower => _powerLevel == 0;

	/// <summary>
	/// Increases the power of the cell by one.
	/// </summary>
	public void IncreasePower()
	{
		if (!IsMaxPower)
		{
			PowerLevel++;
		}
	}

	/// <summary>
	/// Descreases the power of the cell by one.
	/// </summary>
	public void DecreasePower()
	{
		if (!IsMinPower)
		{
			PowerLevel--;
		}
	}

	/// <inheritdoc/>
	public override void _Ready()
	{
		_pieces = new MeshInstance3D[]
		{
			null,
			(MeshInstance3D)GetNode("Piece/Level1"),
			(MeshInstance3D)GetNode("Piece/Level2"),
			(MeshInstance3D)GetNode("Piece/Level3"),
			(MeshInstance3D)GetNode("Piece/Level4"),
		};

		AssignMaterialToPieceMeshes();
	}

	private void AssignMaterialToPieceMeshes()
	{
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
		if (_current_piece != null)
		{
			_current_piece.Visible = false;
		}

		_current_piece = level < _pieces.Length ? _pieces[level] : null;

		if (_current_piece != null)
		{
			_current_piece.Visible = true;
		}
	}
}
