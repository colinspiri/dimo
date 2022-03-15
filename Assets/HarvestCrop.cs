using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCrop : MonoBehaviour {
    // public constants
    public float harvestRadius;

    // components
    public Transform player;
    
    // private state
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        selected = distanceToPlayer <= harvestRadius;

        if (selected && Input.GetKey(KeyCode.E)) {
            Harvest();
        }
    }

    private void Harvest() {
        // add to count
        
        // despawn
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, harvestRadius);
    }
}
