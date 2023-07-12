// Copyright (c) Bruno Brant. All rights reserved.

using System.Diagnostics;
using Contagion.Scenes.Board.Scripts;
using Contagion.Scenes.Cell.Scripts;

/// <summary>
/// List of powers for a player.
/// </summary>
public partial class PowerList : GridContainer
{
	private Player? _player;

	/// <summary>
	/// Gets or sets the material of each power item.
	/// </summary>
	[Export]
	public Material? ItemMaterial { get; set; } 

	/// <inheritdoc/>
	public override void _Ready()
	{
		// TODO: I hate this code, please let's find a better way to do this.
		_player = GetParent().GetParent<Player>();
	}

	/// <inheritdoc/>
	public override void _Process(double delta)
	{
		Debug.Assert(_player != null, "Player is null!");

		var powers = _player.Powers;

		ClearChildren();

		var currentLevelRow = PowerLevel.MinPowerLevel;

		var subgrid = new GridContainer
		{
			Columns = 10, // TODO: make this dynamic (max number of powers per level)
		};

		AddChild(subgrid);

		foreach (var item in powers)
		{
			if (item.PowerLevel.Level != currentLevelRow)
			{
				subgrid = new GridContainer
				{
					Columns = 2,
				};

				AddChild(subgrid);
			}

			subgrid.AddChild(GetItemForPower(item));
		}
	}

	private Label GetItemForPower(PowerEffect item)
	{
		Debug.Assert(ItemMaterial != null, "Material is null!");

		var material = (ShaderMaterial)ItemMaterial.Duplicate();

		material.SetShaderParameter("Percent", item.PowerLevel.PercentCharged);

		return new Label
		{
			Text = $"Power {item.PowerLevel.Level}",
			Material = material,
		};
	}

	private void ClearChildren()
	{
		foreach (var child in GetChildren())
		{
			RemoveChild(child);
		}
	}
}
