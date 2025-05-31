using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Characters")]
    public List<BaseCharacter> allCharacters;
    public CameraController cameraController;
    public UIManager uiManager;
    private int currentEnemyTargetIndex;

    public static GameManager Instance = null;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1.2f;
        StartCoroutine(StartMatchSequence());
        SetupMatch(LevelManager.Instance.CurrentLevel);
    }

    private IEnumerator StartMatchSequence()
    {
        yield return new WaitForSeconds(1f);

        foreach (var character in allCharacters)
        {
            StartCoroutine(character.WalkToPosition());
        }

        yield return new WaitForSeconds(6f);

        var player = allCharacters.OfType<PlayerController>().FirstOrDefault();
        if (player != null)
        {
            cameraController.EndIntroAndFollow(player.transform);
        }

        yield return new WaitForSeconds(1f);

        uiManager.ShowCountdown(() =>
        {
            MatchManager.Instance.StartMatch();
        });
    }

    public void SetupMatch(LevelConfig config)
    {
        MatchManager.Instance.SpawnPlayer(config.playerHealth, config.playerAttackRate);
        MatchManager.Instance.SpawnSubPlayers(config.numberOfPlayers, config.enemyHealth, config.enemyAttackRate, config.aiAggressiveness);
        MatchManager.Instance.SpawnEnemies(config.numberOfEnemies, config.enemyHealth, config.enemyAttackRate, config.aiAggressiveness);
        MatchManager.Instance.setTargetforPlayer();
        MatchManager.Instance.setTargetforSubPlayers();
        MatchManager.Instance.setTargetforEnemies();
        
        Debug.Log(config.numberOfPlayers);

    }

    public void ReportKnockout(BaseCharacter target)
    {
        if (gameEnded) return;

        
        allCharacters.Remove(target);

        if (IsAllEnemiesDown())
        {
            gameEnded = true;
            EndGame(true);
        }
        else if (IsAllPlayersDown())
        {
            gameEnded = true;
            EndGame(false);
        }
    }

    private void EndGame(bool playerWin)
    {
        
        MatchManager.Instance.CanAttack = false;

        if (playerWin)
        {
            UIManager.Instance.ShowVictory();
            GameEvents.OnGameEnd?.Invoke();
            Invoke("LoadNextLevel", 3f);
        }
        else
        {
            UIManager.Instance.ShowDefeat();
            GameEvents.OnGameEnd?.Invoke();
            Invoke("RestartLevel", 3f);
        }

        
    }

    public bool IsAllEnemiesDown()
    {
        return MatchManager.Instance.allEnemies.All(e => !e.gameObject.activeSelf);
    }

    public bool IsAllPlayersDown()
    {
        return !MatchManager.Instance.player.gameObject.activeSelf;
    }

    private void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
        
    }
    private void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();

    }
    public void OnSwitchTargetBtnClick()
    {

        MatchManager.Instance.allEnemies[currentEnemyTargetIndex].fightTarget = MatchManager.Instance.enemyFightPos[currentEnemyTargetIndex];
        currentEnemyTargetIndex +=1;
        if(currentEnemyTargetIndex>= MatchManager.Instance.allEnemies.Count)
        {
            currentEnemyTargetIndex = 0;
        }
        if(currentEnemyTargetIndex==0)
        {
            MatchManager.Instance.player.fightTarget = MatchManager.Instance.playerFightPos[currentEnemyTargetIndex];
            
        }
        else
        {
            MatchManager.Instance.player.fightTarget = MatchManager.Instance.allEnemies[currentEnemyTargetIndex].transform;
            
        }
        MatchManager.Instance.player.target = MatchManager.Instance.allEnemies[currentEnemyTargetIndex];
        MatchManager.Instance.allEnemies[currentEnemyTargetIndex].fightTarget = MatchManager.Instance.player.transform;
        

    }
}
