using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

[CustomEditor (typeof (HabitatAttributes))]
public class HabitatEditor : Editor
{
    private void OnSceneGUI() {
        HabitatAttributes attr = target as HabitatAttributes;

        Vector3[] corners = { new Vector2(attr.MaxX, attr.MaxY), new Vector2(attr.MaxX, attr.MinY), new Vector2(attr.MinX, attr.MinY), new Vector2(attr.MinX, attr.MaxY), new Vector2(attr.MaxX, attr.MaxY) };

        Handles.color = Color.cyan;
        Handles.DrawPolyLine(corners);
        
    }
}
