using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Higher number means less Lag in crosshairs for reaching the mouse position")]
    float moveSpeed;
    float jitterX;
    float jitterY;
    [SerializeField]
    float jitterMax;
    [SerializeField]
    float jitterSpeed;

    private void OnEnable()
    {
        //Cursor.visible = false;//Temp
        StartCoroutine(JitterDelay());
    }
    private void OnDisable()
    {
        //Cursor.visible = true;//Temp
        StopAllCoroutines();
    }
    private void Update()
    {
        
        Vector3 jitterPos = new Vector3(Input.mousePosition.x + jitterX, Input.mousePosition.y + jitterY, 0f);
        Vector3 newPos = Vector3.Slerp(transform.position, jitterPos, Time.deltaTime * moveSpeed);
        transform.position = newPos;
    }

    IEnumerator JitterDelay()
    {
        while(true)
        {
            jitterX = Random.Range(-jitterMax, jitterMax);
            jitterY = Random.Range(-jitterMax, jitterMax);
            //jitterX = 0f; //Temp
            //jitterY = 0f; //Temp
            yield return new WaitForSeconds(jitterSpeed);
        }
    }
}
