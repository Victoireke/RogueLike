using UnityEngine;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"Label{i + 1}");
        }

        Clear();
        AddMessage("Welcome to the dungeon, Adventurer!", Color.green);
    }

    public void Clear()
    {
        foreach (var label in labels)
        {
            label.text = string.Empty;
            label.style.color = Color.black;
        }
    }

    public void MoveUp()
    {
        for (int i = labels.Length - 1; i > 0; i--)
        {
            labels[i].text = labels[i - 1].text;
            labels[i].style.color = labels[i - 1].style.color;
        }
        labels[0].text = string.Empty;
        labels[0].style.color = Color.black;
    }

    public void AddMessage(string content, Color color)
    {
        MoveUp();
        labels[0].text = content;
        labels[0].style.color = color;
    }
}
