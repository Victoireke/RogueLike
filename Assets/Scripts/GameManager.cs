using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Actor Player { get; set; } // Ensure this property is available
    public List<Actor> Enemies { get; private set; } = new List<Actor>();

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

    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public static GameManager Get { get => instance; }

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
}
