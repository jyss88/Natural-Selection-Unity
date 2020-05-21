using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    GameObject[] creatures;
    int noCreatures;
    float avgVelocity;
    float avgSight;
    float avgSize;

    public Text noCreaturesText;
    public Text avgVelocityText;
    public Text avgSightText;
    public Text avgSizeText;

    // Start is called before the first frame update
    void Start()
    {
        creatures = GameObject.FindGameObjectsWithTag("Creature");
        noCreatures = creatures.Length;
        avgVelocity = GetAvgVelocity(creatures);
        avgSight = GetAvgSight(creatures);
    }

    // Update is called once per frame
    void Update()
    {
        creatures = GameObject.FindGameObjectsWithTag("Creature") ;
        noCreatures = creatures.Length;
        avgVelocity = GetAvgVelocity(creatures);
        avgSight = GetAvgSight(creatures);
        avgSize = GetAvgSize(creatures);


        noCreaturesText.text = "Number of creatures: " + noCreatures.ToString();
        avgVelocityText.text = "Avg velocity: " + avgVelocity.ToString("F2");
        avgSightText.text = "Avg sight radius: " + avgSight.ToString("F2");
        avgSizeText.text = "Avg size: " + avgSize.ToString("F2");
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
