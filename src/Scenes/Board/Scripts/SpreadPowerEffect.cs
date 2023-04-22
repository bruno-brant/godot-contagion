// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

/// <summary>
/// Applies to a cell, and spreads the power to all adjacent cells.
/// </summary>
public class SpreadPowerEffect
{
	// The power level of this effect.
	private readonly int _powerLevel;

	// The board to apply the power to.
	private readonly Board _board;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpreadPowerEffect"/> class.
	/// </summary>
	/// <param name="powerLevel">The power level of this effect.</param>
	/// <param name="board">The board to apply the power to.</param>
	public SpreadPowerEffect(int powerLevel, Board board)
	{
		this._powerLevel = powerLevel;
		this._board = board;
	}

	///// <summary>
	///// Applies the power effect to a cell.
	///// </summary>
	///// <param name="cell">
	///// A cell instance to apply the power effect to.
	///// </param>
	//public void Apply(Node3D cell) // TODO: Change to Cell.
	//{
	//	// Apply power to the cell.
	//	cell.Set("power_level", _powerLevel);

	//	// Apply power to adjacent cells.
	//	foreach (var adjacentCell in this._board.GetAdjacentCells(cell))
	//	{
	//		adjacentCell.PowerLevel = this._powerLevel;
	//	}
	//}
}
