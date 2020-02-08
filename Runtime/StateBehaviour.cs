using System;

namespace Ludred.States
{
/// <summary>
/// Abstract class which define the single state of a <see cref="FiniteStateMachineProfile"/>
/// </summary>
[Serializable]
public abstract class StateBehaviour
{
    /// <summary>
    /// Called when the <see cref="FiniteStateMachineProfile"/> transition to this state
    /// </summary>
    public virtual void OnStateEnter(){}
    /// <summary>
    /// Called on the update of the Finite State Machine, if the actual state has this behaviour
    /// </summary>
    public virtual void OnStateUpdate(){}
    /// <summary>
    /// Called when the <see cref="FiniteStateMachineProfile"/> transition from this state
    /// </summary>
    public virtual void OnStateExit(){}
}
}