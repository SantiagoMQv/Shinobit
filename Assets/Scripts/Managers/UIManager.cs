using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{

    [Header("Health Config")]
    [SerializeField] private Image playerHealth;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [Header("Mana Config")]
    [SerializeField] private Image playerMana;
    [SerializeField] private TextMeshProUGUI manaTMP;
    [Header("Stamina Config")]
    [SerializeField] private Image playerStamina;
    [SerializeField] private TextMeshProUGUI staminaTMP;
    [SerializeField] private Image backgroundStamina;

    private float currentHealth;
    private float maxHealth;

    private float currentMana;
    private float maxMana;

    private float currentStamina;
    private float maxStamina;

    void Update()
    {
        UpdatePlayerUI();
    }

    private void UpdatePlayerUI()
    {
        //Health
        playerHealth.fillAmount = Mathf.Lerp(playerHealth.fillAmount, currentHealth / maxHealth, 10 * Time.deltaTime);
        healthTMP.text = $"{currentHealth}/{maxHealth}";
        //Mana
        playerMana.fillAmount = Mathf.Lerp(playerMana.fillAmount, currentMana / maxMana, 10 * Time.deltaTime);
        manaTMP.text = $"{currentMana}/{maxMana}";
        //Stamina
        playerStamina.fillAmount = Mathf.Lerp(playerStamina.fillAmount, currentStamina / maxStamina, 10 * Time.deltaTime);
        staminaTMP.text = $"{currentStamina}/{maxStamina}";
    }

    public void UpdatePlayerHealth(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    public void UpdatePlayerMana(float currentMana, float maxMana)
    {
        this.currentMana = currentMana;
        this.maxMana = maxMana;
    }

    public void UpdatePlayerStamina(float currentStamina, float maxStamina)
    {
        this.currentStamina = currentStamina;
        this.maxStamina = maxStamina;
    }

    public void UpdateColorStaminaToBlack()
    {
        Color color;
        ColorUtility.TryParseHtmlString("#1B1D1C", out color);
        backgroundStamina.color = color;
    }

    public void ResetColorStamina()
    {
        Color greenColor;
        ColorUtility.TryParseHtmlString("#22652F", out greenColor);
        backgroundStamina.color = greenColor;
    }
}
