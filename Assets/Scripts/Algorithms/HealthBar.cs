using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found.");
            return;
        }

        root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("Root VisualElement not found.");
            return;
        }

        healthBar = root.Q<VisualElement>("Foreground");
        if (healthBar == null)
        {
            Debug.LogError("Foreground VisualElement not found.");
            return;
        }

        healthLabel = root.Q<Label>("HealthLabel");
        if (healthLabel == null)
        {
            Debug.LogError("HealthLabel not found.");
            return;
        }

        SetValues(30, 100);
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        if (healthBar == null || healthLabel == null)
        {
            Debug.LogError("HealthBar or HealthLabel is not initialized.");
            return;
        }

        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = new Length(percent, LengthUnit.Percent);
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
}
