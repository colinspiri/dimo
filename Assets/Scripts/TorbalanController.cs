using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(TorbalanSenses))]
public class TorbalanController : MonoBehaviour {
    // components
    private NavMeshAgent agent;
    private TorbalanSenses senses;
    
    // public constants
    public float killDistance;
    public List<Transform> passiveRoute;
    public float closeEnoughDistance;
    public float maxSearchTime;
    public float passiveSpeed;
    public float chaseSpeed;
    
    // state
    private enum AIState { Passive, Search, Chase }
    private AIState state;
    // passive
    private int nextPassiveNode;
    // search
    private Vector3 searchLocation;
    private float searchTimer;
    // chase
    
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        senses = GetComponent<TorbalanSenses>();
    }

    // Start is called before the first frame update
    void Start() {
        ChangeState(AIState.Passive);

        senses.onPlayerEnterSight += () => {
            if (state == AIState.Passive) ChangeState(AIState.Search);
        };
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
            // Debug.Log("distance to node " + nextPassiveNode + " = " + distanceToNode);
            if (Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position) <= closeEnoughDistance) {
                nextPassiveNode++;
                nextPassiveNode %= passiveRoute.Count;
            }
            
            // if player noticed, chase
            if(senses.PlayerNoticed()) ChangeState(AIState.Chase);
        }
        else if (state == AIState.Search) {
            // go towards last known player location
            agent.SetDestination(searchLocation);
            
            // if close enough, go back to passive
            if (Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position) <= closeEnoughDistance) {
                ChangeState(AIState.Passive);
            }
            // if been searching for enough time, go back to passive
            searchTimer += Time.deltaTime;
            if (searchTimer >= maxSearchTime) {
                ChangeState(AIState.Passive);
            }
            
            // if player noticed, chase
            if(senses.PlayerNoticed()) ChangeState(AIState.Chase);
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
            
            // if player no longer within line of sight, search for player
            if(!senses.PlayerNoticed()) ChangeState(AIState.Search);
        }
    }

    private void ChangeState(AIState newState) {
        Debug.Log(gameObject.name + " switched from " + state + " to " + newState);
        state = newState;
        if(state == AIState.Passive) InitializePassiveState();
        else if(state == AIState.Search) InitializeSearchState();
        else if (state == AIState.Chase) InitializeChaseState();
    }

    private void InitializePassiveState() {
        // set next passive node equal to closest node in the path
        for (int i = 0; i < passiveRoute.Count; i++) {
            var distance = Vector3.Distance(transform.position, passiveRoute[i].position);
            if (distance < Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position)) {
                nextPassiveNode = i;
            }
        }
        // set speed
        agent.speed = passiveSpeed;
    }

    private void InitializeSearchState() {
        searchTimer = 0;
        // store last known player location
        searchLocation = PlayerController.Instance.transform.position;
        // set speed
        agent.speed = passiveSpeed;
    }

    private void InitializeChaseState() {
        // set speed
        agent.speed = chaseSpeed;
    }
}
