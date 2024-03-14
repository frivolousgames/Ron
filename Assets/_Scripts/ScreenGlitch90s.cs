using IE.RichFX;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ScreenGlitch90s : MonoBehaviour
{
    public VolumeProfile ppVolume;
    public ScreenGlitch glitchFX;
    public float speed = .1f;


    private void OnEnable()
    {
        for (int i = 0; i < ppVolume.components.Count; i++)
        {
            if (ppVolume.components[i].name == "ScreenGlitch")
            {
                glitchFX = (ScreenGlitch)ppVolume.components[i];
                Debug.Log("Gotem");
            }
        }
        glitchFX.intensity.value = 0;
        glitchFX.speed.value = 0;
        glitchFX.randomInferencePower.value = 0;
        glitchFX.linePower.value = 0;
        glitchFX.colorLerp.value = 0;
        glitchFX.xDisplacement.value = 0;
        StartCoroutine(GlitchGrow());
    }

    private void OnDisable()
    {
        glitchFX.intensity.value = 0;
        glitchFX.speed.value = 0;
        glitchFX.randomInferencePower.value = 0;
        glitchFX.linePower.value = 0;
        glitchFX.colorLerp.value = 0;
        glitchFX.xDisplacement.value = 0;
    }
    IEnumerator GlitchGrow()
    {
        //yield return new WaitForSeconds(2);
        while(glitchFX.speed.value < 100)
        {
            glitchFX.intensity.value += speed;
            glitchFX.speed.value += speed / 2;
            glitchFX.randomInferencePower.value += speed;
            glitchFX.linePower.value += speed;
            glitchFX.colorLerp.value += speed;
            glitchFX.xDisplacement.value += speed;
            speed += .00001f;
            yield return null;
        }
        yield break;
    }
}


