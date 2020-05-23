using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    GameObject[] creatures;

    public int NoCreatures { get; private set; }
    public float AvgVelocity { get; private set; }
    public float AvgSight { get; private set; }
    public float AvgSize { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        creatures = GameObject.FindGameObjectsWithTag("Creature");
        NoCreatures = creatures.Length;
        AvgVelocity = GetAvgVelocity(creatures);
        AvgSight = GetAvgSight(creatures);
    }

    // Update is called once per frame
    void Update()
    {
        creatures = GameObject.FindGameObjectsWithTag("Creature") ;
        NoCreatures = creatures.Length;
        AvgVelocity = GetAvgVelocity(creatures);
        AvgSight = GetAvgSight(creatures);
        AvgSize = GetAvgSize(creatures);
    }

    private float GetAvgVelocity(GameObject[] creaturesIn) {
        float total = 0;

        foreach (GameObject creature in creaturesIn) {
            total += creature.GetComponent<CreatureAttributes>().Velocity;
        }

        return total / creaturesIn.Length;
    }

    private float GetAvgSight(GameObject[] creaturesIn) {
        float total = 0;

        foreach (GameObject creature in creaturesIn) {
            total += creature.GetComponent<CreatureAttributes>().SightRadius;
        }

        return total / creaturesIn.Length;
    }

    private float GetAvgSize(GameObject[] creaturesIn) {
        float total = 0;

        foreach (GameObject creature in creaturesIn) {
            total += creature.GetComponent<CreatureAttributes>().Size;
        }

        return total / creaturesIn.Length;
    }
}
