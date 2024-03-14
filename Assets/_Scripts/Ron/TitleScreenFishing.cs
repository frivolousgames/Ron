using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenFishing : MonoBehaviour
{
    Animator anim;
    public Animator rodAnim;

    bool fishing;
    public bool inAction;

    List<string> triggerList;

    public GameObject bottleHeld;
    public GameObject bottleSit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fishing = true;
        triggerList = new List<string>
        {
            "Reel",
            "Cast",
            "Drink"
        };

    }

    private void Start()
    {
        StartCoroutine(FishingRoutine());
    }

    IEnumerator FishingRoutine()
    {
        while (fishing)
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            int triggerIndex = Random.Range(0, triggerList.Count);
            anim.SetTrigger(triggerList[triggerIndex]);
            rodAnim.SetTrigger(triggerList[triggerIndex]);

            inAction = true;
            {
                while (inAction)
                {
                    yield return null;
                }
            }
            yield return null;
        }
    }

    public void SwitchBool()
    {
        if (inAction)
        {
            inAction = false;
        }
    }
}
