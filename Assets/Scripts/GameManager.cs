using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    // To be able to stop the game, we need to determinate if the game is active or over.
    public bool isGameActive;
    // We need to be able to reference the button & title screen to turn them on and off. 
    public Button restartButton;
    //public GameObject titleScreen;
    public GameObject pauseScreen;

    //private int score;
    [SerializeField] int lives = 3;
    private bool paused;


    void Awake()
    {
        // We search for the integer of the amount of Game Manager objects that we have.
        int numGameManager = FindObjectsOfType<GameManager>().Length;
        // So if there is already one Game Manager object, destroy the new one.
        if (numGameManager > 1)
        {
            Destroy(gameObject);
        }
        // If there is only one, don't destroy on load.
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePaused();
        }
    }

    // We pass the integer of the difficulty to modify the method.
    void Start()
    {
        // This must go first because if not the Coroutine won't know that the game is active and won't start.
        isGameActive = true;
        // It will turn the integer into a string, so that it gets written in the TMP.
        livesText.text = "Lives: " + lives.ToString();
        
        
        // Set score to 0
        //score = 0;
        //UpdateScore(0);
    }

    /* This method will change the paused boolean when it is called. When the boolean is changed to true, it
       enables the pauseScreen and sets the Time.timeScale to 0. Setting the Time.timeScale to 0 makes it
       so that physics calculations are paused. When the boolean is changed to false, it disables the
       pauseScreen and sets the Time.timeScale to 1.*/
    void ChangePaused()
    {
        if (!paused)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    
    /*
    public void UpdateScore(int scoreToAdd)
    {
        // Update the score before the text.
        score += scoreToAdd;
        // Update text. Add to the textmeshpro the words we need + the variable we created for the score.
        scoreText.text = "Score: " + score;
    }*/

    public void UpdateLives()
    {
        if (lives > 1)
        {
            TakeLife();
        }
        // If the player has at least one life, reset Game Session.
        else
        {
            GameOver();
        }
    }

    void TakeLife()
    {
        // First we substract one life.
        lives--;
        // Then we look for the number of scene that we are currently running.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Finally we reload the current scene.
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + lives.ToString();
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        /* You can get the scene to reload by naming it or just calling the Active scene at the moment.
           The rest of the information of this method is used in the inspector of the button, it has an On Click () method.*/
        SceneManager.LoadScene(0);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }
}
