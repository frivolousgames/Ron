using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfWaver : MonoBehaviour
{
    [SerializeField]
    Animator ronAnim;

    [SerializeField]
    bool isWaving;

    [SerializeField]
    GameObject zoomCam;
    private void Awake()
    {
        isWaving = true;
    }
    private void Update()
    {
        ronAnim.SetBool("isWaving", isWaving);
    }
    public void StopWaving()
    {
        isWaving = false;
    }
    public void DisableZoomCam()
    {
        zoomCam.SetActive(false);
    }

}
