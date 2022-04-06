using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Panels")]
    [SerializeField] private GameObject panelStats;

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

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageStatTMP;
    [SerializeField] private TextMeshProUGUI defenseStatTMP;
    [SerializeField] private TextMeshProUGUI speedStatTMP;
    [SerializeField] private TextMeshProUGUI CriticPercent;
    [SerializeField] private TextMeshProUGUI CriticBlock;

    private float currentHealth;
    private float maxHealth;

    private float currentMana;
    private float maxMana;

    private float currentStamina;
    private float maxStamina;

    private bool colorToBlack;
    private Color blackColor;
    private Color greenColor;
    private float timeChangeColorStamina;
    private void Start()
    {
        timeChangeColorStamina = 0;
        ColorUtility.TryParseHtmlString("#1B1D1C", out blackColor);
        ColorUtility.TryParseHtmlString("#22652F", out greenColor);

    }

    void Update()
    {
        timeChangeColorStamina += Time.deltaTime;
        UpdatePlayerUI();
        UpdateStatsPanel();
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
            backgroundStamina.color = Color.Lerp(greenColor, blackColor, timeChangeColorStamina * 5);
        }
        else
        {
            backgroundStamina.color = Color.Lerp(Color.black, greenColor, timeChangeColorStamina * 5);
        }
        
    }

    private void UpdateStatsPanel()
    {
        if (!panelStats.activeSelf)
        {
            return;
        }

        damageStatTMP.text = stats.Damage.ToString();
        defenseStatTMP.text = stats.Defense.ToString();
        speedStatTMP.text = stats.Speed.ToString();
        CriticPercent.text = $"{stats.CriticPercent}%";
        CriticBlock.text = $"{stats.CriticBlock}%";
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
        timeChangeColorStamina = 0;
        colorToBlack = true;
        
        //backgroundStamina.color = color;
    }

    public void ResetColorStamina()
    {
        timeChangeColorStamina = 0;
        colorToBlack = false;
        //backgroundStamina.color = greenColor;
    }
}
