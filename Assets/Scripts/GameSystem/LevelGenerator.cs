using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    OneVsOne,
    OneVsTwo,
    TwoVsTwo
}

public static class LevelGenerator
{
    public static List<LevelConfig> GenerateLevels()
    {
        List<LevelConfig> levels = new();

        for (int i = 1; i <= 10; i++)
        {
            LevelConfig level = ScriptableObject.CreateInstance<LevelConfig>();
            level.name = $"Level {i}";

            level.gameMode = i switch
            {
                <= 3 => GameMode.OneVsOne,
                <= 6 => GameMode.OneVsTwo,
                _ => GameMode.TwoVsTwo
            };

            level.numberOfEnemies = i <= 3 ? 1 : 2;
            level.enemyHealth = 300 + (i * 100);
            level.enemyAttackRate = Mathf.Max(1.5f - (i * 0.1f), 0.5f);
            level.aiAggressiveness = Mathf.Min(1f + (i * 0.07f), 2f);

            level.numberOfPlayers = i <= 6 ? 1 : 2;
            level.playerHealth = 500;
            level.playerAttackRate = Mathf.Max(1.5f - (i * 0.1f), 0.5f); ;

            levels.Add(level);
        }

        return levels;
    }
}
