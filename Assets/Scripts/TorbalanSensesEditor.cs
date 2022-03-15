using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TorbalanSenses))]
public class TorbalanSensesEditor : Editor
{
    private void OnSceneGUI() {
        TorbalanSenses senses = (TorbalanSenses) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(senses.transform.position, Vector3.up, Vector3.forward, 360, senses.viewRadius);
        Vector3 viewAngleA = senses.DirectionFromAngle(-senses.viewAngle / 2, false);
        Vector3 viewAngleB = senses.DirectionFromAngle(senses.viewAngle / 2, false);
        
        Handles.DrawLine(senses.transform.position, senses.transform.position + viewAngleA * senses.viewRadius);
        Handles.DrawLine(senses.transform.position, senses.transform.position + viewAngleB * senses.viewRadius);

        Handles.color = Color.red;
        /*
        foreach (Transform visibleTarget in senses.visibleTargets) {
            Handles.DrawLine(senses.transform.position, visibleTarget.position);
        }
        */
    }
    
}
