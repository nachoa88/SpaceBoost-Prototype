using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    // To be able to stop the game, we need to determinate if the game is active or not.
    public bool isGameActive;
    // To be able to pause the game, we need to determinate if the game is paused or not.
    public bool isGamePaused;
    
    // We need to be able to reference the gameOverScreen & pauseScreen to turn them on and off. 
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject youWonScreen;
    public Image timerImage;

    [SerializeField] int lives = 3;
    [SerializeField] float timeToCompleteLevel = 30f;

    private float fillFraction;
    public float timerValue;
    private int timeRemainingAtSuccess;
    private int score = 0;



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

    void Start()
    {
        // This must go first because if not the Coroutine won't know that the game is active and won't start.
        isGameActive = true;
        // It will turn the integer into a string, so that it gets written in the TMP.
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: " + score;

        // Set the timer Value to the value that we have to complete the level.
        timerValue = timeToCompleteLevel;

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePaused();
        }

        UpdateTimer();
    }

    /* This method will change the paused boolean when it is called. When the boolean is changed to true, it enables the pauseScreen and 
       sets the Time.timeScale to 0. Setting the Time.timeScale to 0 makes the physics calculations to be paused, the normal value is 1. */
    void ChangePaused()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isGamePaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }

        // Missing a way to make the sound of thrust not being played while paused.
    }

    void UpdateTimer()
    {
        // We set equals for the image and the fillFraction calculated in Timer's Script.
        timerImage.fillAmount = fillFraction;
        // We reduce timerValue per every frame (timerValue = timerValuer - Time.deltaTime).
        timerValue -= Time.deltaTime;

        if (timerValue > 0)
        {
            // So the fraction of the fill of the timer image will be equal to the timerValue / timeToCompleteQuestion.
            fillFraction = timerValue / timeToCompleteLevel;
        }
        // Debug.Log(timerValue);
    }

    public void RestartTimer()
    {
        // Restart timerValue to original value set at Start().
        timerValue = timeToCompleteLevel;
    }


    public void UpdateLives()
    {
        if (lives > 1)
        {
            TakeLife();
        }
        else
        {
            GameOver();
        }
    }

    void TakeLife()
    {
        // We substract one life.
        lives--;
        livesText.text = "Lives: " + lives.ToString();
    }

    public void GameOver()
    {
        livesText.text = "Lives: 0";
        gameOverScreen.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        // Destroy the current GameManager object so everything is reseted to the Game Manager prefab.
        Destroy(gameObject); 
        /* You can get the scene to reload by naming it or just calling the Active scene at the moment.
           The rest of the information of this method is used in the inspector of the button, it has an On Click () method.*/
        SceneManager.LoadScene(0);
        gameOverScreen.SetActive(false);
        youWonScreen.SetActive(false);
    }
    
    public void PlayerScore()
    {
        // First we round up the time left in timerValue into an Integer.
        timeRemainingAtSuccess = Mathf.RoundToInt(timerValue);
        Debug.Log("Your score at the end of the level is of: " + timeRemainingAtSuccess);
        // Then we update the score in our scoreText.
        UpdateScore(timeRemainingAtSuccess);
    }

    public void UpdateScore(int scoreToAdd)
    {
        // Update the score before the text.
        score += scoreToAdd;
        // Update text. Add to the textmeshpro the words we need + the variable we created for the score.
        scoreText.text = "Score: " + score;
    }

    
    public void ShowFinalScore()
    {
        youWonScreen.SetActive(true);
        finalScoreText.text = "Congratulations,\nyour score is of " + score + "!";

        // Compare and update Best score.
        CompareAndUpdateBestScore();
    }

    public void CompareAndUpdateBestScore()
    {
        if (score > MenuManager.Instance.bestScore)
        {
            MenuManager.Instance.bestScore = score;
            MenuManager.Instance.playerWithBestScore = MenuManager.Instance.playerName;
            MenuManager.Instance.SavePlayerScore();
        }
    }
}
