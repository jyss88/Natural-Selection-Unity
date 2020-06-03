using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class DataCollection : MonoBehaviour
{
    public float pollingPeriod = 10f;
    public float savePeriod = 30f;
    public GameStats stats;

    public static bool isCollecting = false;

    private float currentTime = 0;
    private int noCreatures = 0;

    private List<List<float>> velocityBuf = new List<List<float>>();
    private List<List<float>> sightBuf = new List<List<float>>();
    private List<List<float>> sizeBuf = new List<List<float>>();

    private Mutex mut = new Mutex();

    private string path;
    private string timePath = "time.csv";
    private string countPath = "counts.csv";
    private string velocityPath = "velocities.csv";
    private string sightPath = "sights.csv";
    private string sizePath = "sizes.csv";

    // Start is called before the first frame update
    void Start()
    {
        path = System.IO.Path.GetDirectoryName(Application.dataPath) + "/data/";
        Debug.Log("Data will be logged at path: " + path);
        isCollecting = true;
        StartCoroutine(DataCollectLoop());
        StartCoroutine(DataSaveLoop());
    }

    public void StartCollection() {
        isCollecting = true;
        StartCoroutine(DataCollectLoop());
    }

    public void StopCollection() {
        mut.WaitOne();
        isCollecting = false;
        mut.ReleaseMutex();
    }

    IEnumerator DataCollectLoop() {
        Debug.Log("Data collection started");
        while (isCollecting) {
            while (TimeManager.isPaused) {
                Debug.Log("Data loop paused");
                yield return null;
            }

            // PROTECTED REGION
            mut.WaitOne();
            Debug.Log("Data collection is entering protected region.");
            velocityBuf.Add(GetVelocities(GameStats.creatureAttributes));
            sightBuf.Add(GetSights(GameStats.creatureAttributes));
            sizeBuf.Add(GetSizes(GameStats.creatureAttributes));
            Debug.Log("Data collection is exiting protected region.");
            mut.ReleaseMutex();

            yield return new WaitForSeconds(pollingPeriod);
        }

        Debug.Log("Data collection complete");
    }

    IEnumerator DataSaveLoop() {
        while(true) {
            while (TimeManager.isPaused) {
                Debug.Log("Saving data is paused");
                yield return new WaitForSecondsRealtime(10f);
            }

            mut.WaitOne();
            Debug.Log("Data saving coroutine is entering protected region.");
            // write data

            ClearBuffers();

            Debug.Log("Data saving coroutine is exiting protected region.");
            mut.ReleaseMutex();

            yield return new WaitForSecondsRealtime(60f);
        }
    }

    private void ClearBuffers() {
        velocityBuf.Clear();
        sightBuf.Clear();
        sightBuf.Clear();
    }

    private List<float> GetVelocities(List<CreatureAttributes> creatureList) {
        List<float> temp = new List<float>();
        foreach(CreatureAttributes creature in creatureList) {
            temp.Add(creature.Velocity);
        }

        return temp;
    }

    private List<float> GetSights(List<CreatureAttributes> creatureList) {
        List<float> temp = new List<float>();
        foreach (CreatureAttributes creature in creatureList) {
            temp.Add(creature.GetComponent<CreatureAttributes>().SightRadius);
        }

        return temp;
    }

    private List<float> GetSizes(List<CreatureAttributes> creatureList) {
        List<float> temp = new List<float>();
        foreach (CreatureAttributes creature in creatureList) {
            temp.Add(creature.size);
        }

        return temp;
    }

    private void WriteListData(List<float> list, StreamWriter writer) {
        writer.WriteLine(string.Join(",", list), true);
    }
}
