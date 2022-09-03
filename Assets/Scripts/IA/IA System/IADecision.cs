using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IADecision : ScriptableObject
{
    public abstract bool Decide(IAController controller);
}
