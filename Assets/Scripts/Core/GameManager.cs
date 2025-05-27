using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Characters")]
    public List<BaseCharacter> allCharacters;
    public CameraController cameraController;
    public UIManager uiManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartMatchSequence());
    }

    private IEnumerator StartMatchSequence()
    {
        yield return new WaitForSeconds(1f);

        foreach (var character in allCharacters)
        {
            StartCoroutine(character.WalkToPosition());
        }

        yield return new WaitForSeconds(3f); 

        
        var player = allCharacters.OfType<PlayerController>().FirstOrDefault();
        cameraController.EndIntroAndFollow(player.transform);

        yield return new WaitForSeconds(1f);

        uiManager.ShowCountdown(() =>
        {
            MatchManager.Instance.StartMatch(); 
        });
    }
}
