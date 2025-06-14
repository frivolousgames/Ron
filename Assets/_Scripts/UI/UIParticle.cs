using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    Image image;
    UIParticleSystem ps;

    //Color
    Color startColor;
    Color endColor;
    Color fadeInColor;
    float fadeInSpeed;

    //Size
    float startSize;
    float endSize;

    //Direction
    Vector3 startDirection;

    //Lifetime
    float startLifetime;

    //Speed
    float startSpeed;

    //Time
    float time;

    //Wiggle
    float wiggleX;
    float wiggleY;
    float wiggleXSpeed;
    float wiggleYSpeed;
    float wiggleXLerp;
    float wiggleYLerp;

    //Gravity
    float gravity;
    float gravityY;
    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        ps = transform.parent.gameObject.GetComponent<UIParticleSystem>();
        image = GetComponent<Image>();
        time = 0f;
        startColor = ps._startColor;
        endColor = ps._endColor;
        fadeInColor = ps._fadeInColor;
        fadeInSpeed = ps._fadeInSpeed;
        startDirection = ps._startDirection;
        startLifetime = ps._startLifetime;
        startSize = ps._startSize;
        endSize = ps._endSize;
        startSpeed = ps._startSpeed;
        image.color = startColor;
        wiggleX = ps._wiggleX;
        wiggleXSpeed = ps._wiggleXSpeed;
        wiggleY = ps._wiggleY;
        wiggleYSpeed = ps._wiggleYSpeed;
        transform.localScale = new Vector3(startSize, startSize, startSize);
        gravity = ps._gravity;
    }

    private void Update()
    {
        time += Time.deltaTime;
        wiggleXLerp = Mathf.Lerp(-wiggleX, wiggleX, Mathf.PingPong(time * wiggleXSpeed, 1f));
        wiggleYLerp = Mathf.Lerp(-wiggleY, wiggleY, Mathf.PingPong(time * wiggleYSpeed, 1f));
        gravityY = transform.position.y;
        gravityY += gravity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + wiggleXLerp, gravityY + wiggleYLerp, transform.position.z);
        transform.position += startSpeed * startDirection * Time.deltaTime;
        image.color = Color.Lerp(Color.Lerp(fadeInColor, startColor, time * fadeInSpeed), endColor, time / startLifetime);
        transform.localScale = Vector3.Lerp(new Vector3(startSize, startSize, startSize), new Vector3(endSize, endSize, endSize), time / startLifetime);
        //Debug.Log("Wiggle: " + wiggleXLerp);

        if(time > startLifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
