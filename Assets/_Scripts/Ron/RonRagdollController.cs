using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonRagdollController : MonoBehaviour
{
    [SerializeField]
    float ragdollDelay;
    [SerializeField]
    GameObject player;

    private void OnEnable()
    {
        StartCoroutine(ReenableRon());
    }

    IEnumerator ReenableRon()
    {
        //add if !isdead check
        yield return new WaitForSeconds(ragdollDelay);
        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.SetActive(true);
        gameObject.SetActive(false);
    }
}
