using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskeyFlameController : MonoBehaviour
{
    [SerializeField]
    float loopWait;
    ParticleSystem ps;
    ParticleSystem.MainModule mainModule;
    ParticleSystem.EmissionModule emissionModule;
    //ParticleSystem.ColorOverLifetimeModule colorovertimeModule;
    //Color startColorMin;
    //Color startColorMax;
    //ParticleSystem.MinMaxGradient startGrad;
    ParticleSystem.MinMaxCurve startRate;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
        emissionModule = ps.emission;
        startRate = emissionModule.rateOverTime;
        //colorovertimeModule = ps.colorOverLifetime;
        //startColorMin = mainModule.startColor.colorMin;
        //startColorMax = mainModule.startColor.colorMax;
        //startGrad = new Gradient();
        //startGrad = new ParticleSystem.MinMaxGradient(startColorMin, startColorMax);
        
    }
    private void OnEnable()
    {
        mainModule.loop = true;
        //mainModule.simulationSpace = ParticleSystemSimulationSpace.Local;
        mainModule.startLifetimeMultiplier = 1f;
        //mainModule.startColor = startGrad;
        emissionModule.rateOverTime = startRate;
        Debug.Log(startRate.constant);
        //StartCoroutine(SlowEmission());
        StartCoroutine(LoopDelay());
    }


    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
    private void LateUpdate()
    {
        if (transform.parent != null)
        {
            
            //        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, rotY,  1f);
            //        //rotX = transform.rotation.y - transform.parent.rotation.y;
            //        //transform.rotation = Quaternion.Euler(transform.rotation.x, rotX, transform.rotation.z);
            //        Debug.Log(transform.rotation);
            //        //transform.Rotate(new Vector3(x, y, z));
        }

    }

    private void OnDisable()
    {

    }

    IEnumerator SlowEmission()
    {
        int i = (int)startRate.constant;
        while (true)
        {
            i -= 100;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(startRate.constant + (float)i);
            yield return null;
        }
    }

    IEnumerator LoopDelay()
    {
        yield return new WaitForSeconds(loopWait);
        mainModule.loop = false;
        transform.parent = null;
        transform.localScale = Vector3.one;
        mainModule.startLifetimeMultiplier = .1f;
        //float i = 20f;
        //while(i > 0f)
        //{
        //    colorovertimeModule.color = new ParticleSystem.MinMaxGradient(new Color(0, 0, 0, i));
        //    i -= .01f;
        //    yield return null;
        //}
        //colorovertimeModule.color = new ParticleSystem.MinMaxGradient(Color.clear, new Color(0,0,0,.1f));
        //mainModule.startColor = new Color(mainModule.startColor.color.r, mainModule.startColor.color.g, mainModule.startColor.color.g, .1f);
        //mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        yield break;
    }
}
