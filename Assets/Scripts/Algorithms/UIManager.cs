using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject healthBarObject;
    public GameObject messagesObject;

    private HealthBar healthBar;
    private Messages messages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        healthBar = healthBarObject.GetComponent<HealthBar>();
        messages = messagesObject.GetComponent<Messages>();
    }

    public void UpdateHealth(int current, int max)
    {
        healthBar.SetValues(current, max);
    }

    public void AddMessage(string message, Color color)
    {
        messages.AddMessage(message, color);
    }
}
