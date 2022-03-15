using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCrop : Interactable {

    protected override void Interact() {
        base.Interact();
        
        // add to count
        CropCounter.Instance.CountCrop();
        
        // despawn
        Destroy(gameObject);
    }

    public override string GetUIText() {
        return "harvest tomato";
    }
}
