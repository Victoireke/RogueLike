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

    // Properties
    public int MaxHitPoints { get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }

    private void Start()
    {
        if (GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    public void Move(Vector3 direction)
    {
        // Verplaats de actor in de opgegeven richting
        transform.position += direction;
    }

    public void UpdateFieldOfView()
    {
        // Implementeer de logica voor het bijwerken van het gezichtsveld hier
    }

    public void DoDamage(int damage)
    {
        // Verminder de hitpoints met de ontvangen schade
        hitPoints -= damage;
        // Zorg ervoor dat hitpoints niet onder nul gaat
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }
        // Update de health UI
        UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        // Als de hitpoints nul zijn, roep Die() aan
        if (hitPoints == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implementeer logica voor wanneer de actor sterft
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
}
