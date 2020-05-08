using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class CreatureSight : MonoBehaviour
{
    public CreatureAttributes attributes;
    public LayerMask foodMask;

    private float radius;
    private float consumptionFactor = 0.1f;
    private Collider2D targetFood;

    // Start is called before the first frame update
    void Start()
    {
        FindVisibleFood();
        radius = attributes.SightRadius;
    }

    // Update is called once per frame
    void Update()
    {
        FindVisibleFood();
        attributes.Energy -= consumptionFactor * radius * Time.deltaTime;
    }

    /// <summary>
    /// Finds target food
    /// </summary>
    private void FindVisibleFood() {
        targetFood = Physics2D.OverlapCircle(transform.position, attributes.SightRadius, foodMask);
        attributes.TargetFood = targetFood;
    }
}
