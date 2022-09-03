using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Actions/AttackPlayer")]
public class ActionAttackPlayer : IAAction
{
    public override void Execute(IAController controller)
    {
        Attack(controller);
    }

    private void Attack(IAController controller)
    {
        if (controller.PlayerReference != null && controller.IsAttackTime() && controller.PlayerInRangeAttack(controller.AttackRangeDeterminated()))
        {
            if(controller.AttackType == AttackTypes.Melee)
            {
                controller.MeleeAttack();
            }else if (controller.AttackType == AttackTypes.Dash)
            {
                controller.DashAttack();
            }else if (controller.AttackType == AttackTypes.Projectile)
            {
                controller.ProjectileAttack();
            }
            else
            {
                controller.AreaAttack();
            }
           
            controller.UpdateTimeBetweenAttacks();
        }
    }

}
