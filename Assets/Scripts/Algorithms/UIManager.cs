using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

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

    public void UpdateHealth(int currentHP, int maxHP)
    {
        // Update health UI logic
    }

    public void ShowMessage(string message, Color color)
    {
        // Show message logic
    }
}
