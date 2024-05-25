using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("UIManager is missing in the scene!");
                }
            }
            return instance;
        }
    }

    // Reference naar de UI-elementen
    public Text healthText;
    public Text messageText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        // Update de health tekst
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + " / " + maxHealth;
        }
    }

    public void ShowMessage(string message, Color color)
    {
        // Toon een bericht
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
        }
    }
}
