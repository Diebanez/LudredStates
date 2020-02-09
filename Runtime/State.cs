using System;
using UnityEngine;

namespace Ludred.States
{
/// <summary>
/// Class which define a single state of a FiniteStateMachine
/// </summary>
[Serializable]
public class State
{
    #region Private Fields
    /// <summary>
    /// The list of <see cref="StateBehaviour"/> attached to this state
    /// </summary>
    public StateBehaviour[] m_Behaviours;
    #endregion

    #region Public Fields
    /// <summary>
    /// The position in the editor window of this state
    /// </summary>
    public Vector2 StateEditorPosition;

    /// <summary>
    /// The name the state
    /// </summary>
    public string Name;
    #endregion

    #region Properties
    /// <summary>
    /// The list of <see cref="StateBehaviour"/> attached to this state
    /// </summary>
    public StateBehaviour[] Behaviours => m_Behaviours;
    #endregion

    #region Constructors
    /// <summary>
    /// Create a new state
    /// </summary>
    public State()
    {
        Name = "New State";
        m_Behaviours = new StateBehaviour[]{new TestBehaviour()};
    }
    #endregion


    #region Public Methods
    /// <summary>
    /// Add a new <see cref="StateBehaviour"/> to this State
    /// </summary>
    /// <param name="behaviour">The <see cref="StateBehaviour"/> to add</param>
    public void AddBehaviour(StateBehaviour behaviour)
    {
        if(!Array.Exists(m_Behaviours, element => element == behaviour))
        {
            Array.Resize(ref m_Behaviours, m_Behaviours.Length + 1);
            m_Behaviours[m_Behaviours.Length - 1] = behaviour;
        }
    }

    /// <summary>
    /// Add a <see cref="StateBehaviour"/> from this State
    /// </summary>
    /// <param name="behaviour">The <see cref="StateBehaviour"/> to remove</param>
    public void RemoveBehaviour(StateBehaviour behaviour)
    {
        if (Array.Exists(m_Behaviours, element => element == behaviour))
        {
            var index = Array.IndexOf(m_Behaviours, behaviour);
            for (int i = index; i < m_Behaviours.Length - 1; i++)
            {
                m_Behaviours[i] = m_Behaviours[i + 1];
            }
            Array.Resize(ref m_Behaviours, m_Behaviours.Length - 1);
        }
    }
    #endregion
}
}