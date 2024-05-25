using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Powers
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;

    // Properties
    public int MaxHitPoints { get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }

    private void Start()
    {
        if (GetComponent<Player>() != null)
        {
            GameManager.Get.UIManager.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    public void DoDamage(int hp)
    {
        hitPoints -= hp;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        if (GetComponent<Player>() != null)
        {
            GameManager.Get.UIManager.UpdateHealth(hitPoints, maxHitPoints);
        }

        if (hitPoints == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GetComponent<Player>() != null)
        {
            GameManager.Get.UIManager.ShowMessage("You died!", Color.red);
        }
        else
        {
            string actorName = gameObject.name;
            GameManager.Get.UIManager.ShowMessage(actorName + " is dead!", Color.green);
            GameManager.Get.CreateActor("Remains of " + actorName, transform.position);
            GameManager.Get.RemoveEnemy(this);
        }

        Destroy(gameObject);
    }

    // Other methods...
}
