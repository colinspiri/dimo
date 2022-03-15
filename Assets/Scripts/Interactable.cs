using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour {
    // components
    private Outline outline;

    // public constants
    public float interactionRadius = 5.0f;
    public float interactionAngle = 80.0f;

    // state
    private bool selected;

    protected virtual void Awake() {
        outline = GetComponent<Outline>();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        InteractableManager.Instance.AddInteractable(this);

        outline.enabled = false;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 4.0f;
    }

    // Update is called once per frame
    void Update() {
        // add as candidate for selection
        if (GetDistanceToPlayer() <= interactionRadius && GetAngleWithPlayer() <= interactionAngle){
            InteractableManager.Instance.AddCandidate(this);
        }
        else InteractableManager.Instance.RemoveCandidate(this);

        if (selected && Input.GetKeyDown(KeyCode.E)){
            Interact();
        }
    }

    protected virtual void Interact() {
        Debug.Log("Interacting with " + gameObject.name);
    }

    public abstract string GetUIText();

    public float GetDistanceToPlayer() {
        return Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
    }
    private float GetAngleWithPlayer() { 
        return Vector3.Angle(PlayerController.Instance.transform.forward, transform.position - PlayerController.Instance.transform.position);
    }

    public void Select() {
        selected = true;
        outline.enabled = true;
    }
    public void Deselect() {
        selected = false;
        outline.enabled = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    protected virtual void OnDestroy() {
        InteractableManager.Instance.RemoveInteractable(this);
    }
}
