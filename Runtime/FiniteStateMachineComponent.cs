using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ludred.States
{
/// <summary>
/// Component which run a specified <see cref="FiniteStateMachineProfile"/> on the attached object
/// </summary>
public class FiniteStateMachineComponent : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineProfile m_Profile;

    private Stack<int> m_PushDownStack;
    private int m_ActualState;

    private FiniteStateMachineProfile m_ActiveProfile;
    private Dictionary<int, List<int>> m_ExitTransitions;

    public FiniteStateMachineProfile Profile => m_Profile;

    private void Start()
    {
        if (m_Profile != null)
        {
            m_ActiveProfile = Instantiate(m_Profile);
            Initialize();
        }
    }

    private void Update()
    {
        UpdateState();
    }
    
    private void Initialize()
    {
        if (m_ActiveProfile != null)
        {
            m_PushDownStack = new Stack<int>();
            m_ExitTransitions = new Dictionary<int, List<int>>();
            for (var i = 0; i < m_ActiveProfile.Transitions.Length; i++)
            {
                var transition = m_ActiveProfile.Transitions[i];

                var sourceStateIndex = Array.IndexOf(m_ActiveProfile.m_States, transition.SourceState);

                if (!m_ExitTransitions.ContainsKey(sourceStateIndex))
                    m_ExitTransitions.Add(sourceStateIndex, new List<int>());
                m_ExitTransitions[sourceStateIndex].Add(i);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying)
        {
            if (enabled && gameObject.activeInHierarchy)
            {
                m_ActiveProfile.States[m_ActualState].DrawGizmos(this);
            }
        }
    }

    private void EnterState(int targetState)
    {
        m_ActiveProfile.States[targetState].EnterState(this);
        m_PushDownStack.Push(targetState);
        m_ActualState = targetState;
    }

    private void UpdateState()
    {
        m_ActiveProfile.States[m_ActualState].UpdateState(this);
    }

    private void ExitState()
    {
        m_ActiveProfile.States[m_ActualState].ExitState(this);
    }
    
    public void Trigger(string triggerName)
    {
        if (m_ActiveProfile.HasTrigger(triggerName))
        {
            if (m_ExitTransitions.ContainsKey(m_ActualState))
            {
                foreach (var transitionIndex in m_ExitTransitions[m_ActualState])
                {
                    if (m_ActiveProfile.Transitions[transitionIndex].TransitionTrigger == triggerName)
                    {
                        ExitState();
                        EnterState(Array.IndexOf(m_ActiveProfile.States, m_ActiveProfile.Transitions[transitionIndex].TargetState));
                        return;
                    }
                }
            }
        }
    }

    public void SetActiveProfile(FiniteStateMachineProfile profile)
    {
        m_ActiveProfile = profile;
        Initialize();
    }
}
}