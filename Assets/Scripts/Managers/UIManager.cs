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

    private bool colorToBlack;
    private Color blackColor;
    private Color greenColor;
    private float time;
    private void Start()
    {
        time = 0;
        ColorUtility.TryParseHtmlString("#1B1D1C", out blackColor);
        ColorUtility.TryParseHtmlString("#22652F", out greenColor);

    }

    void Update()
    {
        time += Time.deltaTime;
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
        //Color Stamina
        if (colorToBlack)
        {
            backgroundStamina.color = Color.Lerp(greenColor, blackColor, time*5);
        }
        else
        {
            backgroundStamina.color = Color.Lerp(Color.black, greenColor, time*5);
        }
        
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
        time = 0;
        colorToBlack = true;
        
        //backgroundStamina.color = color;
    }

    public void ResetColorStamina()
    {
        time = 0;
        colorToBlack = false;
        //backgroundStamina.color = greenColor;
    }
}
