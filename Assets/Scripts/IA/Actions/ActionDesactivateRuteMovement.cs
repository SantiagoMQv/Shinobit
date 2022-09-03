using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Actions/DesactivateRuteMovement")]
public class ActionDesactivateRuteMovement : IAAction
{
    public override void Execute(IAController controller)
    {
        if (controller.EnemyMovement != null)
        {
            controller.EnemyMovement.enabled = false;
        }
    }
}
