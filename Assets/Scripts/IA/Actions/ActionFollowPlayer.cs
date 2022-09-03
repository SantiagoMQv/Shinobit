using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Actions/FollowPlayer")]
public class ActionFollowPlayer : IAAction
{
    public override void Execute(IAController controller)
    {
        FollowPlayer(controller);
    }

    private void FollowPlayer(IAController controller)
    {
        if(controller.PlayerReference != null)
        {
            Vector3 DirToPlayer = controller.PlayerReference.position - controller.transform.position;
            Vector3 direction = DirToPlayer.normalized;
            float distance = DirToPlayer.magnitude;
            if(distance >= 1.3f)
            {
                controller.transform.Translate(direction * controller.SpeedMovement * Time.deltaTime);
            }
        }
    }

}
