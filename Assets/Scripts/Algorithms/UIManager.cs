using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    public InventoryUI InventoryUI { get; private set; } // Reference to the InventoryUI
    private HealthBar healthBar;

    [SerializeField] private FloorInfo floorInfo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar component not found.");
        }

        InventoryUI = FindObjectOfType<InventoryUI>();
        if (InventoryUI == null)
        {
            Debug.LogError("InventoryUI component not found.");
        }

        if (floorInfo == null)
        {
            Debug.LogError("FloorInfo component not found.");
        }
    }

    public void UpdateHealth(int currentHP, int maxHP)
    {
        if (healthBar != null)
        {
            healthBar.SetValues(currentHP, maxHP);
        }
    }

    public void UpdateLevel(int level)
    {
        if (healthBar != null)
        {
            healthBar.SetLevel(level);
        }
    }

    public void UpdateXP(int xp)
    {
        if (healthBar != null)
        {
            healthBar.SetXP(xp);
        }
    }

    public void UpdateFloor(int floor)
    {
        if (floorInfo != null)
        {
            floorInfo.SetFloor(floor);
        }
    }

    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        if (floorInfo != null)
        {
            floorInfo.SetEnemiesLeft(enemiesLeft);
        }
    }

    public void ShowMessage(string message, Color color)
    {
        // Show message logic
    }
}
