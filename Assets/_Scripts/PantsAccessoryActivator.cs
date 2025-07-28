using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PantsAccessoryActivator : MonoBehaviour
{
    [SerializeField]
    GameObject belt;
    [SerializeField]
    GameObject walletChain;
    SkinnedMeshRenderer mr;
    [SerializeField]
    string[] jeanMatNames;

    private void Awake() //This accessory system is temporary and broken. Most likely add Clothing categories/SOs for accessories like belts, chains, etc. Some are activatable for only certain items
    {
        mr = GetComponent<SkinnedMeshRenderer>();
        //Debug.Log("Name: " + gameObject.name);
    }
    public void SetChainAndBeltActive()
    {
        foreach(string name in jeanMatNames)
        {
            if(mr.sharedMaterial.name == name)
            {
                //Debug.Log("Name: " + mr.sharedMaterial.name);
                belt.SetActive(true);
                walletChain.SetActive(true);
                break;
            }
            else
            {
                belt.SetActive(false);
                walletChain.SetActive(false);
            }
        }
    }

    public void PosCargoBlendShapes()
    {
        var currentBlendShape = mr.sharedMesh.GetBlendShapeIndex("Cargo");
        mr.SetBlendShapeWeight(currentBlendShape, 100);
    }
    public void NegCargoBlendShapes()
    {
        var currentBlendShape = mr.sharedMesh.GetBlendShapeIndex("Cargo");
        mr.SetBlendShapeWeight(currentBlendShape, 0);
    }
}
