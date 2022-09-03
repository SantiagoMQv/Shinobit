using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Decisions/PlayerInRangeAttack")]
public class DecisionPlayerInRangeAttack : IADecision
{
    public override bool Decide(IAController controller)
    {
        return InAttackRange(controller);
    }

    private bool InAttackRange(IAController controller)
    {
        if(controller.PlayerReference != null)
        {
            float distance = (controller.PlayerReference.position - controller.transform.position).sqrMagnitude;
            if(distance < Mathf.Pow(controller.AttackRangeDeterminated(), 2))
            {
                return true;
            }
        }
        return false;
    }
}
