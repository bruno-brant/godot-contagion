// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Cell.Scripts;

using Contagion.Scenes.Board.Scripts;

/// <summary>
/// Represents a player in the game.
/// </summary>
public partial class Player : Node
{
	/// <summary>
	/// Gets or sets the color of the player.
	/// </summary>
	[Export(hintString: "The color of the player")]
	public Color Color { get; set; }

	/// <summary>
	/// Gets the powers available to the player.
	/// </summary>
	public List<PowerEffect> Powers { get; } = new();

	/// <inheritdoc/>
	public override void _Ready()
	{
		base._Ready();

		Powers.Add(new PowerEffect(new PowerLevel(PowerLevel.MinPowerLevel), this));
		Powers.Add(new PowerEffect(new PowerLevel(PowerLevel.MinPowerLevel), this));
		Powers.Add(new PowerEffect(new PowerLevel(PowerLevel.MinPowerLevel + 1), this));
		Powers.Add(new PowerEffect(new PowerLevel(PowerLevel.MinPowerLevel + 2), this));
		Powers.Add(new PowerEffect(new PowerLevel(PowerLevel.MinPowerLevel + 3), this));
	}
}
