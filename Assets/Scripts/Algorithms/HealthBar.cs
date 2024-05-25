using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;

    private void Start()
    {
        // Get the UIDocument component and root VisualElement
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Assign healthBar and healthLabel
        healthBar = root.Q<VisualElement>("Foreground");
        healthLabel = root.Q<Label>("HealthLabel");

        // Initialize with some values (optional)
        SetValues(30, 100);
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = new Length(percent, LengthUnit.Percent);
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
}
