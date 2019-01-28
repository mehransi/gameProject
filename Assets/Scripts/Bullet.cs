using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	int BulletLifeTime = 1;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = new Vector2(10, 10);
	}

	// Update is called once per frame
	void Update () {
		BulletLifeTime --;
		rb2d.velocity = new Vector2(10, 5);
		if (BulletLifeTime == 0)
		{
			Destroy(gameObject);
		}

	}
}
