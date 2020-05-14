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

    private Vector2 moveSpot;
    private float timeSinceChanged = 0;

    // Start is called before the first frame update
    void Start()
    {
        moveSpot = new Vector2(Random.Range(-attributes.Bounds.x, attributes.Bounds.x), Random.Range(-attributes.Bounds.y, attributes.Bounds.y));
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

        // energy consumed at rate of (velocityConsumFactor * velocity^2) units per second
        attributes.Energy -= velocityConsumFactor * Mathf.Pow(attributes.Velocity, 2) * Time.deltaTime;
    }

    private void MoveRandomly() {
        float minDist = 0.2f;

        transform.position = Vector2.MoveTowards(transform.position, moveSpot, attributes.velocity * Time.deltaTime);
        timeSinceChanged += Time.deltaTime;

        if (Vector2.Distance(transform.position, moveSpot) < minDist || timeSinceChanged > 10) {
            moveSpot = new Vector2(Random.Range(-attributes.Bounds.x, attributes.Bounds.x), Random.Range(-attributes.Bounds.y, attributes.Bounds.y));
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
