using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Panels")]
    [SerializeField] private GameObject panelStats;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject HealthTokenPanel;

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

    [Header("HealthToken Config")]
    [SerializeField] private GameObject HealthToken;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageStatTMP;
    [SerializeField] private TextMeshProUGUI defenseStatTMP;
    [SerializeField] private TextMeshProUGUI PotionTMP;
    [SerializeField] private TextMeshProUGUI HealthPointsTMP;
    [SerializeField] private TextMeshProUGUI SpellPointsTMP;
    [SerializeField] private TextMeshProUGUI StaminaPointsTMP;

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

    private Transform HealthTokenContainer;
    private void Start()
    {
        timeChangeColorStamina = 0;
        ColorUtility.TryParseHtmlString("#1B1D1C", out blackColor);
        ColorUtility.TryParseHtmlString("#22652F", out greenColor);
        HealthTokenContainer = HealthTokenPanel.transform.GetChild(0);
    }

    void Update()
    {
        timeChangeColorStamina += Time.deltaTime;
        UpdatePlayerUI();
        UpdateStatsPanel();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenCloseMenu();
        }
        if(Inventary.Instance.specialItems.HealingNinjutsu == true && Inventary.Instance.healingNinjutsuItem != null)
        {
            GenerateHealthTokenPanel();
        }
        else
        {
            HideHealthTokenPanel();
        }
    }

    #region PlayerInfo
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
        PotionTMP.text = stats.Potion.ToString();
        HealthPointsTMP.text = stats.HealthPoints.ToString();
        SpellPointsTMP.text = stats.SpellPoints.ToString();
        StaminaPointsTMP.text = stats.StaminaPoints.ToString();
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
    #endregion

    #region HealthToken

    public int RemoveHealthTokenUI()
    {
        Destroy(HealthTokenContainer.GetChild(0).gameObject);
        return HealthTokenContainer.childCount;

    }

    public int AddHealthTokenUI()
    {
        Instantiate(HealthToken, HealthTokenContainer);
        return HealthTokenContainer.childCount;
    }

    #endregion

    #region Panels

    public void OpenCloseMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void GenerateHealthTokenPanel()
    {
        HealthTokenPanel.SetActive(true);
    }

    public void HideHealthTokenPanel()
    {
        HealthTokenPanel.SetActive(false);
    }
    #endregion
}
