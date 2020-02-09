using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludred.States
{
/// <summary>
/// Component which run a specified <see cref="FiniteStateMachineProfile"/> on the attached object
/// </summary>
public class FiniteStateMachineComponent : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineProfile m_Profile;

    public FiniteStateMachineProfile Profile => m_Profile;
}
}