using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{

    public TMP_InputField playerNameInput;
    public TextMeshProUGUI bestScoreText;


    private void Awake()
    {
        if (MenuManager.Instance != null)
        {
            bestScoreText.text = $" {MenuManager.Instance.playerWithBestScore} {MenuManager.Instance.bestScore}";
        }
    }

    public void OnStartClick()
    {
        MenuManager.Instance.playerName = playerNameInput.text;
        SceneManager.LoadScene(1);
    }


    public void OnQuitClick()
    {
        MenuManager.Instance.SavePlayerScore();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}