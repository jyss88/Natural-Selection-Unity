using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (CreatureAttributes))]
public class CreatureEditor : Editor
{
    private CreatureAttributes attr;
    private void OnSceneGUI() {
        attr = target as CreatureAttributes;

        Handles.color = Color.red;
        Handles.DrawWireArc(attr.transform.position, Vector3.forward, Vector3.right, 360, attr.SightRadius);

        if (attr.TargetFood != null) {
            Handles.DrawWireArc(attr.TargetFood.transform.position, Vector3.forward, Vector3.right, 360, 1);
        }
        
    }
}
