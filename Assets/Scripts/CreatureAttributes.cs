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
    public float sightRadius = 3;
   
    public float startingEnergy = 10;
    public float hoardEnergy = 10;
    private int generation = 1;

    private float deltaMutate = 0.1f;

    void Start() {
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
        if (Energy > (StartingEnergy + HoardEnergy)) {
            Vector3 offsetVector = new Vector3(transform.localScale.x, transform.localScale.y);
            GameObject child = Instantiate(gameObject, transform.position + offsetVector, Quaternion.identity);

            child.GetComponent<CreatureAttributes>().CloneAttributes(this);
            child.GetComponent<CreatureAttributes>().Mutate();
            child.transform.parent = gameObject.transform.parent;

            Energy -= StartingEnergy;
        }
    }

    public CreatureState State { get; set; } = CreatureState.Wander;

    public float Velocity {
        get { return velocity; }
    }

    public float SightRadius {
        get { return sightRadius;  }
    }

    public float Energy { get; set; }

    public float StartingEnergy {
        get { return startingEnergy; }
    }

    public float HoardEnergy {
        get { return hoardEnergy; }
    }

    public int Generation { get { return generation; } }

    public Collider2D TargetFood { get; set; }
    public void CloneAttributes(CreatureAttributes source) {
        velocity = source.Velocity;
        sightRadius = source.SightRadius;
        startingEnergy = source.StartingEnergy;
        hoardEnergy = source.HoardEnergy;
        generation = source.Generation + 1;
    }

    public void Mutate() {
        velocity = Mathf.Abs(velocity + Random.Range(-deltaMutate, deltaMutate));
        sightRadius = Mathf.Abs(sightRadius + Random.Range(-deltaMutate, deltaMutate));
        startingEnergy = Mathf.Abs(startingEnergy + Random.Range(-deltaMutate, deltaMutate));
        hoardEnergy = Mathf.Abs(hoardEnergy + Random.Range(-deltaMutate, deltaMutate));
    }

    // Update is called once per frame
    void Update()
    {
        checkEnergy();
        SwitchState();
        Reproduce();
    }
}
