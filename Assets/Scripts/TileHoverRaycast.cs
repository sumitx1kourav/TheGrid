using UnityEngine;
using TMPro;

// Detects which tile the mouse is hovering over
public class TileHoverRaycast : MonoBehaviour
{
    public TextMeshProUGUI tileInfoText;

    void Update()
    {
        DetectTileUnderMouse();
    }

    void DetectTileUnderMouse()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Shoot the ray into the scene
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the object hit has TileData
            TileData tile = hit.collider.GetComponent<TileData>();

            if (tile != null)
            {

                tileInfoText.text = $"Tile: ({tile.x}, {tile.y})";
                return;
            }
        }

        // If no tile is hit
        tileInfoText.text = "Tile: -";
    }
}
