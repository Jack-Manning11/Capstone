using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionHandleExample)), CanEditMultipleObjects]
public class ColliderCreator : Editor
{
    protected virtual void OnSceneGUI()
    {
        PositionHandleExample example = (PositionHandleExample)target;

        EditorGUI.BeginChangeCheck();
        // Get the position of the target GameObject
        Vector3 targetPosition = example.transform.position;
        // Add a small horizontal offset
        float horizontalOffset = 2f; // Adjust as needed
        targetPosition += new Vector3(horizontalOffset, 0, 0);
        // Draw the handle at the modified position
        Vector3 newTargetPosition = Handles.PositionHandle(targetPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            // Calculate the offset from the original position
            example.xOffset = newTargetPosition.x - targetPosition.x;
            example.UpdateColliderVertices();
        }
    }
}
