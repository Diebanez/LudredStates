using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ludred.States.Editor
{
public class FiniteStateMachineWindow : EditorWindow
{
    private const float STATE_WIDTH = 100;
    private const float STATE_HEIGHT = 30;

    private FiniteStateMachineProfile m_SelectedProfile;
    private FiniteStateMachineComponent m_SelectedComponent;

    private Vector2 m_StatesViewOffset;
    private GUIStyle m_StateStyle;

    private string m_NewTrigger = "";

    private bool m_Moving = false;
    private State m_MovingState;

    private State m_SelectedState;

    private bool m_Dragging = false;

    private Rect m_TriggersInspectorRect;
    private Rect m_StatesViewRect;
    private Rect m_StateInspectorRect;

    [MenuItem("Ludred/State Machine Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<FiniteStateMachineWindow>();
        window.titleContent = new GUIContent("State Machine Window");
        window.minSize = new Vector2(800, 600);
        window.Show();
    }

    private void OnEnable()
    {
        var stateTexture = Resources.Load<Texture2D>("T_Node");
        m_StateStyle = new GUIStyle {normal = {background = stateTexture}, border = new RectOffset(16, 16, 16, 16), alignment = TextAnchor.MiddleCenter};
        UpdateSelection();
    }

    private void OnSelectionChange()
    {
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        m_SelectedProfile = null;
        m_SelectedComponent = null;
        m_SelectedState = null;

        if (Selection.objects.Length == 1)
        {
            if (Selection.objects[0] is FiniteStateMachineProfile)
            {
                m_SelectedProfile = (FiniteStateMachineProfile) Selection.objects[0];
            }
            else
            {
                if (Selection.gameObjects.Length == 1)
                {
                    var component = Selection.gameObjects[0].GetComponent<FiniteStateMachineComponent>();
                    if (component)
                    {
                        m_SelectedComponent = component;
                    }
                }
            }
        }

        Repaint();
    }

    private void OnGUI()
    {
        if (m_SelectedProfile != null || m_SelectedComponent != null)
        {
            var sideWidth = position.width * .3f > 300 ? 300 : position.width * .3f;
            var statesViewWidth = position.width - sideWidth * 2;

            m_TriggersInspectorRect = new Rect(0, 0, sideWidth, position.height);
            m_StatesViewRect = new Rect(sideWidth, 0, statesViewWidth, position.height);
            m_StateInspectorRect = new Rect(sideWidth + statesViewWidth, 0, sideWidth, position.height);

            ObtainInputs();

            UpdateInputs();

            DrawStatesView(m_StatesViewRect);
            DrawTriggersInspector(m_TriggersInspectorRect);
            DrawStateInspector(m_StateInspectorRect);
        }
    }

    private void DrawTriggersInspector(Rect rect)
    {
        EditorGUI.DrawRect(rect, new Color(56f / 255f, 56f / 255f, 56f / 255f));

        var offset = new Rect(rect);
        offset.height = EditorGUIUtility.singleLineHeight;
        var targetProfile = m_SelectedProfile == null ? m_SelectedComponent.Profile : m_SelectedProfile;
        foreach (var trigger in targetProfile.Triggers)
        {
            offset.width = rect.width / 2f;
            EditorGUI.LabelField(offset, trigger);
            offset.x += rect.width / 2f;
            if (GUI.Button(offset, "X"))
            {
                Undo.RecordObject(targetProfile, "Deleted State Machine Trigger");
                targetProfile.RemoveTrigger(trigger);
                Undo.FlushUndoRecordObjects();
                Repaint();
            }

            offset.x = rect.x;
            offset.width = rect.width;
            offset.y += EditorGUIUtility.singleLineHeight;
        }

        offset.width = rect.width / 2f;
        m_NewTrigger = EditorGUI.TextField(offset, m_NewTrigger);
        offset.x += rect.width / 2f;
        if (GUI.Button(offset, "Add Trigger"))
        {
            Undo.RecordObject(targetProfile, "Added State Machine Trigger");
            targetProfile.AddTrigger(m_NewTrigger);
            Undo.FlushUndoRecordObjects();
            m_NewTrigger = "";
            Repaint();
        }

        offset.x = rect.x;
        offset.width = rect.width;
        offset.y += EditorGUIUtility.singleLineHeight;
    }

    private void DrawStatesView(Rect rect)
    {
        var targetProfile = m_SelectedProfile == null ? m_SelectedComponent.Profile : m_SelectedProfile;
        EditorGUI.DrawRect(rect, new Color(42f / 255f, 42f / 255f, 42f / 255f));

        foreach (var state in targetProfile.States)
        {
            DrawState(state);
        }

        foreach (var transition in targetProfile.Transitions)
        {
            DrawTransition(transition);
        }
    }

    private void DrawStateInspector(Rect rect)
    {
        EditorGUI.DrawRect(rect, new Color(56f / 255f, 56f / 255f, 56f / 255f));

        var offset = new Rect(rect);
        offset.height = EditorGUIUtility.singleLineHeight;
        var targetProfile = m_SelectedProfile == null ? m_SelectedComponent.Profile : m_SelectedProfile;

        if (m_SelectedState != null)
        {
            var newString = EditorGUI.TextField(offset, "Name", m_SelectedState.Name);
            if (newString != m_SelectedState.Name)
            {
                Undo.RecordObject(targetProfile, "State Name Changed");
                m_SelectedState.Name = newString;
                Undo.FlushUndoRecordObjects();
                Repaint();
            }

            offset.y += EditorGUIUtility.singleLineHeight;

            var so = new SerializedObject(targetProfile);
            var spIndex = Array.IndexOf(targetProfile.States, m_SelectedState);
            var sp = so.FindProperty("m_States").GetArrayElementAtIndex(spIndex);
            

            for (var i = 0; i < m_SelectedState.Behaviours.Length; i++)
            {
                var serializedBehaviour = sp.FindPropertyRelative("m_Behaviours");
                DrawStateBehaviour(so, serializedBehaviour.GetArrayElementAtIndex(i), rect, ref offset);
            }
        }
    }

    private void DrawState(State state)
    {
        var position = m_StatesViewRect.position + m_StatesViewOffset + state.StateEditorPosition;
        GUI.Box(new Rect(position.x, position.y, STATE_WIDTH, STATE_HEIGHT), state.Name, m_StateStyle);
    }

    private void DrawTransition(StateTransition transition)
    {
    }

    private void ObtainInputs()
    {
        switch (Event.current.type)
        {
            case EventType.MouseDown:
            {
                OnMouseDown();
                break;
            }
            case EventType.MouseUp:
            {
                OnMouseUp();
                break;
            }
        }
    }

    private void UpdateInputs()
    {
        var targetProfile = m_SelectedProfile == null ? m_SelectedComponent.Profile : m_SelectedProfile;

        if (m_Moving)
        {
            if (!m_StatesViewRect.Contains(Event.current.mousePosition))
            {
                m_Moving = false;
                m_MovingState = null;
            }
            else
            {
                Undo.RecordObject(targetProfile, "State Position Moved");
                m_MovingState.StateEditorPosition += Event.current.delta / 2f;
                Undo.FlushUndoRecordObjects();
                Repaint();
            }
        }
        else if (m_Dragging)
        {
            m_StatesViewOffset += Event.current.delta / 2f;
            Repaint();
        }
    }

    private void OnMouseDown()
    {
        if (m_Dragging || m_Moving)
            return;

        if (Event.current.button == 0)
        {
            foreach (var state in m_SelectedProfile.States)
            {
                var position = m_StatesViewRect.position + m_StatesViewOffset + state.StateEditorPosition;
                var rect = new Rect(position.x, position.y, STATE_WIDTH, STATE_HEIGHT);
                if (rect.Contains(Event.current.mousePosition))
                {
                    m_Moving = true;
                    m_MovingState = state;
                    m_SelectedState = state;
                    return;
                }
            }
        }else if (Event.current.button == 1)
        {
            Undo.RecordObject(m_SelectedProfile, "Added New State");
            m_SelectedProfile.AddState();
            Undo.FlushUndoRecordObjects();
            Repaint();
        }
        else if (Event.current.button == 2)
        {
            m_Dragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (Event.current.button == 0)
        {
            if (m_Moving)
            {
                m_Moving = false;
                m_MovingState = null;
            }
        }
        else if (Event.current.button == 2)
        {
            if (m_Dragging)
                m_Dragging = false;
        }
    }

    private void DrawStateBehaviour(SerializedObject parentSO, SerializedProperty serializedProperty, Rect parentRect, ref Rect offsetRect)
    {
        var propertyType = Type.GetType(serializedProperty.type);
        foreach (var field in propertyType.GetFields())
        {
            EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative(field.Name));
        }

        parentSO.ApplyModifiedProperties();
    }
}
}