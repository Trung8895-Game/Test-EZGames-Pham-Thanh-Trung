using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public bool CanAttack { get; set; } = false;
    public List<BaseCharacter> allEnemies = new List<BaseCharacter>();
    public List<BaseCharacter> allSubPlayers = new List<BaseCharacter>();
    private int currentTargetIndex = 0;

    public GameObject enemyPrefab; 
    public Transform[] enemySpawnPoints; 
    public Transform[] enemyFightPos;

    public GameObject playerPrefab;
    public GameObject subPlayerPrefab;
    public Transform[] playerSpawnPoints; 
    public Transform[] playerFightPos;
    public PlayerController player;
    public static MatchManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartMatch()
    {
        CanAttack = true;
    }

    public BaseCharacter GetCurrentTarget(PlayerController player)
    {
        if (allEnemies.Count == 0) return null;

        int validCount = 0;
        foreach (var enemy in allEnemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
                validCount++;
        }

        if (validCount == 0) return null;

        return allEnemies[currentTargetIndex % allEnemies.Count];
    }

    public void SwitchTarget()
    {
        if (allEnemies.Count > 1)
        {
            currentTargetIndex++;
        }
    }

    public void SpawnEnemies(int numberOfEnemies, float enemyHealth, float attackRate, float aiAggressiveness)
    {
        allEnemies.Clear();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Length];
            GameObject go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyAIController enemy = go.GetComponent<EnemyAIController>();
            enemy.fightPosition = enemyFightPos[i];
            enemy.fightTarget = enemy.fightPosition.transform;
            //character.target = allPlayers[i];

            enemy.Init(characterType: CharacterType.Enemy, isAI: true);
            enemy.SetStats(enemyHealth, attackRate);
            enemy.SetAIAggressiveness(aiAggressiveness);
            allEnemies.Add(enemy);
            GameManager.Instance.allCharacters.Add(enemy);
        }
    }
    public void SpawnPlayer(float playerHealth, float attackRate)
    {
        
       
            Transform spawnPoint = playerSpawnPoints[0];
            GameObject go;
            go = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            player = go.GetComponent<PlayerController>();
            player.Init(characterType: CharacterType.Player, isAI: false);

            player.fightPosition = playerFightPos[0];
            player.fightTarget = player.fightPosition.transform;
            //character.target = allEnemies[i];


            player.SetStats(playerHealth, attackRate);

           
            GameManager.Instance.allCharacters.Add(player);

            
        
    }
    public void SpawnSubPlayers(int numberOfPlayers, float playerHealth, float attackRate, float aiAggressiveness)
    {
        allSubPlayers.Clear();
        for (int i = 0; i < numberOfPlayers-1; i++)
        {
            Transform spawnPoint = playerSpawnPoints[(i+1) % playerSpawnPoints.Length];
            GameObject go;
            go = Instantiate(subPlayerPrefab, spawnPoint.position, Quaternion.identity);
            EnemyAIController subPlayer = go.GetComponent<EnemyAIController>();
            subPlayer.Init(characterType: CharacterType.SubPlayer, isAI: true);

            subPlayer.fightPosition = playerFightPos[i+1];
            subPlayer.fightTarget = subPlayer.fightPosition.transform;
            //character.target = allEnemies[i];


            subPlayer.SetStats(playerHealth, attackRate);
            subPlayer.SetAIAggressiveness(aiAggressiveness);

            allSubPlayers.Add(subPlayer);
            GameManager.Instance.allCharacters.Add(subPlayer);
        }
    }

    public void setTargetforEnemies()
    {
        allEnemies[0].target = player;
        for (int i = 1; i < allEnemies.Count; i++)
        {
            if(i>=allSubPlayers.Count)
            {
                allEnemies[i].target = player;
                return;
            }
            allEnemies[i].target= allSubPlayers[i];
        }
    }
    public void setTargetforSubPlayers()
    {
        for (int i = 0; i < allSubPlayers.Count; i++)
        {
            if (i >= allEnemies.Count-1)
            {
                allSubPlayers[i].target = allEnemies[0];
                return;
            }
            allSubPlayers[i].target = allEnemies[i+1];
        }
    }
    public void setTargetforPlayer()
    {
        player.target = allEnemies[0];
    }
}
