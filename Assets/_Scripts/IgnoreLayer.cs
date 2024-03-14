using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLayer : MonoBehaviour
{
    public string layer1;
    public string layer2;

    private void FixedUpdate()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(layer1), LayerMask.NameToLayer(layer2));
    }
}
