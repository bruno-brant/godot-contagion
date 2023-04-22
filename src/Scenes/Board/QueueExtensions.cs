// Copyright (c) Bruno Brant. All rights reserved.

namespace Contagion.Scenes.Board.Scripts;

/// <summary>
/// Extensions for the <see cref="Queue{T}"/> class.
/// </summary>
public static class QueueExtensions
{
	/// <summary>
	///     A convenience method to add an element to a queue.
	/// </summary>
	/// <typeparam name="T">
	///     The type of elements in the queue.
	/// </typeparam>
	/// <param name="queue">The queue to add the element to.</param>
	/// <param name="item">The item to add.</param>
	/// <remarks>
	///     The reason for this method is that the <see cref="Queue{T}"/> class
	///     doesn't have an <c>Add</c> method, which prevents us from using
	///     a syntax like <c>[<![CDATA[new Queue<int> { 1, 2, 3 };]]></c>.
	///
	///     I imagine that this is by design, because order matters in a queue and
	///     this syntax would be misleading. It's also ununsual to use a queue that
	///     has been initialized with elements, but I have such a case here.
	/// </remarks>
	public static void Add<T>(this Queue<T> queue, T item)
		where T : class
	{
		queue.Enqueue(item);
	}
}
