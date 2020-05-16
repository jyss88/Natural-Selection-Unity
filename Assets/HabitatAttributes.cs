using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        MaxX = screenBounds.x - rightOffset;
        MinX = -screenBounds.x + leftOffset;
        MaxY = screenBounds.y - topOffset;
        MinY = -screenBounds.y + bottomOffset;

        Width = MaxX - MinX;
        Height = MaxY - MinY;
    }

}
