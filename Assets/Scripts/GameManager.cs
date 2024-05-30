using Items;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Actor Player { get; set; }
    public List<Actor> Enemies { get; private set; } = new List<Actor>();
    private UIManager uiManager;

    // Lijst voor Consumable items
    private List<Consumable> items = new List<Consumable>();

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

        // Initialize UIManager reference
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene!");
        }
    }

    // Voeg een Consumable item toe aan de lijst
    public void AddItem(Consumable item)
    {
        items.Add(item);
    }

    // Verwijder een Consumable item uit de lijst
    public void RemoveItem(Consumable item)
    {
        items.Remove(item);
    }

    // Haal een Consumable item op basis van locatie
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (var item in items)
        {
            if (item.transform.position == location)
            {
                return item;
            }
        }
        return null;
    }

    public static GameManager Get { get => instance; }

    // Bestaande functies...
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        foreach (var enemy in Enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        return null;
    }

    public GameObject CreateActor(string actorType, Vector2 position)
    {
        GameObject actorPrefab = Resources.Load<GameObject>(actorType);
        if (actorPrefab != null)
        {
            return Instantiate(actorPrefab, position, Quaternion.identity);
        }

        return null;
    }

    public void StartEnemyTurn()
    {
        foreach (var enemyActor in Enemies)
        {
            var enemyComponent = enemyActor.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
        }
    }

    public void RemoveEnemy(Actor enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject); // Optionally, depending on your implementation
    }

    public UIManager GetUIManager()
    {
        return uiManager;
    }
}
