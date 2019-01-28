using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;         //A reference to our game control script so we can access it statically.
    public Text scoreText;                      //A reference to the UI text component that displays the player's score.
    public GameObject gameOvertext;             //A reference to the object that displays the text which appears when the player dies.
    public GameObject SnoopButton;
    public GameObject PanizButton;
    public GameObject DambizButton;
    public GameObject ChangizButton;
    public bool snoop = false;
    public bool dambiz = false;
    public bool paniz = false;
    public bool changiz = false;
    public float scrollSpeed = -1.5f;
    private int score = 0;                      //The player's score.
    public bool gameOver = false;               //Is the game over?


    // void Start ()
    // {
    //   SnoopButton.onClick.AddListener(TaskOnClick);
    // }
    // void TaskOnClick()
    // {
    // //Output this to console when Button1 or Button3 is clicked
    // Debug.Log("You have clicked the button!");
    // }

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if(instance != this)
            //...destroy this one because it is a duplicate.
            Destroy (gameObject);
    }

    void Update()
    {
        //SnoopButton = SnoopButton.GetComponent<Button>();
        //If the game is over and the player has pressed some input...
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            // if(SnoopButton.OnCLick)
            //...reload the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void BirdScored()
    {
        //The bird can't score if the game is over.
        if (gameOver)
            return;
        //If the game is not over, increase the score...
        score++;
        //...and adjust the score text.
        scoreText.text = "Score: " + score.ToString();
    }

    public void BirdDied()
    {
        //Activate the game over text.
        gameOvertext.SetActive (true);
        SnoopButton.SetActive(true);
        ChangizButton.SetActive(true);
        PanizButton.SetActive(true);
        DambizButton.SetActive(true);
        //Set the game to be over.
        gameOver = true;
    }

}
