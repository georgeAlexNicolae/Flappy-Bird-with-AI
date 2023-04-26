//using Unity.MLAgents;
//using Unity.MLAgents.Sensors;
//using Unity.MLAgents.Actuators;
//using UnityEngine;

//public class DifficultyAgent : Agent
//{
//    [SerializeField] private PipeSpawnScript pipeSpawner;
//    [SerializeField] private BirdScript birdScript;
//    [SerializeField] private LogicScript logicScript;
//    private int previousScore;

//    public override void OnEpisodeBegin()
//    {
//        previousScore = 0;
//    }

//    public override void CollectObservations(VectorSensor sensor)
//    {
//        sensor.AddObservation(logicScript.playerScore);
//        sensor.AddObservation(pipeSpawner.spawnRate);
//        sensor.AddObservation(pipeSpawner.heightOffset);
//    }

//    public override void OnActionReceived(ActionBuffers actions)
//    {
//        float newSpawnRate = actions.ContinuousActions[0];
//        float newHeightOffset = actions.ContinuousActions[1];

//        pipeSpawner.spawnRate = Mathf.Clamp(newSpawnRate, 1f, 5f);
//        pipeSpawner.heightOffset = Mathf.Clamp(newHeightOffset, 1f, 10f);

//        if (birdScript.birdIsAlive)
//        {
//            if (previousScore < logicScript.playerScore)
//            {
//                AddReward(1.0f);
//                previousScore = logicScript.playerScore;
//            }
//        }
//        else
//        {
//            AddReward(-1.0f);
//            EndEpisode();
//        }
//    }

//    public override void Heuristic(in ActionBuffers actionsOut)
//    {
//        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
//        continuousActions[0] = Input.GetAxis("Vertical");
//        continuousActions[1] = Input.GetAxis("Horizontal");
//    }
//}
