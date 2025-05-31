using UnityEngine;
using UnityEngine.UI;

public class SwitchTargetButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameManager.Instance.OnSwitchTargetBtnClick();
        });
    }
}
