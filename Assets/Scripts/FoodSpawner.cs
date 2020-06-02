using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSpawner : MonoBehaviour
{
    public Transform spawnerTransform;
    public GameObject food;
    public Text numFoodText;
    public float spawnRate = 1;
    public float numSpawn = 10;

    private float nextSpawn = 0;
    private Vector2 whereToSpawn;
    private HabitatAttributes habAttr;

    // Start is called before the first frame update
    void Start()
    {
        habAttr = gameObject.GetComponentInParent<HabitatAttributes>();
        numFoodText.text = numSpawn.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            for (int i = 0; i < numSpawn; i++) {
                whereToSpawn = new Vector2(Random.Range(habAttr.MinX, habAttr.MaxX), Random.Range(habAttr.MinY, habAttr.MaxY));

                GameObject foodObj = Instantiate(food, whereToSpawn, Quaternion.identity);

                foodObj.transform.parent = gameObject.transform;
            }     
        }
    }

    public void ChangeNumSpawn(float newNumSpawn) {
        numSpawn = newNumSpawn;
        numFoodText.text = Mathf.RoundToInt(numSpawn).ToString();
    }
}
