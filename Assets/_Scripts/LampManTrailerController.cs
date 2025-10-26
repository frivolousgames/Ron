using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LampManTrailerController : MonoBehaviour
{
    Animator anim;

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

    public bool isUnfolding;

    public AudioSource flameSource;
    public AudioClip flame;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        lightEffect = lightEffectObject.GetComponent<VisualEffect>();
    }
    private void OnEnable()
    {
        attackLight.range = lightRangeMin;
    }

    private void Update()
    {
        anim.SetBool("isUnfolded", isUnfolding);
    }

    public void Unfold()
    {
        isUnfolding = true;
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
        flameSource.pitch = Random.Range(.95f, 1.05f);
        flameSource.PlayOneShot(flame);
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
