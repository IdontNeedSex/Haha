using UnityEngine;

public class ToolEvents : MonoBehaviour
{
    public delegate void OnToolTypeChanged(ToolType toolType);

    public event OnToolTypeChanged OnToolTypeChangedEvent;

    public delegate void OnToolActionEvent(ToolType toolType, Vector3Int mousePos);

    public event OnToolActionEvent OnToolActionTriggered;


    public void TriggerOnToolAction(ToolType toolType, Vector3Int mousePos)
    {
        Debug.Log("EVENT: Tool "+ toolType +" action trigger: "); 
        OnToolActionTriggered?.Invoke(toolType, mousePos);
    }

    public void TriggerOnToolChange(ToolType toolType)
    {
        Debug.Log("Event: Tool change from: " + toolType);
        OnToolTypeChangedEvent?.Invoke(toolType);
    }
}