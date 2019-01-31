using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class BirdAgent : Agent
{
    public float upForce = 180;                   //Upward force of the "flap".
    private bool isDead = false;            //Has the player collided with a wall?
    private bool jump = false;
    float reward;
    private Animator anim;                  //Reference to the Animator component.
    private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.
    public float fireRate = 0;
    public float Damage = 10;
    // public LayerMask notToHit;
    public GameObject ShitbirdPrefab;

    float timeToFire = 0;




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
        AddVectorObs(player_y - pipePos.y - 5.65f);
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

    public Vector3 GetNextPipe()
    {
        float leftMost = float.MaxValue;
        GameObject leftChild = null;
        Vector3 t = Vector3.zero;
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
    public Vector3 GetNextCoin()
    {
        float leftMost = float.MaxValue;
        GameObject leftChild = null;
        Vector3 t = Vector3.zero;
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
        if (isDead)
        {
            reward -= 1f;
        }
       
        if (GameController.instance.scored == 1) {
            reward += .1f;
            GameController.instance.scored = 0;
        }
        if (GameController.instance.scored == 2)
        {
            reward += .05f;
            GameController.instance.scored = 0;
        }
        AddReward(reward);
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
        //...and tell the game control about it.
        //GameController.instance.BirdDied();
        
      }
    }

    public override void AgentReset()
    {
        gameObject.transform.position = new Vector2(0,140);
        gameObject.transform.rotation = Quaternion.identity;

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

