using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Decisions/DetectPlayer")]
public class DecisionDetectPlayer : IADecision
{
    private bool DetectPlayer(IAController controller)
    {
        // Crea un Collider que al detectar al personaje, guarda su referencia
        Collider2D playerDetected = Physics2D.OverlapCircle(controller.transform.position, controller.DetectionRange, controller.PlayerLayerMask);
        if (playerDetected != null)
        {
            controller.PlayerReference = playerDetected.transform;
            return true;
        }
        controller.PlayerReference = null;
        return false;
    }
    public override bool Decide(IAController controller)
    {
        return DetectPlayer(controller);
    }
}
