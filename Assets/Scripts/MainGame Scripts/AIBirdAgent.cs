using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIBirdAgent : Agent
{
    public BirdScript birdScript;
    public LogicScript logicScript;
    public float flapStrength = 5f;

    public override void Initialize()
    {
        birdScript = GetComponent<BirdScript>();
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public override void OnEpisodeBegin()
    {
        logicScript.restartGame();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (birdScript.birdIsAlive)
        {
            if (Mathf.FloorToInt(actions.DiscreteActions[0]) == 1)
            {
                birdScript.myRigidBody.velocity = Vector2.up * flapStrength;
            }
        }
        else
        {
            // Apply a negative reward for dying
            AddReward(-1.0f);
            EndEpisode();
        }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Bird's vertical position
        sensor.AddObservation(transform.localPosition.y);

        // Bird's vertical velocity
        sensor.AddObservation(birdScript.myRigidBody.velocity.y);
    }

    public void PassedPipe()
    {
        // Apply a positive reward for passing through a pipe
        AddReward(1.0f);
    }

    public void OnCollision()
    {
        // Apply a negative reward for collisions
        AddReward(-0.5f);
        EndEpisode();
    }
}
