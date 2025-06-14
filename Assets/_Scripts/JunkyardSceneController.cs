using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkyardSceneController : MonoBehaviour
{
    public static bool playerActivated;
    public static bool playerReady;

    [SerializeField]
    GameObject mainCam;

    [SerializeField]
    GameObject closeCam;

    [SerializeField]
    GameObject fightCam;


    [SerializeField]
    float closeCamWait;

    [SerializeField]
    float climbWait;

    [SerializeField]
    float playerWait;

    [SerializeField]
    GameObject[] devilChildren;

    private void Awake()
    {
        StartCoroutine(CamSwitch());
    }

    IEnumerator CamSwitch()
    {
        while (true)
        {
            yield return new WaitForSeconds(closeCamWait);
            closeCam.SetActive(false);
            yield return new WaitForSeconds(climbWait);
            foreach (var child in devilChildren)
            {
                child.SetActive(true);
            }
            yield return new WaitForSeconds(playerWait);
            playerActivated = true;
            while(!playerReady)
            {
                yield return null;
            }
            fightCam.SetActive(true);
            yield return null;
        }

    }
}
