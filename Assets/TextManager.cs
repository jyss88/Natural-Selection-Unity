using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text noCreaturesText;
    public Text avgVelocityText;
    public Text avgSightText;
    public Text avgSizeText;

    public void UpdateCreatureText(int noCreatures) {
        noCreaturesText.text = "Number of creatures: " + noCreatures.ToString();
    }

    public void UpdateVelocityText(float avgVelocity) {
        avgVelocityText.text = "Avg velocity: " + avgVelocity.ToString("F2");
    }

    public void UpdateSightText(float avgSight) {
        avgSightText.text = "Avg sight radius: " + avgSight.ToString("F2");
    }

    public void UpdateSizeText(float avgSize) {
        avgSizeText.text = "Avg size: " + avgSize.ToString("F2");
    }
}
