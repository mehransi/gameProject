using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    public float upForce = 180;                   //Upward force of the "flap".
    private bool isDead = false;            //Has the player collided with a wall?
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

    void Update()
    {
        //Don't allow control if the bird has died.
        if (isDead == false)
        {
            //Look for input to trigger a "flap".
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W))
            {
                //...tell the animator about it and then...
                anim.SetTrigger("Flap");
                //...zero out the birds current y velocity before...
                rb2d.velocity = Vector2.zero;
                //  new Vector2(rb2d.velocity.x, 0);
                //..giving the bird some upward force.
                rb2d.AddForce(new Vector2(0, upForce));
            }

            if (fireRate == 0){
              if (Input.GetMouseButtonDown(1)){
                Shoot();
              }
            }
            // else {
            //   if (Input.GetMouseButtonDown(1) && Time.time > timeToFire) {
            //     timeToFire = Time.time + 1/fireRate;
            //     Shoot();
            //   }
            // }

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
        rb2d.velocity = Vector2.zero;
        // If the bird collides with something set it to dead...
        isDead = true;
        //...tell the Animator about it...
        anim.SetTrigger ("Die");
        //...and tell the game control about it.
        GameController.instance.BirdDied ();
      }
    }
}
