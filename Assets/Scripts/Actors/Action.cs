using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void Move(Actor actor, Vector2 direction)
    {
        // Kijk of er iemand op de doelpositie staat
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // Als er niemand staat, kunnen we bewegen
        if (target == null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }

        // Beëindig de beurt als dit de speler is
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
