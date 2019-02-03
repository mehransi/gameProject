
using UnityEngine;
using System.Collections;

public class CoinPool : MonoBehaviour
{
    public GameObject coinPrefab;                                 //The column game object.
    public int coinPoolSize = 5;                                  //How many columns to keep on standby.
    public float spawnRate = 2f;                                    //How quickly columns spawn.
    public float coinMin = 136f;                                   //Minimum y value of the column position.
    public float coinMax = 140f;                                  //Maximum y value of the column position.

    public static GameObject[] coins;                                   //Collection of pooled columns.
    private int currentCoin = 0;                                  //Index of the current column in the collection.
    private Vector2 objectPoolPosition = new Vector2(-15, -25);     //A holding position for our unused columns offscreen.
    private float spawnXPosition = 7f;

    private float timeSinceLastSpawned;


    void Start()
    {
        timeSinceLastSpawned = 0f;

        //Initialize the columns collection.
        coins = new GameObject[coinPoolSize];
        //Loop through the collection...
        for (int i = 0; i < coinPoolSize; i++)
        {
            //...and create the individual columns.
            coins[i] = (GameObject)Instantiate(coinPrefab, objectPoolPosition, Quaternion.identity);
        }
    }


    //This spawns columns as long as the game is not over.
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (GameController.instance.gameOver == false && timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0f;

            //Set a random y position for the column
            float spawnYPosition = Random.Range(coinMin, coinMax);

            //...then set the current column to that position.
            coins[currentCoin].transform.position = new Vector2(spawnXPosition, spawnYPosition);

            //Increase the value of currentColumn. If the new size is too big, set it back to zero
            currentCoin++;

            if (currentCoin >= coinPoolSize)
            {
                currentCoin = 0;
            }
        }
    }
}
