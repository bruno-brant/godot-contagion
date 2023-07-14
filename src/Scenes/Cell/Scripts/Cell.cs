// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Cell.Scripts;

using Contagion.Scenes.Board.Scripts;

/// <summary>
/// A cell in the game board.
/// </summary>
[Tool]
public partial class Cell : StaticBody3D
{
	// The material used for the cell pieces.
	private readonly BaseMaterial3D _pieceMaterial = (BaseMaterial3D)GD.Load("res://Scenes/Cell/Materials/SolidColorStandardMaterial.tres").Duplicate();

	// The pieces that make up the cell.
	private readonly List<MeshInstance3D?> _pieces = new();

	// The mesh of the base of the cell.
	private readonly Lazy<MeshInstance3D> _baseMesh;

	// The current piece being displayed.
	private MeshInstance3D? _currentPiece;

	// The player that owns this cell.
	private Player? _player = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="Cell"/> class.
	/// </summary>
	public Cell()
	{
		PowerLevel.LevelChanged += (newLevel, player) =>
		{
			_player = player;
			SetVisibleMesh(newLevel);
			EmitSignal(nameof(PowerLevelChangedEventHandler), newLevel);
		};

		_baseMesh = new Lazy<MeshInstance3D>(() => GetNode<MeshInstance3D>("Base/BaseMesh"));
	}

	/// <summary>
	/// Event raised when the power level of the cell changes.
	/// </summary>
	/// <param name="newLevel">
	/// The new level of power of the cell.
	/// </param>
	[Signal]
	public delegate void PowerLevelChangedEventHandler(int newLevel);

	/// <summary>
	/// Gets or sets the material of the base of the cell.
	/// </summary>
	[Export]
	public Material? BaseMaterial { get; set; }

	/// <summary>
	/// Gets or sets the material of the base of the cell when it is highlighted.
	/// </summary>
	[Export]
	public Material? BaseHighlightedMaterial { get; set; }
	
	/// <summary>
	/// Gets the board this cell belongs to.
	/// </summary>
	public Board Board => GetParent<Board>();

	/// <summary>
	/// Gets the power level of the cell.
	/// </summary>
	public PowerLevel PowerLevel { get; } = new();

	/// <summary>
	/// Gets or sets the player that owns this cell.
	/// </summary>
	public Player? Player
	{
		get => _player;

		set
		{
			_player = value;
			_pieceMaterial.AlbedoColor = _player?.Color ?? Colors.Gray;
		}
	}

	/// <summary>
	/// Sets the cell to be highlighted.
	/// </summary>
	/// <seealso cref="Dim"/>
	public void Highlight()
	{
		_pieceMaterial.EmissionEnabled = true;
		_baseMesh.Value.MaterialOverride = BaseHighlightedMaterial;
	}

	/// <summary>
	/// Sets the cell to not be highlighted.
	/// </summary>
	/// <seealso cref="Highlight"/>"
	public void Dim()
	{
		_pieceMaterial.EmissionEnabled = false;
		_baseMesh.Value.MaterialOverride = BaseMaterial;
	}

	/// <inheritdoc/>
	public override void _Ready()
	{
		_pieces.AddRange(new MeshInstance3D?[]
		{
			null,
			GetNode<MeshInstance3D>("Piece/Level1"),
			GetNode<MeshInstance3D>("Piece/Level2"),
			GetNode<MeshInstance3D>("Piece/Level3"),
			GetNode<MeshInstance3D>("Piece/Level4"),
		});

		foreach (var piece in _pieces)
		{
			if (piece != null)
			{
				piece.Visible = false;
			}
		}

		AssignMaterialToPieceMeshes();
	}

	private void AssignMaterialToPieceMeshes()
	{
		foreach (var piece in _pieces)
		{
			if (piece != null)
			{
				piece.MaterialOverride = _pieceMaterial;
			}
		}
	}

	private void SetVisibleMesh(int level)
	{
		if (_currentPiece != null)
		{
			_currentPiece.Visible = false;
		}

		_currentPiece = level < _pieces.Count ? _pieces[level] : null;

		if (_currentPiece != null)
		{
			_currentPiece.Visible = true;
		}
	}
}
