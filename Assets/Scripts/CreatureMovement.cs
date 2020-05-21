using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class handling movement of creature
/// </summary>
public class CreatureMovement : MonoBehaviour
{

    public Transform creatureTransform;
    public CreatureAttributes attributes;

    private float velocityConsumFactor = 0.5f;
    private float sizeConsumFactor = 0.5f;

    private Vector2 moveSpot;
    private float timeSinceChanged = 0;
    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        minX = transform.parent.gameObject.GetComponent<HabitatAttributes>().MinX;
        maxX = transform.parent.gameObject.GetComponent<HabitatAttributes>().MaxX;
        minY = transform.parent.gameObject.GetComponent<HabitatAttributes>().MinY;
        maxY = transform.parent.gameObject.GetComponent<HabitatAttributes>().MaxY;
        moveSpot = CreateMoveSpot();
     }

    private Vector2 CreateMoveSpot() {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Move() {
        //Vector3 moveVector;

        switch (attributes.State) {
            case CreatureAttributes.CreatureState.Wander:
                MoveRandomly();
                break;
            case CreatureAttributes.CreatureState.Hunt:
                MoveToPrey();
                break;
            default:
                MoveRandomly();
                break;
        }

        // energy consumed at rate of (velocityConsumFactor * velocity^2 + sizeConsumFactor * size^3) units per second
        attributes.Energy -= (velocityConsumFactor * Mathf.Pow(attributes.Velocity, 2) + sizeConsumFactor * Mathf.Pow(attributes.Size, 3)) * Time.deltaTime;
    }

    private void MoveRandomly() {
        float minDist = 0.2f;

        transform.position = Vector2.MoveTowards(transform.position, moveSpot, attributes.velocity * Time.deltaTime);
        timeSinceChanged += Time.deltaTime;

        if (Vector2.Distance(transform.position, moveSpot) < minDist || timeSinceChanged > 10) {
            moveSpot = CreateMoveSpot();
            timeSinceChanged = 0;
        }
    }

    private void MoveToPrey() {
        if (attributes.TargetFood) {
            transform.position = Vector2.MoveTowards(transform.position, attributes.TargetFood.transform.position, attributes.velocity * Time.deltaTime);
        } else {
            MoveRandomly();
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
    }

    // Collision
    void OnCollisionEnter2D(Collision2D collision) {
        // Eat food
        if (collision.gameObject.tag.Equals("Food")) {
            attributes.Energy += collision.gameObject.GetComponent<Nutrition>().NutritionalValue;
            Destroy(collision.gameObject);
        }
    }
}
