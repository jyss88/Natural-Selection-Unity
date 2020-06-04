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

    private List<float> timeBuf = new List<float>();
    private List<int> countBuf = new List<int>();
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

        ClearFiles();
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
            /*while (TimeManager.isPaused) {
                Debug.Log("Data loop paused");
                yield return null;
            }*/

            // PROTECTED REGION
            mut.WaitOne();
            Debug.Log("Data collection is entering protected region.");
            timeBuf.Add(Time.time);
            countBuf.Add(GameStats.creatureAttributes.Count);

            velocityBuf.Add(GetVelocities(GameStats.creatureAttributes));
            sightBuf.Add(GetSights(GameStats.creatureAttributes));
            sizeBuf.Add(GetSizes(GameStats.creatureAttributes));
            Debug.Log("Data collection is exiting protected region.");
            mut.ReleaseMutex();
            // END PROTECTED REGION

            yield return new WaitForSeconds(pollingPeriod);
        }

        Debug.Log("Data collection complete");
    }

    IEnumerator DataSaveLoop() {
        while(true) {
            /*while (TimeManager.isPaused) {
                Debug.Log("Saving data is paused");
                yield return new WaitForSecondsRealtime(10f);
            }*/

            // write data
            SaveFiles();

            yield return new WaitForSecondsRealtime(60f);
        }
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

    private void ClearFiles() {
        using (StreamWriter timeW = new StreamWriter(path + timePath, append: false))
        using (StreamWriter countW = new StreamWriter(path + countPath, append: false))
        using (StreamWriter velocityW = new StreamWriter(path + velocityPath, append: false))
        using (StreamWriter sightW = new StreamWriter(path + sightPath, append: false))
        using (StreamWriter sizeW = new StreamWriter(path + sizePath, append: false)) {
            timeW.WriteLine("");
            countW.WriteLine("");
            velocityW.WriteLine("");
            sightW.WriteLine("");
            sizeW.WriteLine("");
        }

    }

    public void SaveFiles() {
        if (timeBuf.Count > 0) {
            // Enter protected region
            mut.WaitOne();
            Debug.Log("Data saving coroutine is entering protected region.");

            using (StreamWriter timeW = new StreamWriter(path + timePath, append: true)) {
                foreach (float entry in timeBuf) {
                    timeW.WriteLine(entry.ToString());
                }
            }

            using (StreamWriter countW = new StreamWriter(path + countPath, append: true)) {
                foreach (int entry in countBuf) {
                    countW.WriteLine(entry.ToString());
                }
            }

            using (StreamWriter velocityW = new StreamWriter(path + velocityPath, append: true)) {
                foreach (List<float> row in velocityBuf) {
                    WriteListData(row, velocityW);
                }
            }

            using (StreamWriter sightW = new StreamWriter(path + sightPath, append: true)) {
                foreach (List<float> row in sightBuf) {
                    WriteListData(row, sightW);
                }
            }

            using (StreamWriter sizeW = new StreamWriter(path + sizePath, append: true)) {
                foreach (List<float> row in sizeBuf) {
                    WriteListData(row, sizeW);
                }
            }

            ClearBuffers(); // Clear buffers

            // Exit protected region
            Debug.Log("Data saving coroutine is exiting protected region.");
            mut.ReleaseMutex();
        } 
    }

    private void ClearBuffers() {
        timeBuf.Clear();
        countBuf.Clear();
        velocityBuf.Clear();
        sightBuf.Clear();
        sizeBuf.Clear();
    }



    private void WriteListData(List<float> list, StreamWriter writer) {
        writer.WriteLine(string.Join(",", list));
    }
}
