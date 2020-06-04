using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

/// <summary>
/// Class for handling data collection
/// </summary>
public class DataCollection : MonoBehaviour
{
    public GameStats stats;
    public static bool isCollecting = false;

    public float pollingPeriod = 10f; // Data sampling period
    public float savePeriod = 30f; // Data saving period

    // Data buffers
    private List<float> timeBuf = new List<float>();
    private List<int> countBuf = new List<int>();
    private List<List<float>> velocityBuf = new List<List<float>>();
    private List<List<float>> sightBuf = new List<List<float>>();
    private List<List<float>> sizeBuf = new List<List<float>>();

    private Mutex fileMut = new Mutex(); // Mutex for file access

    // Paths for output files
    private string path; // Data path
    private string timePath = "time.csv";
    private string countPath = "counts.csv";
    private string velocityPath = "velocities.csv";
    private string sightPath = "sights.csv";
    private string sizePath = "sizes.csv";

    // Start is called before the first frame update
    void Start()
    {
        path = System.IO.Path.GetDirectoryName(Application.dataPath) + "/data/"; // Find data directory
        Debug.Log("Data will be logged at path: " + path);

        ClearFiles();
        isCollecting = true;

        // Start coroutines
        StartCoroutine(DataCollectLoop());
        StartCoroutine(DataSaveLoop());
    }

    /// <summary>
    /// Starts collecting coroutine
    /// </summary>
    public void StartCollection() {
        isCollecting = true;
        StartCoroutine(DataCollectLoop());
    }

    /// <summary>
    /// Stops collecting coroutine
    /// </summary>
    public void StopCollection() {
        fileMut.WaitOne();
        isCollecting = false;
        fileMut.ReleaseMutex();
    }

    /// <summary>
    /// Coroutine for collecting data
    /// </summary>
    /// <returns></returns>
    IEnumerator DataCollectLoop() {
        Debug.Log("Data collection started");
        while (isCollecting) {
            // PROTECTED REGION
            fileMut.WaitOne(); // Signal mutex
            Debug.Log("Data collection is entering protected region.");

            // Add data to buffers
            timeBuf.Add(Time.time);
            countBuf.Add(GameStats.creatureAttributes.Count);

            velocityBuf.Add(GetVelocities(GameStats.creatureAttributes));
            sightBuf.Add(GetSights(GameStats.creatureAttributes));
            sizeBuf.Add(GetSizes(GameStats.creatureAttributes));

            Debug.Log("Data collection is exiting protected region.");
            fileMut.ReleaseMutex(); // Signal mutex
            // END PROTECTED REGION

            yield return new WaitForSeconds(pollingPeriod); // Wait for polling period
        }

        Debug.Log("Data collection complete");
    }

    /// <summary>
    /// Coroutine for saving data
    /// </summary>
    /// <returns></returns>
    IEnumerator DataSaveLoop() {
        while(true) {
            // write data
            SaveFiles();

            yield return new WaitForSecondsRealtime(60f);
        }
    }

    /// <summary>
    /// Collects velocity data & saves it to buffer
    /// </summary>
    /// <param name="attrList">List of creature attributes</param>
    /// <returns>List of creature velocities</returns>
    private List<float> GetVelocities(List<CreatureAttributes> attrList) {
        List<float> temp = new List<float>();
        foreach(CreatureAttributes creature in attrList) {
            temp.Add(creature.Velocity);
        }

        return temp;
    }

    /// <summary>
    /// Collects creature sight radius data & saves it to buffer
    /// </summary>
    /// <param name="attrList">List of creature attributes</param>
    /// <returns>List of creature sight radii</returns>
    private List<float> GetSights(List<CreatureAttributes> attrList) {
        List<float> temp = new List<float>();
        foreach (CreatureAttributes creature in attrList) {
            temp.Add(creature.GetComponent<CreatureAttributes>().SightRadius);
        }

        return temp;
    }

    /// <summary>
    /// Collects creature size data & saves it to buffer
    /// </summary>
    /// <param name="attrList">List of creature attributes</param>
    /// <returns>List of creature sizes</returns>
    private List<float> GetSizes(List<CreatureAttributes> attrList) {
        List<float> temp = new List<float>();
        foreach (CreatureAttributes creature in attrList) {
            temp.Add(creature.size);
        }

        return temp;
    }

    /// <summary>
    /// Clears all files
    /// </summary>
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

    /// <summary>
    /// Saves buffer to csv fils.
    /// Also clears buffer
    /// </summary>
    public void SaveFiles() {
        if (timeBuf.Count > 0) {
            // Enter protected region
            fileMut.WaitOne();
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
            fileMut.ReleaseMutex();
        } 
    }

    /// <summary>
    /// Clears buffer
    /// </summary>
    private void ClearBuffers() {
        timeBuf.Clear();
        countBuf.Clear();
        velocityBuf.Clear();
        sightBuf.Clear();
        sizeBuf.Clear();
    }

    /// <summary>
    /// Writes a list of data to a StreamWriter
    /// </summary>
    /// <param name="list"></param>
    /// <param name="writer"></param>
    private void WriteListData(List<float> list, StreamWriter writer) {
        writer.WriteLine(string.Join(",", list));
    }
}
