// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using System.Diagnostics;
using Contagion.Scenes.Cell.Scripts;
using Godot;

/// <summary>
/// Encapulates the rules for increasing power of cells.
/// </summary>
/// <remarks>
/// This object can both represent the power level of a cell or of a power effect.
/// </remarks>
public class PowerLevel
{
	/// <summary>
	/// The minimum power level of a cell.
	/// </summary>
	public const int MinPowerLevel = 1;

	/// <summary>
	/// The maximum power level of a cell.
	/// </summary>
	public const int MaxPowerLevel = 4;

	/// <summary>
	/// Initializes a new instance of the <see cref="PowerLevel"/> class.
	/// </summary>
	/// <param name="level">The level of power to initialize this to.</param>
	public PowerLevel(int level = MinPowerLevel)
	{
		if (level is < MinPowerLevel or > MaxPowerLevel)
		{
			throw new ArgumentOutOfRangeException(nameof(level), level, $"{nameof(level)} must be between {MinPowerLevel} and {MaxPowerLevel}");
		}

		Level = level;
	}

	/// <summary>
	/// Delegate for the <see cref="LevelChanged"/> event.
	/// </summary>
	/// <param name="level">The new level of power.</param>
	/// <param name="player">The player that applied the power.</param>
	public delegate void LevelChangedEventHandler(int level, Player player);

	/// <summary>
	/// Event raised when the level of power changes.
	/// </summary>
	public event LevelChangedEventHandler? LevelChanged;

	/// <summary>
	/// Gets the level of power.
	/// </summary>
	public int Level { get; private set; }

	/// <summary>
	/// Gets the percent of the power that was charged. Only charged powers can be used.
	/// </summary>
	public float PercentCharged { get; internal set; } = 0.75f;

	/// <summary>
	/// Gets a value indicating whether the power is charged and ready to use.
	/// </summary>
	public bool IsCharged => PercentCharged >= 1;

	/// <summary>
	/// Checks if the cell can be affected by this power effect.
	/// </summary>
	/// <param name="other">
	/// The cell to check.
	/// </param>
	/// <returns>
	/// True if the cell can be affected by this power effect, false otherwise.
	/// </returns>
	public bool CanApply(PowerLevel other)
	{
		Debug.Assert(other.Level != MinPowerLevel, $"Can never apply a {nameof(PowerLevel)} of minimum power.");

		return Level != MaxPowerLevel && Level < other.Level;
	}

	/// <summary>
	/// Increases the power of this instance absorbing the power of another instance.
	/// </summary>
	/// <param name="powerLevel">
	/// The power to absorb.
	/// </param>
	/// <param name="player">
	/// The player that's applying the power.
	/// </param>
	public void Apply(PowerLevel powerLevel, Player player)
	{
		Debug.Assert(CanApply(powerLevel), $"Can't apply power with level '{powerLevel.Level}'.");

		if (Level == powerLevel.Level)
		{
			Level++;
		}
		else
		{
			Level = powerLevel.Level;
		}

		LevelChanged?.Invoke(Level, player);
	}

	/// <summary>
	/// Get the power that spread from this instance.
	/// </summary>
	/// <returns>
	/// The power that spread from this instance.
	/// </returns>
	public PowerLevel? GetSpreadPower()
	{
		if (Level != MinPowerLevel)
		{
			return new PowerLevel(Level - 1);
		}

		return null;
	}
}
