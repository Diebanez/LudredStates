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
    public virtual void OnStateEnter(FiniteStateMachineComponent component, State state){}
    /// <summary>
    /// Called on the update of the Finite State Machine, if the actual state has this behaviour
    /// </summary>
    public virtual void OnStateUpdate(FiniteStateMachineComponent component, State state){}
    /// <summary>
    /// Called when the <see cref="FiniteStateMachineProfile"/> transition from this state
    /// </summary>
    public virtual void OnStateExit(FiniteStateMachineComponent component, State state){}
    public virtual void OnUpdateDrawGizmos(FiniteStateMachineComponent component, State state){}
}
}