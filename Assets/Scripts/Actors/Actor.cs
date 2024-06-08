using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public List<Vector3Int> FieldOfView { get; set; } = new List<Vector3Int>();

    // Powers
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1;
    [SerializeField] private int xp = 0;
    [SerializeField] private int xpToNextLevel = 100;

    // Properties
    public int MaxHitPoints { get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }
    public int Level { get { return level; } }
    public int XP { get { return xp; } }
    public int XPToNextLevel { get { return xpToNextLevel; } }

    private void Start()
    {
        if (GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateXP(xp);
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    public void UpdateFieldOfView()
    {
        // Implement the logic for updating the field of view here
    }

    public void DoDamage(int damage, Actor attacker)
    {
        hitPoints -= damage;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);

        if (hitPoints == 0)
        {
            Die(attacker);
        }
    }

    private void Die(Actor attacker)
    {
        if (attacker.GetComponent<Player>() != null)
        {
            attacker.AddXP(xp);
        }
        // Implement logic for when the actor dies
    }

    public void Heal(int hp)
    {
        int oldHitPoints = hitPoints;
        hitPoints += hp;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        if (GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.ShowMessage($"You have been healed for {hitPoints - oldHitPoints} HP.", Color.green);
        }
    }

    public void AddXP(int xp)
    {
        this.xp += xp;
        while (this.xp >= xpToNextLevel)
        {
            this.xp -= xpToNextLevel;
            LevelUp();
        }

        if (GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateXP(this.xp);
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        maxHitPoints += 10; // Increase max hit points
        defense += 2; // Increase defense
        power += 2; // Increase power

        if (GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.ShowMessage($"You have leveled up to level {level}!", Color.yellow);
        }
    }
}
