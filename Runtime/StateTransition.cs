using System;

namespace Ludred.States
{
/// <summary>
/// Class which define a transition between two states of a finite state machine
/// </summary>
[Serializable]
public class StateTransition : IEquatable<StateTransition>
{
    #region Private Fields
    private string m_TransitionTrigger;
    private State m_SourceState;
    private State m_TargetState;
    #endregion

    #region Properties
    public string TransitionTrigger => m_TransitionTrigger;
    public State SourceState => m_SourceState;
    public State TargetState => m_TargetState;
    #endregion

    #region Contructors
    public StateTransition(State sourceState, State targetState, string transitionTrigger)
    {
        m_SourceState = sourceState;
        m_TargetState = targetState;
        m_TransitionTrigger = transitionTrigger;
    }
    #endregion

    public bool Equals(StateTransition other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return m_TransitionTrigger == other.m_TransitionTrigger && Equals(m_SourceState, other.m_SourceState) && Equals(m_TargetState, other.m_TargetState);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StateTransition) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (m_TransitionTrigger != null ? m_TransitionTrigger.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (m_SourceState != null ? m_SourceState.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (m_TargetState != null ? m_TargetState.GetHashCode() : 0);
            return hashCode;
        }
    }
}
}