using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float spawnRate = 5;
   
    public float startingEnergy = 10;
    private float maxX, maxY;

    void Start() {
        maxX = Camera.main.orthographicSize;
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

    public Collider2D TargetFood { get; set; }
    public void CloneAttributes(CreatureAttributes source) {
        velocity = source.Velocity;
        sightRadius = source.SightRadius;
        startingEnergy = source.StartingEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        checkEnergy();
        SwitchState();
    }
}
