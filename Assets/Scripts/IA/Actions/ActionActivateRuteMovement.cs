using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Actions/ActivateRuteMovement")]
public class ActionActivateRuteMovement : IAAction
{
    public override void Execute(IAController controller)
    {
        if(controller.EnemyMovement != null)
        {
            controller.EnemyMovement.enabled = true;
        }
    }
}
