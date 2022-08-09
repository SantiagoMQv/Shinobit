using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private IAState initialState;
    [SerializeField] private IAState DefaultState;
    public IAState CurrentState { get; set; }

    private void Start()
    {
        CurrentState = initialState;
    }

    private void Update()
    {
        CurrentState.ExecuteState(this);
    }

    public void ChangeState(IAState newState)
    {
        if(newState != DefaultState)
        {
            CurrentState = newState;
        }
    }

}
