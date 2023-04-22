// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using System.Diagnostics;

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
	public PowerLevel(int level)
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
	public delegate void LevelChangedEventHandler(int level);

	/// <summary>
	/// Event raised when the level of power changes.
	/// </summary>
	public event LevelChangedEventHandler? LevelChanged;

	/// <summary>
	/// Gets the level of power.
	/// </summary>
	public int Level { get; private set; }

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
		Debug.Assert(other.Level == MinPowerLevel, $"Can never apply a {nameof(PowerLevel)} of minimum power.");

		return Level != MaxPowerLevel && Level < other.Level;
	}

	/// <summary>
	/// Increases the power of this instance absorbing the power of another instance.
	/// </summary>
	/// <param name="powerLevel">
	/// The power to absorb.
	/// </param>
	public void Apply(PowerLevel powerLevel)
	{
		Debug.Assert(CanApply(powerLevel), $"Can't apply power with level = {powerLevel.Level}");

		if (Level == powerLevel.Level)
		{
			Level++;
		}
		else
		{
			Level = powerLevel.Level;
		}

		LevelChanged?.Invoke(Level);
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
