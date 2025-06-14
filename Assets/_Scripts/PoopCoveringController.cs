using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopCoveringController : MonoBehaviour
{
    SkinnedMeshRenderer mRenderer;

    private void Awake()
    {
        mRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    private void OnEnable()
    {
        mRenderer.SetBlendShapeWeight(1, Random.Range(0f, 100f));
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
