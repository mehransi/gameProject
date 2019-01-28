using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour {
	float fireBulletRate = 1f;
	public GameObject BulletPrefab;
	float timeSinceLastShot = 0f;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		timeSinceLastShot += Time.deltaTime;
		if (fireBulletRate < timeSinceLastShot){
				timeSinceLastShot = 0;
				ShootBullet();
			}
}
void ShootBullet(){

	GameObject Bullet = Instantiate(BulletPrefab);
	Bullet.transform.position = new Vector2(gameObject.transform.position[0]+0.60f,gameObject.transform.position[1]+0.35f);

	// Debug.LogError(gameObject.transform.position[0]+gameObject.transform.position[1]);
}
}
