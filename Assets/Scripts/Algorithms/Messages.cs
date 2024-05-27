using UnityEngine;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;

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

        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"Label{i + 1}");
            if (labels[i] == null)
            {
                Debug.LogError($"Label{i + 1} not found.");
                return;
            }
        }

        Clear();
        AddMessage("Welcome to the dungeon, Adventurer!", Color.green);
    }

    public void Clear()
    {
        foreach (var label in labels)
        {
            if (label != null)
            {
                label.text = string.Empty;
                label.style.color = Color.black;
            }
        }
    }

    public void MoveUp()
    {
        for (int i = labels.Length - 1; i > 0; i--)
        {
            if (labels[i] != null && labels[i - 1] != null)
            {
                labels[i].text = labels[i - 1].text;
                labels[i].style.color = labels[i - 1].style.color;
            }
        }

        if (labels[0] != null)
        {
            labels[0].text = string.Empty;
            labels[0].style.color = Color.black;
        }
    }

    public void AddMessage(string content, Color color)
    {
        MoveUp();
        if (labels[0] != null)
        {
            labels[0].text = content;
            labels[0].style.color = color;
        }
    }
}
