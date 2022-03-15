using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TorbalanController : MonoBehaviour {
    // components
    private NavMeshAgent agent;
    private TorbalanSenses senses;
    
    // public constants
    public float killDistance;
    public List<Transform> passiveRoute;
    public float closeEnoughDistance;
    
    // state
    private enum AIState { Passive, Search, Chase }
    private AIState state;
    private int nextPassiveNode;
    
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start() {
        ChangeState(AIState.Passive);
    }

    // Update is called once per frame
    void Update() {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeState(AIState.Passive);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeState(AIState.Search);
        else if(Input.GetKeyDown(KeyCode.Alpha3)) ChangeState(AIState.Chase);

        // state-specific updates
        if (state == AIState.Passive) {
            // set next node as destination
            agent.SetDestination(passiveRoute[nextPassiveNode].position);

            // if close enough, go to next node
            float distanceToNode = Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position);
            // Debug.Log("distance to node " + nextPassiveNode + " = " + distanceToNode);
            if (distanceToNode <= closeEnoughDistance) {
                nextPassiveNode++;
                nextPassiveNode %= passiveRoute.Count;
            }
        }
        else if (state == AIState.Search) {
            
        }
        else if (state == AIState.Chase) {
            // follow the player
            agent.SetDestination(PlayerController.Instance.transform.position);
            // if close enough to the player, game over
            float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
            if (distance <= killDistance) {
                // game over
                Debug.Log("GAME OVER");
            }
        }
    }

    private void ChangeState(AIState newState) {
        Debug.Log(gameObject.name + " switched from " + state + " to " + newState);
        state = newState;
        if(state == AIState.Passive) InitializePassiveState();
    }

    private void InitializePassiveState() {
        // set next passive node equal to closest node in the path
        for (int i = 0; i < passiveRoute.Count; i++) {
            var distance = Vector3.Distance(transform.position, passiveRoute[i].position);
            if (distance < Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position)) {
                nextPassiveNode = i;
            }
        }
    }
}
