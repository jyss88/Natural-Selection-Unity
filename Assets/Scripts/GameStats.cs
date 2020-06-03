using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public static List<CreatureAttributes> creatureAttributes = new List<CreatureAttributes>();
    public int NoCreatures { get; private set; }
    public float AvgVelocity { get; private set; }
    public float AvgSight { get; private set; }
    public float AvgSize { get; private set; }

    public List<CreatureAttributes> Creatures { get { return creatureAttributes; } }

    // Start is called before the first frame update
    void Start()
    {
        NoCreatures = creatureAttributes.Count;
        AvgVelocity = GetAvgVelocity(creatureAttributes);
        AvgSight = GetAvgSight(creatureAttributes);
    }

    // Update is called once per frame
    void Update()
    {
        NoCreatures = creatureAttributes.Count;
        AvgVelocity = GetAvgVelocity(creatureAttributes);
        AvgSight = GetAvgSight(creatureAttributes);
        AvgSize = GetAvgSize(creatureAttributes);
    }

    private float GetAvgVelocity(List<CreatureAttributes> creaturesIn) {
        float total = 0;

        foreach (CreatureAttributes creature in creaturesIn) {
            total += creature.Velocity;
        }

        return total / creaturesIn.Count;
    }

    private float GetAvgSight(List<CreatureAttributes> creaturesIn) {
        float total = 0;

        foreach (CreatureAttributes creature in creaturesIn) {
            total += creature.SightRadius;
        }

        return total / creaturesIn.Count;
    }

    private float GetAvgSize(List<CreatureAttributes> creaturesIn) {
        float total = 0;

        foreach (CreatureAttributes creature in creaturesIn) {
            total += creature.Size;
        }

        return total / creaturesIn.Count;
    }
}
