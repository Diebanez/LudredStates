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

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Entered = true;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Updated++;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Exited = true;
    }
}
