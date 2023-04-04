using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    public LogicScript logic;
    private AudioSource audioSource;
    public AudioClip dingSound;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            BirdScript birdScript = collision.GetComponent<BirdScript>();
            if (birdScript != null && birdScript.birdIsAlive == true)
            {
                logic.addScore(1);
                audioSource.PlayOneShot(dingSound);

                // Notify the agent that it passed through a pipe
                AIBirdAgent aiBirdAgent = collision.GetComponent<AIBirdAgent>();
                if (aiBirdAgent != null)
                {
                    aiBirdAgent.PassedPipe();
                }
            }
        }
    }

}
