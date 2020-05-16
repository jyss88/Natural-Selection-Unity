using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public Transform spawnerTransform;
    public GameObject food;
    public float spawnRate = 4;
    public float numSpawn = 10;

    private float maxX;
    private float maxY;
    private float nextSpawn = 0;
    private Vector2 whereToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        maxX = GetComponentInParent<HabitatAttributes>().MaxX;
        maxY = GetComponentInParent<HabitatAttributes>().MaxY;

        spawnerTransform.localScale += new Vector3(2*maxX, 2*maxY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            for (int i = 0; i < numSpawn; i++) {
                whereToSpawn = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));

                GameObject foodObj = Instantiate(food, whereToSpawn, Quaternion.identity);

                foodObj.transform.parent = gameObject.transform;
            }     
        }
    }
}
