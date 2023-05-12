using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

public class PipeSpawnerAgent : Agent
{
    public PipeSpawnScript pipeSpawner;
    public BirdAgent birdAgent;
    public LogicScript logic;
    public float maxPipeSpeed;
    public float minPipeSpeed;

    public override void Initialize()
    {
        // You can initialize your references here, similar to your BirdAgent
        // but if your references are set up in the Unity editor, you don't need to do anything here.
    }

    public override void OnEpisodeBegin()
    {
        // Reset the pipe spawner settings here
        //pipeSpawner.spawnRate = minPipeSpeed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the bird's score and the current pipe spawn settings
        sensor.AddObservation(logic.playerScore);
        sensor.AddObservation(pipeSpawner.spawnRate);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Use the actions to adjust the pipe spawner settings
        var continuousActions = actions.ContinuousActions;
        pipeSpawner.spawnRate = Mathf.Lerp(minPipeSpeed, maxPipeSpeed, continuousActions[0]);
        //pipeSpawner.heightOffset = Mathf.Lerp(minPipeGap, maxPipeGap, continuousActions[1]);

        // Add rewards based on the game state
        if (logic.DidPassPipe)
        {
            AddReward(1.0f);
            logic.DidPassPipe = false;
        }

        if (logic.DidHitPipe)
        {
            AddReward(-1.0f);
            logic.DidHitPipe = false;
            EndEpisode();
        }

        if (logic.DidMissPipe)
        {
            AddReward(-1.0f);
            logic.DidMissPipe = false;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // If you want to manually control the pipe spawner for testing, you can do so here
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow Pressed");
            continuousActionsOut[0] = 1; // Increase spawn rate
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            continuousActionsOut[0] = 0; // Decrease spawn rate
        }
        else
        {
            continuousActionsOut[0] = 0.5f; // Keep spawn rate the same
        }
    }
}
