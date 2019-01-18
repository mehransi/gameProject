using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour {

    public AudioClip aud;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bird>() != null)
        {
            //If the bird hits the trigger collider in between the columns then
            //tell the game control that the bird scored.
            GameController.instance.BirdScored();
            AudioSource.PlayClipAtPoint(aud, transform.position);
            this.transform.position = Vector2.zero;
        }
    }
	
}
