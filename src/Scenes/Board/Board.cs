namespace Contagion.Scenes.Board.Scripts;

[Tool]
public partial class Board : Node3D
{
    /// <summary>
    /// The radius of the board.
    /// </summary>
	private int _currentRadius = 1;

    private float _currentDistance = 1;

    /// <summary>
    /// The distance between cells.
    /// </summary>
    // TODO: Get cell radius from mesh geometry!
    [Export]
    public float Distance { get; set; } = 1f;

    /// <summary>
    /// Cell resource to use for the board.
    /// </summary>
    [Export]
    public PackedScene CellScene { get; set; }

    /// <summary>
    /// Gets or sets the radius of the board.
    /// </summary>
    [Export]
    public int Radius { get; set; }

    /// <inheritdoc/>
    public override void _Process(double delta)
    {
        // TODO: Don't always recreate?
        if (Radius != _currentRadius || _currentDistance != Distance)
        {
            RecreateBoard();

            _currentRadius = Radius;
            _currentDistance = Distance;
        }
    }

    private void RecreateBoard()
    {
        RemoveChildren();

        var toVisit = new Queue<HexCoord> { HexCoord.Zero };
        var visited = new HashSet<HexCoord> { };

        GD.Print("Recreating board");

        while (toVisit.Any())
        {
            var coord = toVisit.Dequeue();

            // Check if we still are within Radius
            if (coord.Length > Radius)
            {
                continue;
            }

            // We are traversing the hexgrid depth-first, but we'll touch cells more than once
            // This check avoids looping eternally
            if (visited.Contains(coord))
            {
                continue;
            }

            _ = visited.Add(coord);
            
            AddCell(coord);

            GD.Print($"Grid has {GetChildCount()} elements.");

            foreach (var neighborCoord in coord.GetNeighbors())
            {
                GD.Print("enqueuing neighbor coord");
                toVisit.Enqueue(neighborCoord);
            }
        }
    }

    private void RemoveChildren()
    {
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
    }

    /// <summary>
    /// Adds a cell at the specified location.
    /// </summary>
    /// <param name="coord">
    /// The coordinate of this cell.
    /// </param>
    /// <returns>
    /// The cell that was added.
    /// </returns>
    private void AddCell(HexCoord coord)
    {
        GD.Print($"Adding cell at {coord}.");

        var cell = CellScene.Instantiate<Node3D>();

        cell.Name = $"Cell_{coord}";

        AddChild(cell);

        cell.Translate(coord.ToVector(Distance));
    }
}