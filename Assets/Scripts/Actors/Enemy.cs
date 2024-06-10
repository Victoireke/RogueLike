using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; private set; }
    public bool IsFighting { get; private set; } = false;
    private AStar algorithm;
    private int confused = 0;

    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        algorithm = GetComponent<AStar>();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        if (confused > 0)
        {
            confused--;
            UIManager.Instance.ShowMessage($"The {gameObject.name} is confused and cannot act.", Color.yellow);
            return;
        }

        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // First check if already fighting, because the FieldOfView check costs more CPU
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(targetGridPosition))
        {
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // Calculate distance between the enemy and the target
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            if (distance < 1.5f)
            {
                // If the distance is less than 1.5, execute the Hit function
                Action.Hit(GetComponent<Actor>(), Target);
            }
            else
            {
                // Otherwise, execute the MoveAlongPath function
                MoveAlongPath(targetGridPosition);
            }
        }
    }

    public void Confuse()
    {
        confused = 8;
    }
}
