using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureColor : MonoBehaviour
{
    public CreatureAttributes attr;

    public enum ColorState
    {
        Generation,
        Velocity,
        Sight,
        Size
    }

    public ColorState State { get; set; } = ColorState.Generation;

    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = attr.GenColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
