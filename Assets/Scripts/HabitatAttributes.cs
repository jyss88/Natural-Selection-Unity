using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handling habitat attributes
/// </summary>
public class HabitatAttributes : MonoBehaviour
{
    private Vector2 screenBounds;
    public float topOffset, bottomOffset, rightOffset, leftOffset = 0;
    private float minY;

    public float MaxX { get; private set; }

    public float MaxY { get; private set; }

    public float MinX { get; private set; }

    public float MinY { get; private set; }

    public float Width { get; private set; }

    public float Height { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate() {
        // Compute screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Convert to X and Y coordinates
        MaxX = screenBounds.x - rightOffset;
        MinX = -screenBounds.x + leftOffset;
        MaxY = screenBounds.y - topOffset;
        MinY = -screenBounds.y + bottomOffset;

        Width = MaxX - MinX;
        Height = MaxY - MinY;
    }

}
