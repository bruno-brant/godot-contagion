// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Tests.Scenes.Board.Scripts;

using AutoFixture.Xunit2;
using Contagion.Scenes.Board.Scripts;

public class HexCoordTests
{
	[Theory, AutoData]
	public void Equals_WhenSameCoords_ResultsTrue(int q, int r, int s)
	{
		var hexCoords1 = new HexCoord(q, r, s);
		var hexCoords2 = new HexCoord(q, r, s);

		Assert.Equal(hexCoords1, hexCoords2);
		Assert.True(hexCoords1.Equals(hexCoords2));
		Assert.True(hexCoords1 == hexCoords2);
	}

	[Theory, AutoData]
	public void Equals_WhenDifferentCoords_ResultsFalse(int q, int r, int s)
	{
		var hexCoords1 = new HexCoord(q, r, s);

		var coords = ShuffleArray(new[] { q, r, s });
		var hexCoords2 = new HexCoord(coords[0], coords[1], coords[2]);

		Assert.NotEqual(hexCoords1, hexCoords2);
		Assert.False(hexCoords1.Equals(hexCoords2));
		Assert.False(hexCoords1 == hexCoords2);
	}

	[Theory, AutoData]
	public void GetHashCode_WhenSameCoords_SameHash(int q, int r, int s)
	{
		var hexCoords1 = new HexCoord(q, r, s);
		var hexCoords2 = new HexCoord(q, r, s);

		Assert.Equal(hexCoords1.GetHashCode(), hexCoords2.GetHashCode());
	}

	[Theory, AutoData]
	public void GetHashCode_WhenDifferentCoords_DifferentHash(int q, int r, int s)
	{
		var hexCoords1 = new HexCoord(q, r, s);

		var coords = ShuffleArray(new[] { q, r, s });
		var hexCoords2 = new HexCoord(coords[0], coords[1], coords[2]);

		Assert.NotEqual(hexCoords1.GetHashCode(), hexCoords2.GetHashCode());
	}

	[Theory]
	[InlineAutoData]
	[InlineAutoData]
	[InlineAutoData]
	[InlineAutoData]
	[InlineData(0, 0, 0)]
	public void HashSetContains_WhenSameCoords_ReturnsTrue(int q, int r, int s)
	{
		var hexCoords1 = new HexCoord(q, r, s);
		var hexCoords2 = new HexCoord(q, r, s);

		var set = new HashSet<HexCoord> { hexCoords1 };

		Assert.Contains(hexCoords2, set);
		Assert.Contains(hexCoords2, set);
	}

	/// <summary>
	///     Shuffles the array using the Fisher-Yates algorithm.
	/// </summary>
	/// <param name="array">
	///     The array to shuffle.
	/// </param>
	/// <returns>
	///     The input array, shuffled.
	/// </returns>
	/// <remarks>
	///     Modifies the array in-place.
	/// </remarks>
	private static T[] ShuffleArray<T>(T[] array)
	{
		var random = new Random();

		for (var i = 0; i < array.Length; i++)
		{
			var j = random.Next(i, array.Length);

			// tuple swap
			(array[j], array[i]) = (array[i], array[j]);
		}

		return array;
	}
}
