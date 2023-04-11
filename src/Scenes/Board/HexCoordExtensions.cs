namespace Contagion.Scenes.Board.Scripts;

public static class HexCoordExtensions
{
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