using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorbalanSenses : MonoBehaviour {
    // public constants
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public float timeToRecognizePlayer;

    // state
    private bool playerWithinSight;
    private float recognizeTimer;
    private bool playerRecognized;

    private void Start() {
        StartCoroutine(LookForPlayerOnDelay(0.2f));
    }

    private void Update() {
        // if player within sight, start timer to recognize them
        if (playerWithinSight) {
            recognizeTimer += Time.deltaTime;
            if (recognizeTimer >= timeToRecognizePlayer) {
                playerRecognized = true;
            }
        }
        else recognizeTimer = 0;
    }

    private IEnumerator LookForPlayerOnDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            LookForPlayer();
        }
    }

    private void LookForPlayer() {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        // search through all targets in the radius
        foreach (var t in targetsInViewRadius) {
            Transform target = t.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // if within angle
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                // if no obstacles between self and player
                playerWithinSight = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask);
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
