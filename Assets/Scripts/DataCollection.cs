using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataCollection : MonoBehaviour
{
    public float pollT = 10f;
    public GameStats stats;

    public static bool isCollecting = false;

    private GameObject[] creatures;
    private float currentTime = 0;
    private int noCreatures = 0;
    private List<float> Velocities = new List<float>();
    private List<float> Sights = new List<float>();
    private List<float> Sizes = new List<float>();

    private string path = "D:/Documents/GitHub/UnityNaturalSelection/Data/";
    private string timePath = "time.csv";
    private string countPath = "counts.csv";
    private string velocityPath = "velocities.csv";
    private string sightPath = "sights.csv";
    private string sizePath = "sizes.csv";

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void StartCollection() {
        isCollecting = true;
        StartCoroutine(DataCollectLoop());
    }

    public void StopCollection() {
        isCollecting = false;
    }

    IEnumerator DataCollectLoop() {
        Debug.Log("Data collection started");

        using (StreamWriter countWriter = new StreamWriter(path + countPath))
        using (StreamWriter velocityWriter = new StreamWriter(path + velocityPath))
        using (StreamWriter sightWriter = new StreamWriter(path + sightPath))
        using (StreamWriter sizeWriter = new StreamWriter(path + sizePath))
        using (StreamWriter timeWriter = new StreamWriter(path + timePath)) {
            while (isCollecting) {
                while (TimeManager.isPaused) {
                    Debug.Log("Data loop paused");
                    yield return null;
                }

                GetData();

                timeWriter.WriteLine(currentTime.ToString());
                countWriter.WriteLine(noCreatures.ToString());

                WriteListData(Velocities, velocityWriter);
                WriteListData(Sights, sightWriter);
                WriteListData(Sizes, sizeWriter);


                Debug.Log("Data point collected");
                yield return new WaitForSecondsRealtime(pollT);
            }
        }

        Debug.Log("Data collection complete");
    }

    private void GetData() {
        creatures = stats.Creatures;

        Velocities.Clear();
        Sights.Clear();
        Sizes.Clear();

        currentTime = Time.time;
        noCreatures = creatures.Length;

        foreach (GameObject creature in creatures) {
            CreatureAttributes creatureAttr = creature.GetComponent<CreatureAttributes>();
            Velocities.Add(creatureAttr.Velocity);
            Sights.Add(creatureAttr.SightRadius);
            Sizes.Add(creatureAttr.Size);
        }
    }

    private void WriteListData(List<float> list, StreamWriter writer) {
        writer.WriteLine(string.Join(",", list));
    }
}
