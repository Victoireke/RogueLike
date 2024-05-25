using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        // Kijk of er iemand op de doelpositie staat
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // Als er niemand staat, kunnen we bewegen
        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
    }

    static public void Move(Actor actor, Vector2 direction)
    {
        // Verplaats de actor in de opgegeven richting
        actor.Move(direction);
        // Update het zichtveld van de actor
        actor.UpdateFieldOfView();
        // Beëindig de beurt
        EndTurn(actor);
    }

    static public void Hit(Actor actor, Actor target)
    {
        // Bereken de schade
        int damage = actor.Power - target.Defense;
        // Als de schade positief is, verminder dan de hitpoints van het doelwit
        if (damage > 0)
        {
            target.DoDamage(damage);
        }

        // Bericht samenstellen
        string message = actor.gameObject.name + " hits ";
        if (damage > 0)
        {
            message += target.gameObject.name + " for " + damage + " damage.";
        }
        else
        {
            message += target.gameObject.name + " but does no damage.";
        }

        // Kleur van het bericht instellen
        Color color = (actor.GetComponent<Player>() != null) ? Color.white : Color.red;

        // Bericht weergeven via UIManager
        UIManager.Instance.ShowMessage(message, color);

        // Beëindig de beurt
        EndTurn(actor);
    }

    static private void EndTurn(Actor actor)
    {
        // Controleer of de actor een spelercomponent heeft
        Player playerComponent = actor.GetComponent<Player>();

        // Als dat zo is, voer dan de StartEnemyTurn-functie van GameManager uit
        if (playerComponent != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
}
