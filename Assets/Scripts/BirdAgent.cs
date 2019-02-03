using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class BirdAgent : Agent
{
    public float upForce = 180;                   //Upward force of the "flap".
    private bool isDead = false;            //Has the player collided with a wall?
    private bool jump = false;
    private float rew;
    private Animator anim;                  //Reference to the Animator component.
    private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.
    // public float Damage = 10;
    // public LayerMask notToHit;
    public GameObject ShitbirdPrefab;




    void Start()
    {
        //Get reference to the Animator component attached to this GameObject.
        anim = GetComponent<Animator> ();
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        rb2d = GetComponent<Rigidbody2D>();
    }

    public override void CollectObservations()
    {
        float player_y = gameObject.transform.position.y;
        AddVectorObs(rb2d.velocity.y);
        Vector3 pipePos = GetNextPipe();
        Vector3 coinPos = GetNextCoin();
        float toCoin = Mathf.FloorToInt(player_y-coinPos.y) + .5f;
        AddVectorObs((toCoin));
        AddVectorObs(Mathf.FloorToInt(coinPos.x));
        AddVectorObs(player_y - pipePos.y - 5.85f);
        AddVectorObs(Mathf.FloorToInt(pipePos.x));
    }

    private void Push()
    {
        anim.SetTrigger("Flap");
        //...zero out the birds current y velocity before...
        rb2d.velocity = Vector2.zero;
        //  new Vector2(rb2d.velocity.x, 0);
        //..giving the bird some upward force.
        rb2d.AddForce(new Vector2(0, upForce));
    }

    public Vector2 GetNextPipe()
    {
        float leftMost = float.MaxValue;
        GameObject leftChild = null;
        Vector2 t = new Vector2(10,134);
        foreach (GameObject child in ColumnPool.columns)
        {
            if (child.transform.localPosition.x < leftMost &&
                child.transform.localPosition.x > -.3f)
            {
                leftChild = child;
                leftMost = child.transform.localPosition.x;
            }
        }
        if(leftChild != null)
            t = leftChild.transform.localPosition;
        return t;
    }
    public Vector2 GetNextCoin()
    {
        float leftMost = float.MaxValue;
        GameObject leftChild = null;
        Vector2 t = new Vector2(10,134);
        foreach (GameObject child in CoinPool.coins)
        {
            if (child.transform.localPosition.x < leftMost &&
                child.transform.localPosition.x > -.3f)
            {
                leftChild = child;
                leftMost = child.transform.localPosition.x;
            }
        }
        if (leftChild != null)
            t = leftChild.transform.localPosition;
        return t;
    } 
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Monitor.Log("actions", vectorAction);
        int tap = Mathf.FloorToInt(vectorAction[0]);
        if (tap == 0)
        {
            jump = false;
        }
        if (tap == 1 && !jump)
        {
            jump = true;
            Push();

        }
		rew = .001f;
        if (isDead)
        {
            rew -= 1f;
        }
       
        if (GameController.instance.scored == 1) {
            rew += .01f;
            GameController.instance.scored = 0;
        }
        if (GameController.instance.scored == 2)
        {
            rew += .002f;
            GameController.instance.scored = 0;
        }
        AddReward(rew);
        if (isDead) { 
            Done();
        }
        
    }


   
    void Shoot(){

      GameObject birdshit = Instantiate(ShitbirdPrefab);
      birdshit.transform.position = new Vector2(gameObject.transform.position[0],gameObject.transform.position[1]-0.25f);

      // Debug.LogError(gameObject.transform.position[0]+gameObject.transform.position[1]);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.tag != "birdShit") {
        // Zero out the bird's velocity
        //rb2d.velocity = Vector2.zero;
        // If the bird collides with something set it to dead...
        isDead = true;
        //...tell the Animator about it...
        //anim.SetTrigger ("Die");
        //GameController.instance.startAgain = true;
        //...and tell the game control about it
        //GameController.instance.BirdDied();
        
      }
    }

    public override void AgentReset()
    {
        int birdY = Random.Range(136, 140);
        gameObject.transform.position = new Vector2(0,birdY);

        rb2d.velocity = Vector2.zero;
        GameController.instance.score = 0;
        GameController.instance.scoreText.text = "Score: " + GameController.instance.score.ToString();

        for (int i = 0; i < 5; i++)
        {
            ColumnPool.columns[i].transform.position = Vector2.zero;
            CoinPool.coins[i].transform.position = Vector2.zero;
        }
        isDead = false;
    }
}

