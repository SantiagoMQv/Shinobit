using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAAction : ScriptableObject
{
    public abstract void Execute(IAController controller);
}
