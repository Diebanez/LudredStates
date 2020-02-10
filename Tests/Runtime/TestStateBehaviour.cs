using System.Collections;
using System.Collections.Generic;
using Ludred.States;
using NUnit.Framework.Constraints;
using UnityEngine;

public class TestStateBehaviour : StateBehaviour
{
    public bool  Entered;
    public int Updated;
    public bool Exited;

    public TestStateBehaviour()
    {
        Entered = false;
        Updated = 0;
        Exited = false;
    }

    public override void OnStateEnter(FiniteStateMachineComponent component, State state)
    {
        base.OnStateEnter(component, state);
        Entered = true;
    }

    public override void OnStateUpdate(FiniteStateMachineComponent component, State state)
    {
        base.OnStateUpdate(component, state);
        Updated++;
    }

    public override void OnStateExit(FiniteStateMachineComponent component, State state)
    {
        base.OnStateExit(component, state);
        Exited = true;
    }
}
