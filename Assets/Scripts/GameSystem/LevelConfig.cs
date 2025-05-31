using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    public GameMode gameMode;
    public int numberOfEnemies;
    public int enemyHealth;
    public float enemyAttackRate;
    public float aiAggressiveness; 
    public int numberOfPlayers;
    public int playerHealth;
    public float playerAttackRate;
}
