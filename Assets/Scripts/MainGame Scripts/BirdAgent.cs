using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BirdAgent : Agent
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    private Vector3 birdStartPosition = new Vector3(-21.0f, 0.0f, 0.0f);
    private bool isFirstRun = true;
    private bool flap = false;

    public override void Initialize()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the bird's position and velocity
        myRigidBody.velocity = Vector2.zero;
        logic.playerScore = 0;
        if (isFirstRun)
        { 
            // Find all the pipe gaps in the scene
            GameObject[] pipeGaps = GameObject.FindGameObjectsWithTag("PipeGap");

            // Find the pipe gap closest to the bird's current X position
            GameObject closestPipeGap = null;
            float closestX = float.MaxValue;

            // Set the bird's position to the initial start position on the first run
            foreach (GameObject gap in pipeGaps)
            {
                float distance = Mathf.Abs(gap.transform.position.x - transform.position.x);
                if (distance < closestX)
                {
                    closestX = distance;
                    closestPipeGap = gap;
                }
                if (closestPipeGap != null)
                {
                    // Calculate the middle position of the gap
                    float middleY = closestPipeGap.transform.position.y;

                    transform.position = new Vector3(-20.17f, middleY, 0f);
                }
                else
                {
                    transform.position = birdStartPosition;
                }
            }
            //transform.position = birdStartPosition;
            isFirstRun = false;
        }
        else
        {
            // Find all the pipe gaps in the scene
            GameObject[] pipeGaps = GameObject.FindGameObjectsWithTag("PipeGap");

            // Find the pipe gap closest to the bird's current X position
            GameObject closestPipeGap = null;
            float closestX = float.MaxValue;

            foreach (GameObject gap in pipeGaps)
            {
                float distance = Mathf.Abs(gap.transform.position.x - transform.position.x);
                if (distance < closestX)
                {
                    closestX = distance;
                    closestPipeGap = gap;
                }
            }

            // Set the bird's position to be in the middle of the closest pipe gap with X position set to -20.17
            if (closestPipeGap != null)
            {
                // Calculate the middle position of the gap
                float middleY = closestPipeGap.transform.position.y;

                transform.position = new Vector3(-20.17f, middleY, 0f);
            }
            else
            {
                transform.position = new Vector3(-20.17f, birdStartPosition.y, birdStartPosition.z);
            }
        }
    }

    private void FixedUpdate()
    {
        AddReward(0.05f);
        if (flap)
        {
            myRigidBody.velocity = Vector2.up * flapStrength;
            flap = false;
        }
    }

    private void Update()
    {
        if (IsBirdOutOfScreen())
        {
            logic.gameOver();
            AddReward(-1f);
            EndEpisode();
            ClickPlayAgainButton();
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the bird's vertical position and vertical velocity as observations
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(myRigidBody.velocity.y);
    }

    private int lastInput;
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = lastInput == 0?(Input.GetKey(KeyCode.Space) ? 1 : 0):0;
        lastInput = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            flap = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        AddReward(-3f);
        EndEpisode();
        ClickPlayAgainButton();
        Debug.Log("ded " + collision.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        logic.addScore(1);
        AddReward(1f);
    }

    private void ClickPlayAgainButton()
    {
        GameObject playAgainButtonObj = GameObject.FindGameObjectWithTag("PlayAgainButton");
        if (playAgainButtonObj != null)
        {
            Button playAgainButton = playAgainButtonObj.GetComponent<Button>();
            if (playAgainButton != null)
            {
                playAgainButton.onClick.Invoke();
            }
        }
    }

    private bool IsBirdOutOfScreen()
    {
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            Vector2 birdSize = GetComponent<SpriteRenderer>().bounds.size;

            // Check if the bird is out of the screen on the y axis (up or down)
            if (transform.position.y > screenBounds.y + birdSize.y / 2 ||
                transform.position.y < -screenBounds.y - birdSize.y / 2)
            {
                return true;
            }

            // Check if the bird is out of the screen on the x axis (left)
            if (transform.position.x < -screenBounds.x - birdSize.x / 2)
            {
                return true;
            }

            return false;
        }

    }
