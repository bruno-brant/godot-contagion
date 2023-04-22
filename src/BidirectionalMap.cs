// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion;

/// <summary>
/// Allows to associate between pairs of objects and retrieve them in both directions.
/// </summary>
/// <typeparam name="T1">
/// The type of the first object.
/// </typeparam>
/// <typeparam name="T2">
/// The type of the second object.
/// </typeparam>
public class BidirectionalMap<T1, T2>
{
	// Maps from T1 to T2.
	private readonly Dictionary<T1, T2> _map1 = new();

	// Maps from T2 to T1.
	private readonly Dictionary<T2, T1> _map2 = new();

	/// <summary>
	/// Associate the key1 with the key2.
	/// </summary>
	/// <param name="key1">
	/// The object that will be associated with key2.
	/// </param>
	/// <param name="key2">
	/// The object that will be associated with key1.
	/// </param>
	public void Add(T1 key1, T2 key2)
	{
		if (_map1.ContainsKey(key1) || _map2.ContainsKey(key2))
		{
			throw new ArgumentException("The key is already mapped.");
		}

		_map1.Add(key1, key2);
		_map2.Add(key2, key1);
	}

	/// <summary>
	/// Get the key mapped from key1.
	/// </summary>
	/// <param name="key1">
	/// The key to map from.
	/// </param>
	/// <returns>
	/// The key mapped from key1.
	/// </returns>
	public T2 Get(T1 key1)
	{
		if (_map1.TryGetValue(key1, out var key2))
		{
			return key2;
		}

		return default;
	}

	/// <summary>
	/// Get the key mapped from key1.
	/// </summary>
	/// <param name="key2">
	/// The key to map from.
	/// </param>
	/// <returns>
	/// The key mapped from key1.
	/// </returns>
	public T1 Get(T2 key2)
	{
		if (_map2.TryGetValue(key2, out var key1))
		{
			return key1;
		}

		return default;
	}

	/// <summary>
	/// Gets the key mapped from key1.
	/// </summary>
	/// <param name="key1">
	/// The key to map from.
	/// </param>
	/// <param name="key2">
	/// The resulting key.
	/// </param>
	/// <returns>
	/// True if the value was found, false otherwise.
	/// </returns>
	public bool TryGetValue(T1 key1, out T2 key2)
	{
		return _map1.TryGetValue(key1, out key2);
	}

	/// <summary>
	/// Gets the key mapped from key2.
	/// </summary>
	/// <param name="key2">
	/// The key to map from.
	/// </param>
	/// <param name="key1">
	/// The resulting key.
	/// </param>
	/// <returns>
	/// True if the value was found, false otherwise.
	/// </returns>
	public bool TryGetValue(T2 key2, out T1 key1)
	{
		return _map2.TryGetValue(key2, out key1);
	}
}
