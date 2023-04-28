using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.UI;

public class BirdAgent : Agent
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    private Vector3 birdStartPosition = new Vector3(-21.0f, 0.0f, 0.0f);

    public override void Initialize()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the bird's position and velocity
        transform.position = birdStartPosition;
        myRigidBody.velocity = Vector2.zero;
        logic.playerScore = 0;
    }

    private void FixedUpdate()
    {
        AddReward(0.05f);
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
            myRigidBody.velocity = Vector2.up * flapStrength;
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
        float birdWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        if (transform.position.y > screenBounds.y + birdWidth / 2 ||
            transform.position.y < -screenBounds.y - birdWidth / 2)
        {
            return true;
        }

        return false;
    }


}
