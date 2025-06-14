using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpinBoardController : MonoBehaviour
{
    [SerializeField]
    [Tooltip ("Set to 1 or -1 to control spin direction (1 is forward -1 is backward")]
    float isPositive;

    bool isHit;

    [SerializeField]
    UnityEvent positive;
    [SerializeField]
    UnityEvent negative;

    [SerializeField]
    Transform board;
    [SerializeField]
    float spinSpeed;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Dwarf"))
        {
            if(!isHit)
            {
                isHit = true;
                if (isPositive > 0)
                {
                    positive.Invoke();
                }
                else
                {
                    negative.Invoke();
                }
            }
        }
    }

    private void Update()
    {
        board.transform.Rotate(0f, isPositive * spinSpeed, 0f);
    }
}
