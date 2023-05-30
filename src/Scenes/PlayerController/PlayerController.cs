// Copyright (c) Bruno Brant. All rights reserved.

namespace Contation.Scenes.PlayerController;

using Contagion.Scenes.Cell.Scripts;
using Microsoft.VisualBasic;

/// <summary>
/// Centralizes the processing of player input.
/// </summary>
public partial class PlayerController : Node3D
{
	private const float RayLength = 10000.0f;

	private Cell? _selectedCell;
	private StandardMaterial3D _redMaterial = new() { AlbedoColor = Colors.Red };

	/// <inheritdoc/>
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouse eventMouse)
		{
			HighlightMouseOver(eventMouse.Position);
		}
	}

	/// <summary>
	/// Highlights the tile the mouse is over.
	/// </summary>
	private void HighlightMouseOver(Vector2 mouseCursorPosition)
	{
		_selectedCell?.Dim();

		_selectedCell = GetCellUnderCursor(mouseCursorPosition);

		_selectedCell?.Highlight();
	}

	/// <summary>
	/// Returns the cell under the cursor, if any.
	/// </summary>
	/// <returns>
	/// The cell under the cursor, or null if there is none.
	/// </returns>
	private Cell? GetCellUnderCursor(Vector2 mouseCursorPosition)
	{
		var camera3d = GetViewport().GetCamera3D();

		var from = camera3d.ProjectRayOrigin(mouseCursorPosition);
		var to = from + (camera3d.ProjectRayNormal(mouseCursorPosition) * RayLength);

		var spaceState = GetWorld3D().DirectSpaceState;

		var query = PhysicsRayQueryParameters3D.Create(from, to);
		var result = spaceState.IntersectRay(query);

		if (OS.IsDebugBuild())
		{
			// Draw the ray if in debug
			DebugDrawRay(from, to);
		}

		ShowMessage($"Hit {result.Count} tiles");

		return result.Count > 0
			? result["collider"].As<Cell>()
			: null;
	}

	private void DebugDrawRay(Vector3 from, Vector3 to)
	{
		var mesh = (ImmediateMesh)GetNode<MeshInstance3D>("Ray").Mesh;

		mesh.ClearSurfaces();
		mesh.SurfaceBegin(Mesh.PrimitiveType.LineStrip, _redMaterial);
		mesh.SurfaceAddVertex(from);
		mesh.SurfaceAddVertex(to);
		mesh.SurfaceEnd();
	}

	private void ShowMessage(string message)
	{
		var label = GetNode<Label>("%DebugText");

		if (label != null)
		{
			label.Text = message;
		}
	}
}
