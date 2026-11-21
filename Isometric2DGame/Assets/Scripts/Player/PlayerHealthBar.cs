using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text healthBarText;
    public void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateUI;
    }
    public void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateUI;
    }
    
    public void UpdateUI(int maxHealth, int newHealth)
    {

        float healthPerc = newHealth / (float)maxHealth;
        healthBarFill.fillAmount = (healthPerc);
        healthBarText.text = newHealth.ToString() + " / " +maxHealth;
    }
}
