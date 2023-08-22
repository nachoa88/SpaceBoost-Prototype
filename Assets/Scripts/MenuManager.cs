using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    public static MenuManager Instance;
    public int bestScore = 0;
    public string playerName;
    public string playerWithBestScore;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadPlayerScore();
    }


    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string playerWithBestScore;
    }

    public void SavePlayerScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.playerWithBestScore = playerWithBestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }


    public void LoadPlayerScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            playerWithBestScore = data.playerWithBestScore;
        }
    }

    public void ResetHighScore()
    {
        // Set values to zero and none.
        bestScore = 0;
        playerWithBestScore = "Player Name";
        // Run the Save Player Score method to actually save them.
        SavePlayerScore();
        // Update the UI to display the reset high score
        bestScoreText.text = $" {playerWithBestScore} {bestScore}";
    }
}
