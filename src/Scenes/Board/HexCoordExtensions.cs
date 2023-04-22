// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

/// <summary>
/// Extensions for the <see cref="HexCoord"/> class.
/// </summary>
public static class HexCoordExtensions
{
	/// <summary>
	///    Converts a <see cref="HexCoord"/> to a <see cref="Vector3"/>.
	/// </summary>
	/// <param name="coord">
	/// The <see cref="HexCoord"/> to convert.
	/// </param>
	/// <param name="size">
	/// The size of the hexagon.
	/// </param>
	/// <returns>
	/// A <see cref="Vector3"/> representing the <see cref="HexCoord"/> according to <paramref name="size"/>.
	/// </returns>
	public static Vector3 ToVector(this HexCoord coord, float size)
	{
		var x = size * ((Mathf.Sqrt(3) * coord.Q) + (Mathf.Sqrt(3) / 2 * coord.R));
		var z = size * (3.0f / 2 * coord.R);

		return new Vector3(x, 0, z);

		//var x = size * 3.0f / 2.0f * coord.Q;
		//var z = size * Mathf.Sqrt(3.0f) * (coord.R + coord.Q / 2.0f);
		//return new Vector3(x, 0, z);
	}
}
