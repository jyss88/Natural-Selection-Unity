using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing attributes & state of creature.
/// Intended to be common interface for creature attributes between all scripts
/// </summary>
public class CreatureAttributes : MonoBehaviour
{
    public enum CreatureState
    {
        Wander,
        Hunt,
    }

    public float velocity = 1;
    public float spawnChance = 0.5f;
    public float sightRadius = 3;
    public float spawnRate = 2;
   
    public float startingEnergy = 10;
    private float maxX, maxY;
    private float nextSpawn = 0;
    private int generation = 1;

    private float deltaMutate = 0.1f;

    void Start() {
        maxX = Camera.main.orthographicSize - transform.localScale.x;
        maxY = maxX * Screen.width / Screen.height;
        Energy = startingEnergy;
    }
    private void checkEnergy() {
        if (Energy <= 0) {
            Destroy(gameObject);
        }
    }

    private void SwitchState() {
        if (TargetFood) {
            State = CreatureState.Hunt;
        } else {
            State = CreatureState.Wander;
        }
    }

    private void Reproduce() {
        if (Time.time > nextSpawn) {
            nextSpawn += spawnRate;

            if ((Energy > 2 * StartingEnergy) && (Random.Range(0.0f, 1.0f) > SpawnChance)) {
                Vector3 offsetVector = new Vector3(transform.localScale.x, transform.localScale.y);
                GameObject child = Instantiate(gameObject, transform.position + offsetVector, Quaternion.identity);

                child.GetComponent<CreatureAttributes>().CloneAttributes(this);
                child.GetComponent<CreatureAttributes>().Mutate();

                Energy -= StartingEnergy;
            }
        }
    }

    public CreatureState State { get; set; } = CreatureState.Wander;

    public float Velocity {
        get { return velocity; }
    }

    public float SpawnChance {
        get { return spawnChance; }
    }

    public float SightRadius {
        get { return sightRadius;  }
    }

    public float SpawnRate {
        get { return spawnRate; }
    }

    public float Energy { get; set; }

    public float StartingEnergy {
        get { return startingEnergy; }
    }

    public Vector2 Bounds {
        get { return new Vector2(maxX, maxY); }
    }

    public int Generation { get { return generation; } }

    public Collider2D TargetFood { get; set; }
    public void CloneAttributes(CreatureAttributes source) {
        velocity = source.Velocity;
        sightRadius = source.SightRadius;
        startingEnergy = source.StartingEnergy;
        spawnChance = source.SpawnChance;
        generation = source.Generation + 1;
    }

    public void Mutate() {
        velocity = Mathf.Abs(velocity + Random.Range(-deltaMutate, deltaMutate));
        sightRadius = Mathf.Abs(sightRadius + Random.Range(-deltaMutate, deltaMutate));
        startingEnergy = Mathf.Abs(startingEnergy + Random.Range(-deltaMutate, deltaMutate));
        spawnChance = Mathf.Clamp(Mathf.Abs(spawnChance + Random.Range(-deltaMutate, deltaMutate)), 0.1f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        checkEnergy();
        SwitchState();
        Reproduce();
    }
}
