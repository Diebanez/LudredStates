using System;
using UnityEngine;

namespace Ludred.States
{
/// <summary>
/// Scriptable Object which define the structure of a finite state machine, made up from <see cref="StateBehaviour"/>
/// </summary>
[CreateAssetMenu(menuName = "Ludred/State Machine Profile")]
public class FiniteStateMachineProfile : ScriptableObject
{
    public string[] m_Triggers = new string[0];
    public State[] m_States = new State[0];
    public StateTransition[] m_Transitions = new StateTransition[0];

    public string[] Triggers => m_Triggers;
    public State[] States => m_States;
    public StateTransition[] Transitions => m_Transitions;

    public void AddTrigger(string triggerName)
    {
        if (!Array.Exists(m_Triggers, element => element == triggerName))
        {
            Array.Resize(ref m_Triggers, m_Triggers.Length + 1);
            m_Triggers[m_Triggers.Length - 1] = triggerName;
        }
    }

    public void RemoveTrigger(string triggerName)
    {
        if (Array.Exists(m_Triggers, element => element == triggerName))
        {
            var targetTransition = Array.FindAll(m_Transitions, transition => transition.TransitionTrigger == triggerName);
            foreach (var transition in targetTransition)
            {
                RemoveTransition(transition);
            }

            var triggerIndex = Array.IndexOf(m_Triggers, triggerName);
            for (int i = triggerIndex; i < m_Triggers.Length - 1; i++)
            {
                m_Triggers[i] = m_Triggers[i + 1];
            }

            Array.Resize(ref m_Triggers, m_Triggers.Length - 1);
        }
    }

    public bool HasTrigger(string triggerName)
    {
        if (Array.Exists(m_Triggers, element => element == triggerName))
            return true;
        return false;
    }

    public State AddState()
    {
         Array.Resize(ref m_States, m_States.Length + 1);
        m_States[m_States.Length -1 ] = new State();
        return m_States[m_States.Length - 1];
    }

    public void RemoveState(State state)
    {
        if (Array.Exists(m_States, element => element == state))
        {
            var stateIndex = Array.IndexOf(m_States, state);
            for (int i = stateIndex; i < m_States.Length - 1; i++)
            {
                m_States[i] = m_States[i + 1];
            }
            Array.Resize(ref m_States,m_States.Length - 1);
        }
    }

    public void AddTransition(State sourceState, State targetState, string transitionTrigger)
    {
        if (sourceState != null && targetState != null && Array.Exists(m_Triggers, element => element == transitionTrigger))
        {
            var newTransition = new StateTransition(sourceState, targetState, transitionTrigger);
            if (!Array.Exists(m_Transitions, element => element.Equals(newTransition)))
            {
                Array.Resize(ref m_Transitions, m_Transitions.Length + 1);
                m_Transitions[m_Transitions.Length - 1] = newTransition;
            }
        }
    }

    public void RemoveTransition(StateTransition transition)
    {
        if (Array.Exists(m_Transitions, element => element.Equals(transition)))
        {
            var transitionIndex = Array.IndexOf(m_Triggers, transition);
            for (int i = transitionIndex; i < m_Transitions.Length - 1; i++)
            {
                m_Transitions[i] = m_Transitions[i + 1];
            }

            Array.Resize(ref m_Transitions, m_Transitions.Length - 1);
        }
    }
}
}