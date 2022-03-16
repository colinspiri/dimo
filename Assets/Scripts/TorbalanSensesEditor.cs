using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR;

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
        if (senses.PlayerNoticed()) {
            Handles.DrawLine(senses.transform.position, PlayerController.Instance.transform.position);
        }
        else if (senses.PlayerWithinSight()) {
            Handles.DrawDottedLine(senses.transform.position, PlayerController.Instance.transform.position, 1f);
        }
    }
}
