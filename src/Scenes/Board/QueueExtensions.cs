namespace Contagion.Scenes.Board.Scripts;

public static class QueueExtensions
{
    public static void Add<T>(this Queue<T> queue, T item)
        where T : class
    {
        queue.Enqueue(item);
    }
}
