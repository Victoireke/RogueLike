using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8]; // Array for 8 labels
    private VisualElement root; // Root element
    private int selected; // Selected index
    private int numItems; // Number of items
    private List<Consumable> items; // List of items

    public int Selected
    {
        get { return selected; }
    }

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement; // Assuming you have a UIDocument component
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"Item{i + 1}");
        }
        Clear();
        root.style.display = DisplayStyle.None;
    }

    public void Clear()
    {
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    private void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = new StyleColor(Color.green); // Selected label
            }
            else
            {
                labels[i].style.backgroundColor = new StyleColor(Color.clear); // Non-selected labels
            }
        }
    }

    public void SelectNextItem()
    {
        if (selected < numItems - 1)
        {
            selected++;
            UpdateSelected();
        }
    }

    public void SelectPreviousItem()
    {
        if (selected > 0)
        {
            selected--;
            UpdateSelected();
        }
    }

    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = list.Count;
        items = list;
        Clear();

        for (int i = 0; i < numItems && i < labels.Length; i++)
        {
            labels[i].text = list[i].name;
        }

        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }

    public Consumable GetSelectedItem()
    {
        if (items == null || items.Count == 0)
        {
            return null;
        }

        return items[selected];
    }
}
