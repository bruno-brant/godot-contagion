// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using System.Diagnostics;
using Contagion.Assets.Scripts;
using Contagion.Scenes.Cell.Scripts;

/// <summary>
/// The board is a hexagonal grid of cells.
/// </summary>
[Tool]
public partial class Board : Node3D
{
	// Maps between cell coordinates and cell instances.
	private readonly BidirectionalMap<HexCoord, Cell> _coordToCellMap = new();

	// The radius of the board.
	private int _currentRadius = 1;

	// Distance between cells
	private float _currentDistance = 1;

	/// <summary>
	/// Gets or sets the distance between cells.
	/// </summary>
	// TODO: Get cell radius from mesh geometry!
	[Export(PropertyHint.Range, "0.001, 10")]
	public float Distance { get; set; } = 1f;

	/// <summary>
	/// Gets or sets cell resource to use for the board.
	/// </summary>
	[Export]
	public PackedScene? CellScene { get; set; }

	/// <summary>
	/// Gets or sets the radius of the board.
	/// </summary>
	[Export(PropertyHint.Range, "1,1000,")]
	public int Radius { get; set; } = 8;

	/// <summary>
	/// Returns a list of adjacent cells to the informed cell.
	/// </summary>
	/// <param name="cell">
	/// The cell to get the adjacent cells from.
	/// </param>
	/// <returns>
	/// A list of adjacent cells.
	/// </returns>
	public IEnumerable<Cell> GetAdjacentCells(Cell cell)
	{
		if (!_coordToCellMap.TryGetValue(cell, out var coord))
		{
			throw new Exception("The cell wasn't part of the board!");
		}

		foreach (var neighborCoord in coord.GetNeighbors())
		{
			if (_coordToCellMap.TryGetValue(neighborCoord, out var neighborCell))
			{
				yield return neighborCell;
			}
		}
	}

	/// <inheritdoc/>
	public override void _Process(double delta)
	{
		// Just recreate the board if the radius or distance changed.
		if (Radius != _currentRadius || _currentDistance != Distance)
		{
			RecreateBoard();

			_currentRadius = Radius;
			_currentDistance = Distance;
		}
	}

	/// <summary>
	/// Recreates the game board with the current radius and distance.
	/// </summary>
	/// <remarks>
	/// TODO: can be improved to minimize changes, however, this is not
	/// a performance bottleneck as it is only called when the board is changed.
	/// </remarks>
	private void RecreateBoard()
	{
		ClearBoard();

		var toVisit = new Queue<HexCoord> { HexCoord.Zero };
		var visited = new HashSet<HexCoord> { };

		GD.Print("Recreating board");

		while (toVisit.Any())
		{
			var coord = toVisit.Dequeue();

			// Check if we still are within Radius
			if (coord.Length >= Radius)
			{
				continue;
			}

			// We are traversing the hexgrid depth-first, but we'll touch cells more than once
			// This check avoids looping eternally
			if (visited.Contains(coord))
			{
				continue;
			}

			_ = visited.Add(coord);

			AddCell(coord);

			GD.Print($"Grid has {GetChildCount()} elements.");

			foreach (var neighborCoord in coord.GetNeighbors())
			{
				GD.Print("enqueuing neighbor coord");
				toVisit.Enqueue(neighborCoord);
			}
		}
	}

	/// <summary>
	/// Clears the board of all cells.
	/// </summary>
	private void ClearBoard()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}
	}

	/// <summary>
	/// Adds a cell at the specified location.
	/// </summary>
	/// <param name="coord">
	/// The coordinate of this cell.
	/// </param>
	private void AddCell(HexCoord coord)
	{
		Debug.Assert(CellScene != null, $"{nameof(CellScene)} must be set in the designer!");

		var cell = CellScene.Instantiate<Cell>();

		cell.Name = $"Cell_{coord}";

		var rigidBody = new StaticBody3D();

		rigidBody.AddChild(cell);

		AddChild(rigidBody);

		rigidBody.Translate(coord.ToVector(Distance));

		rigidBody.InputEvent += (camera, @event, position, normal, shapeIdx) => CellClicked(cell, @event);
	}

	private void CellClicked(Cell cell, InputEvent @event)
	{
		GD.Print($"Clicked cell {cell.Name}");

		if (@event is not InputEventMouse eventMouse)
		{
			return;
		}

		// check if the left mouse button was pressed
		if (!eventMouse.ButtonMask.HasFlag(MouseButtonMask.Left))
		{
			return;
		}

		var effect = new SpreadPowerEffect(new PowerLevel(1), this);

		effect.Apply(cell);
	}
}
