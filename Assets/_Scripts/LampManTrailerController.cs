using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LampManTrailerController : MonoBehaviour
{

    [SerializeField]
    protected Light attackLight;
    [SerializeField]
    protected GameObject lightEffectObject;
    protected VisualEffect lightEffect;
    [SerializeField]
    protected float lightRangeMax;
    [SerializeField]
    protected float lightRangeMin;
    [SerializeField]
    protected float lightDelay;
    [SerializeField]
    protected float glowSpeed;

    private void Start()
    {
        lightEffect = lightEffectObject.GetComponent<VisualEffect>();
    }
    private void OnEnable()
    {
        attackLight.range = lightRangeMin;
    }

    public void Glow()
    {
        StartCoroutine(LightGlow());
    }
    IEnumerator LightGlow()
    {
        while (attackLight.range < lightRangeMax)
        {
            attackLight.range += glowSpeed * Time.deltaTime;
            yield return null;
        }
        lightEffect.Play();
        yield return new WaitForSeconds(lightDelay);
        lightEffect.Stop();
        while (attackLight.range > lightRangeMin)
        {
            attackLight.range -= glowSpeed * Time.deltaTime * 2;
            yield return null;
        }
        attackLight.range = lightRangeMin;
        lightEffect.Stop();
        yield break;
    }
}
