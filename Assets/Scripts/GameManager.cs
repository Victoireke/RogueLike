using Items; // Ensure this is included
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Items.Consumable;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Actor Player { get; set; }
    public List<Actor> Enemies { get; private set; } = new List<Actor>();
    private UIManager uiManager;
    public List<Ladder> Ladders { get; private set; } = new List<Ladder>(); // List of ladders

    // Lijst voor Consumable items
    private List<Consumable> items = new List<Consumable>();

    // Lijst voor TombStones
    private List<TombStone> tombStones = new List<TombStone>(); // Add this line


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
        items.Remove(item); // Change `remove` to `Remove`
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
        uiManager.UpdateEnemiesLeft(Enemies.Count); // Update enemies left in UI
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
        uiManager.UpdateEnemiesLeft(Enemies.Count); // Update enemies left in UI
    }

    public UIManager GetUIManager()
    {
        return uiManager;
    }

    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();
        foreach (Actor enemy in Enemies)
        {
            if (Vector3.Distance(location, enemy.transform.position) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }
        return nearbyEnemies;
    }

    // Add a ladder to the GameManager
    public void AddLadder(Ladder ladder)
    {
        Ladders.Add(ladder);
    }

    // Get a ladder at a specific location
    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach (var ladder in Ladders)
        {
            if (ladder.transform.position == location)
            {
                return ladder;
            }
        }
        return null;
    }

    // Add a TombStone to the GameManager
    public void AddTombStone(TombStone stone)
    {
        tombStones.Add(stone);
    }

    // Clear all entities and items on the current floor
    public void ClearFloor()
    {
        // Clear enemies
        foreach (var enemy in Enemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemies.Clear();
        uiManager.UpdateEnemiesLeft(Enemies.Count); // Update enemies left in UI

        // Clear items
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();

        // Clear ladders
        foreach (var ladder in Ladders)
        {
            Destroy(ladder.gameObject);
        }
        Ladders.Clear();

        // Clear tombstones
        foreach (var tombStone in tombStones)
        {
            Destroy(tombStone.gameObject);
        }
        tombStones.Clear();
    }
}
