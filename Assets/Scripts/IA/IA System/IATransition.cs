using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Controla la transición de un estado a otro
[Serializable]
public class IATransition
{
    public IADecision Decision;
    public IAState TrueState;
    public IAState FalseState;
}
