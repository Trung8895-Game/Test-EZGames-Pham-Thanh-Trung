using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFill;
    

    public void Setup()
    {
        healthFill.fillAmount = 1;
        //healthSlider.value = maxHealth;
    }

    public void SetHealth(float value, float maxHealth)
    {
        healthFill.fillAmount = value/maxHealth;
    }

    
}
