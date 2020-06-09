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
    public float size = 1;
    private int generation = 1;

    private float deltaMutate = 0.1f;

    public CreatureState State { get; set; } = CreatureState.Wander;

    public float Velocity {
        get { return velocity; }
    }

    public float SightRadius {
        get { return sightRadius; }
    }

    public float Energy { get; set; }

    public float StartingEnergy {
        get { return startingEnergy; }
    }

    public float Size {
        get { return size; }
        set {
            size = value;
            transform.localScale = new Vector3(size, size, 0);
        }
    }

    public int Generation { get { return generation; } }

    public Collider2D TargetFood { get; set; }

    void Start() {
        Energy = startingEnergy;
        GameStats.creatureAttributes.Add(this);
    }

    private void OnDestroy() {
        GameStats.creatureAttributes.Remove(this);
    }

    // Update is called once per frame
    void Update() {
        checkEnergy();
        SwitchState();
        Reproduce();
    }

    /// <summary>
    /// Checks energy of creature.
    /// If energy is < 0, creature is destroyed.
    /// </summary>
    private void checkEnergy() {
        if (Energy <= 0) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Switches state to hunt if TargetFood exists
    /// </summary>
    private void SwitchState() {
        if (TargetFood) {
            State = CreatureState.Hunt;
        } else {
            State = CreatureState.Wander;
        }
    }

    /// <summary>
    /// Reproduces creature.
    /// </summary>
    private void Reproduce() {
        if (Energy > (StartingEnergy + Size * 10)) { // Creature's energy must exceed a threshold, depending on creature size
            // Instantiate new creature
            Vector3 offsetVector = new Vector3(transform.localScale.x, transform.localScale.y);
            GameObject child = Instantiate(gameObject, transform.position + offsetVector, Quaternion.identity);

            // Clone & mutate child's attributes
            child.GetComponent<CreatureAttributes>().CloneAttributes(this);
            child.GetComponent<CreatureAttributes>().Mutate();
            child.transform.parent = gameObject.transform.parent;

            Energy -= StartingEnergy; // Decrement energy
        }
    }

    /// <summary>
    /// Clones attributes from source creature
    /// </summary>
    /// <param name="source">Source creature to copy attributes from</param>
    public void CloneAttributes(CreatureAttributes source) {
        velocity = source.Velocity;
        sightRadius = source.SightRadius;
        startingEnergy = source.StartingEnergy;
        Size = source.Size;
        generation = source.Generation + 1;
    }

    /// <summary>
    /// Mutates creature attributes
    /// </summary>
    public void Mutate() {
        velocity = Mathf.Abs(velocity + Random.Range(-deltaMutate, deltaMutate));
        sightRadius = Mathf.Abs(sightRadius + Random.Range(-deltaMutate, deltaMutate));
        startingEnergy = Mathf.Abs(startingEnergy + Random.Range(-deltaMutate, deltaMutate));
        Size = Mathf.Clamp(Size + Random.Range(-deltaMutate, deltaMutate), 0.1f, 1000f);
    }
}
