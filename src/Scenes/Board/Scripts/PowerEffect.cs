// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using System.Diagnostics;
using Contagion.Scenes.Cell.Scripts;

/// <summary>
/// Applies to a cell, and spreads the power to all adjacent cells.
/// </summary>
public class PowerEffect
{
	// The player that's applying the power.
	private readonly Player _player;

	// What cells have already been affected by this power effect.
	private readonly HashSet<Cell> _affectedCells = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="PowerEffect"/> class.
	/// </summary>
	/// <param name="powerLevel">The power level of this effect.</param>
	/// <param name="player">The player that's applying the power.</param>
	public PowerEffect(PowerLevel powerLevel, Player player)
	{
		PowerLevel = powerLevel;
		_player = player;
	}

	/// <summary>
	/// Gets the power level of this effect.
	/// </summary>
	public PowerLevel PowerLevel { get; init; }

	/// <summary>
	/// Applies the power effect to a cell.
	/// </summary>
	/// <param name="cell">
	/// A cell instance to apply the power effect to.
	/// </param>
	public void Apply(Cell cell)
	{
		Debug.Assert(cell.PowerLevel.CanApply(PowerLevel), $"Can't apply this PowerLevel to cell {cell.Name}!");

		cell.PowerLevel.Apply(PowerLevel, _player);

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
		foreach (var adjacentCell in from.Board.GetAdjacentCells(from))
		{
			adjacentCell.PowerLevel.Apply(spreadPower, _player);

			_affectedCells.Add(adjacentCell);
		}

		foreach (var adjacentCell in from.Board.GetAdjacentCells(from))
		{
			SpreadToNeighbors(adjacentCell);
		}
	}
}
