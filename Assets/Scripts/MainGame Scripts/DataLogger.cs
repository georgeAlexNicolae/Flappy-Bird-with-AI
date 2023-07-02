using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    private string filePath;
    private StreamWriter writer;
    private string delimiter = ",";

    public LogicScript logicScript; // Add reference to LogicScript
    public BirdAgent birdAgent; // Add reference to BirdAgent
    public PipeSpawnerAgent pipeSpawnerAgent; // Add reference to PipeSpawnerAgent

    // Start is called before the first frame update
    void Start()
    {
        // Initialize your file path here. Change this to your preferred location.
        filePath = Application.persistentDataPath + "/GameData.csv";

        // Create a new StreamWriter and write the column names as the first line.
        writer = new StreamWriter(filePath, true);
        writer.WriteLine("Time" + delimiter + "Score" + delimiter + "BirdPosition" + delimiter + "PipeSpawnRate");

        Debug.Log("Data logging started at: " + filePath);

    }

    void Update()
    {
        // Check if the references are not null
        if (writer != null && logicScript != null && birdAgent != null && pipeSpawnerAgent != null && pipeSpawnerAgent.pipeSpawner != null)
        {
            // Record data every frame
            string time = Time.time.ToString();

            string score = logicScript.playerScore.ToString(); // Access playerScore from logicScript instance

            string birdPosition = birdAgent.transform.position.y.ToString(); // Access Y position from birdAgent instance

            string pipeSpawnRate = pipeSpawnerAgent.pipeSpawner.spawnRate.ToString(); // Access spawnRate from pipeSpawner instance

            // Write the data to the CSV file
            writer.WriteLine(time + delimiter + score + delimiter + birdPosition + delimiter + pipeSpawnRate);
        }
    }


    private void OnApplicationQuit()
    {
        // Make sure to close the writer when you quit the application
        writer.Close();
    }

    public void OnGameRestart()
    {
        // Close the writer before re-instantiating it
        if (writer != null)
        {
            writer.Close();
        }

        // Re-instantiate the writer
        writer = new StreamWriter(filePath, true);

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        birdAgent = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAgent>();
        pipeSpawnerAgent = GameObject.FindGameObjectWithTag("PipeSpawner").GetComponent<PipeSpawnerAgent>();
    }

}
