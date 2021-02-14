using UnityEngine;

static public class RectTransformExt
{
	/// <summary>
	/// Converts RectTransform.rect's local coordinates to world space
	/// Usage example RectTransformExt.GetWorldRect(myRect, Vector2.one);
	/// </summary>
	/// <returns>The world rect.</returns>
	/// <param name="rt">RectangleTransform we want to convert to world coordinates.</param>
	/// <param name="scale">Optional scale pulled from the CanvasScaler. Default to using Vector2.one.</param>
	static public Rect GetWorldRect(RectTransform rt, Vector2 scale)
	{
		// Convert the rectangle to world corners and grab the top left
		Vector3[] corners = new Vector3[4];
		rt.GetWorldCorners(corners);
		Vector3 topLeft = corners[0];

		// Rescale the size appropriately based on the current Canvas scale
		Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

		return new Rect(topLeft, scaledSize);
	}
}
