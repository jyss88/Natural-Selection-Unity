using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStats gameStats;
    public TextManager textManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText() {
        textManager.UpdateCreatureText(gameStats.NoCreatures);
        textManager.UpdateVelocityText(gameStats.AvgVelocity);
        textManager.UpdateSightText(gameStats.AvgSight);
        textManager.UpdateSizeText(gameStats.AvgSize);
    }
}
