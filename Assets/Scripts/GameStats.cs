using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling game statistics
/// </summary>
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

    /// <summary>
    /// Gets the average velocity of all creatures
    /// </summary>
    /// <param name="attrList"></param>
    /// <returns></returns>
    private float GetAvgVelocity(List<CreatureAttributes> attrList) {
        float total = 0;

        foreach (CreatureAttributes creature in attrList) {
            total += creature.Velocity;
        }

        return total / attrList.Count;
    }

    /// <summary>
    /// Gets average sight radius of all creatures
    /// </summary>
    /// <param name="attrList">List of creature attributes</param>
    /// <returns></returns>
    private float GetAvgSight(List<CreatureAttributes> attrList) {
        float total = 0;

        foreach (CreatureAttributes creature in attrList) {
            total += creature.SightRadius;
        }

        return total / attrList.Count;
    }

    /// <summary>
    /// Gets average size of all creatures
    /// </summary>
    /// <param name="attrList">List of creature attributes</param>
    /// <returns></returns>
    private float GetAvgSize(List<CreatureAttributes> attrList) {
        float total = 0;

        foreach (CreatureAttributes creature in attrList) {
            total += creature.Size;
        }

        return total / attrList.Count;
    }
}
