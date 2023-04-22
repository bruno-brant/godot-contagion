// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>
/// A cube coordinate for hexagonal space.
/// </summary>
/// <remarks>
/// See more at https://www.redblobgames.com/grids/hexagons.
/// </remarks>
public class HexCoord
{
	/// <summary>
	/// Zero value for coordinates.
	/// </summary>
	public static readonly HexCoord Zero = new(0, 0, 0);

	/// <summary>
	/// List of directions for neighbors of a given cell.
	/// </summary>
	public static readonly IReadOnlyList<HexCoord> NeighborsDirections = new[]
	{
		// TODO: Extract this as members (Up, Down, etc...)
		new HexCoord(1, -1, 0), new HexCoord(1, 0, -1), new HexCoord(0, 1, -1),
		new HexCoord(-1, 1, 0), new HexCoord(-1, 0, 1), new HexCoord(0, -1, 1),
	};

	/// <summary>
	/// Initializes a new instance of the <see cref="HexCoord"/> class.
	/// </summary>
	/// <param name="q">
	/// The Q axis location.
	/// </param>
	/// <param name="r">
	/// The R axis location.
	/// </param>
	/// <param name="s">
	/// The S axis location.
	/// </param>
	public HexCoord(int q, int r, int s)
	{
		Q = q;
		R = r;
		S = s;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HexCoord"/> class.
	/// </summary>
	/// <param name="q">
	/// The Q position.
	/// </param>
	/// <param name="r">
	/// The R position.
	/// </param>
	public HexCoord(int q, int r)
	{
		Q = q;
		R = r;
		S = -q - r;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HexCoord"/> class.
	/// </summary>
	public HexCoord()
	{
	}

	/// <summary>
	/// Gets or sets the q axis value.
	/// </summary>
	public int Q { get; set; } = 0;

	/// <summary>
	/// Gets or sets the r axis value.
	/// </summary>
	public int R { get; set; } = 0;

	/// <summary>
	/// Gets or sets the s axis value.
	/// </summary>
	public int S { get; set; } = 0;

	/// <summary>
	/// Gets the length of this coordinate.
	/// </summary>
	/// <returns>
	/// The length (magniture) of the coordinate.
	/// </returns>
	public uint Length => (uint)((Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2);

	/// <summary>
	/// Adds two HexCoord.
	/// </summary>
	/// <param name="left">Left operand.</param>
	/// <param name="right">Right operand.</param>
	/// <returns>
	/// A new instance of <see cref="HexCoord"/> that's the result of the addition.
	/// </returns>
	public static HexCoord operator +(HexCoord left, HexCoord right)
	{
		return new HexCoord(left.Q + right.Q, left.R + right.R, left.S + right.S);
	}

	/// <summary>
	///     Subtracts two <see cref="HexCoord"/>.
	/// </summary>
	/// <param name="left">Coord that will be subtracted from.</param>
	/// <param name="right">Coord to subtract.</param>
	/// <returns>
	///     A new instance of <see cref="HexCoord"/> that's the result of the subtraction.
	/// </returns>
	public static HexCoord operator -(HexCoord left, HexCoord right)
	{
		return new HexCoord(left.Q - right.Q, left.R - right.R, left.S - right.S);
	}

	/// <summary>
	///     Multiplies HexCoord by a scalar.
	/// </summary>
	/// <param name="h">The <see cref="HexCoord"/> to multiply.</param>
	/// <param name="m">The magnitude to multiple by.</param>
	/// <returns>
	///     A new <see cref="HexCoord"/>instance that is the result of the multiplication.
	/// </returns>
	public static HexCoord operator *(HexCoord h, int m)
	{
		return new HexCoord(h.Q * m, h.R * m, h.S * m);
	}

	/// <summary>
	///     Checks if the two <see cref="HexCoord"/> are equal.
	/// </summary>
	/// <param name="left">Left operand.</param>
	/// <param name="right">Right operand.</param>
	/// <returns>
	///     True if <paramref name="right"/> equals <paramref name="left"/>.
	/// </returns>
	public static bool operator ==(HexCoord left, HexCoord right)
	{
		return left.Q == right.Q && left.R == right.R && left.S == right.S;
	}

	/// <summary>
	///     Checks if two <see cref="HexCoord"/> are different.
	/// </summary>
	/// <param name="this">Left operand.</param>
	/// <param name="other">Right operand.</param>
	/// <returns>
	///     True if <paramref name="other"/> is different to <paramref name="this"/>.
	/// </returns>
	public static bool operator !=(HexCoord @this, HexCoord other)
	{
		return !(@this == other);
	}

	/// <summary>
	/// Returns the distance between two coordinates.
	/// </summary>
	/// <param name="left">The left operand.</param>
	/// <param name="right">The right operand.</param>
	/// <returns>
	/// The distance (in units) between the two <see cref="HexCoord"/>.
	/// </returns>
	public static uint Distance(HexCoord left, HexCoord right)
	{
		return (left - right).Length;
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (obj is HexCoord other)
		{
			return this == other;
		}

		return false;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return HashCode.Combine(Q, R, S);

		// This used to work, if necessary
		//return (Q * 1000) + (R * 100) + (S * 10);
	}

	/// <summary>
	/// Get the coordinates for all the six neighbors of this coordinate.
	/// </summary>
	/// <returns>
	/// A list with the coordinates of each of the six neighbors of this coordinate.
	/// </returns>
	public List<HexCoord> GetNeighbors()
	{
		var current = this;

		return NeighborsDirections.Select(_ => _ + current).ToList();
	}

	/// <inheritdoc/>
	public override string ToString() => $"[{Q} {R} {S}]";
}
