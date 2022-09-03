using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/State")]
public class IAState : ScriptableObject
{
    public IAAction[] Actions;
    public IATransition[] Transitions;

    public void ExecuteState(IAController controller)
    {
        ExecuteActions(controller);
        DoTransitions(controller);
    }

    private void ExecuteActions(IAController controller)
    {
        if(Actions != null || Actions.Length > 0)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].Execute(controller);
            }
        }
    }

    private void DoTransitions(IAController controller)
    {
        if (Transitions != null || Transitions.Length > 0)
        {
            for (int i = 0; i < Transitions.Length; i++)
            {
                bool decisionValue = Transitions[i].Decision.Decide(controller);
                if (decisionValue)
                {
                    controller.ChangeState(Transitions[i].TrueState);
                }
                else
                {
                    controller.ChangeState(Transitions[i].FalseState);
                }
            }
        }
    }
}
