using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<LevelConfig> levels;
    public int currentLevelIndex = 0;

    public LevelConfig CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        Instance = this;
        levels = LevelGenerator.GenerateLevels();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("You finished all levels!");
            currentLevelIndex = levels.Count - 1;
        }

        SceneManager.LoadScene("Main"); 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }
}
