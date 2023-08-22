using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    // Particle system components.
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    //GameManager gameManager;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // If we want to add more "trick" keys, we can add them in this method.
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C)) 
        {
            // If it's true it will turn it false, and if it is false it will turn it true.
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled)
        {
            return;
        }
        
        switch (other.gameObject.tag) 
        {
            case "Friendly":
                Debug.Log("You're ready to launch!");
                break;
            case "Finish":
                Debug.Log("Yes! You've succeded!");
                StartSuccessSequence();
                break;
            default:
                Debug.Log("Oh no! You've crashed!");
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        FindObjectOfType<GameManager>().PlayerScore();
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
        FindObjectOfType<GameManager>().UpdateLives();
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        FindObjectOfType<GameManager>().RestartTimer();
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            FindObjectOfType<GameManager>().ShowFinalScore();
        }
        else 
        {
            SceneManager.LoadScene(nextSceneIndex);
            FindObjectOfType<GameManager>().RestartTimer();
        }
    }
}
