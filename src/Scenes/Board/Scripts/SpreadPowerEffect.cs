// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using Contagion.Scenes.Cell.Scripts;

/// <summary>
/// Applies to a cell, and spreads the power to all adjacent cells.
/// </summary>
public class SpreadPowerEffect
{
	// The power level of this effect.
	private readonly PowerLevel _powerLevel;

	// The board to apply the power to.
	private readonly Board _board;

	// What cells have already been affected by this power effect.
	private readonly HashSet<Cell> _affectedCells = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="SpreadPowerEffect"/> class.
	/// </summary>
	/// <param name="powerLevel">The power level of this effect.</param>
	/// <param name="board">The board to apply the power to.</param>
	public SpreadPowerEffect(PowerLevel powerLevel, Board board)
	{
		_powerLevel = powerLevel;
		_board = board;
	}

	/// <summary>
	/// Applies the power effect to a cell.
	/// </summary>
	/// <param name="cell">
	/// A cell instance to apply the power effect to.
	/// </param>
	public void Apply(Cell cell)
	{
		if (!cell.PowerLevel.CanApply(_powerLevel))
		{
			throw new InvalidOperationException($"Can't apply this PowerLevel to cell {cell.Name}!");
		}

		cell.PowerLevel.Apply(_powerLevel);

		SpreadToNeighbors(cell);
	}

	/// <summary>
	/// Spread power from the current cell to adjacent cells.
	/// </summary>
	/// <param name="from">
	/// The cell to spread power from.
	/// </param>
	private void SpreadToNeighbors(Cell from)
	{
		if (_affectedCells.Contains(from))
		{
			// already affected
			return;
		}

		var spreadPower = from.PowerLevel.GetSpreadPower();

		if (spreadPower == null)
		{
			// no power spread from cell
			return;
		}

		// Apply power to adjacent cells.
		foreach (var adjacentCell in _board.GetAdjacentCells(from))
		{
			adjacentCell.PowerLevel.Apply(spreadPower);

			_affectedCells.Add(adjacentCell);
		}

		foreach (var adjacentCell in _board.GetAdjacentCells(from))
		{
			SpreadToNeighbors(adjacentCell);
		}
	}
}
