using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    GameObject[] hearts;
    List<GameObject> activeHearts;
    [SerializeField]
    Slider[] healthSlider;
    float maxHealth;
    float health;
    float bonusHealth;
    float trueMax;
    float heartMax;

    [SerializeField]
    float heartDelay;

    public static event Action heartDamage;

    private void Awake()
    {
        activeHearts = new List<GameObject>();
    }
    private void Start()
    {
        maxHealth = PlayerHealth._maxHealth;
        health = PlayerHealth._health;
        FillHearts();
    }

    private void OnEnable()
    {
        
    }
    private void Update()
    {
        maxHealth = PlayerHealth._maxHealth;
        health = PlayerHealth._health;
    }

    void FillHearts()
    {
        
        for(int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i].activeSelf)
            {
                activeHearts.Add(hearts[i]); 
            }
        }
        trueMax = maxHealth + bonusHealth;
        heartMax = trueMax / activeHearts.Count;
        //Debug.Log("heartMax: " + heartMax);
        float tempHealth = health;
        //Debug.Log("tempHealth: " + tempHealth);
        for (int i = 0; i < activeHearts.Count; i++)
        {
            healthSlider[i].maxValue = heartMax;
            if (tempHealth > heartMax)
            {  
                healthSlider[i].value = heartMax;
                tempHealth -= heartMax;
            }
            else
            {
                healthSlider[i].value = tempHealth;
                tempHealth = 0;
            }
        }
    }
    public void ApplyHeartDamage()
    {
        float damage = PlayerHitController.hitPower;
        for (int i = activeHearts.Count - 1; i >= 0; i--)
        {
            if(damage > 0)
            {
                if (healthSlider[i].value < damage && healthSlider[i].value > 0)
                {
                    damage -= healthSlider[i].value;
                    //activeHearts[i].GetComponent<HeartController>().HeartDamage();
                    healthSlider[i].value = 0;
                }
                else if (healthSlider[i].value == 0)
                {
                    continue;
                }
                else
                {
                    activeHearts[i].GetComponent<HeartController>().HeartDamage();
                    healthSlider[i].value -= damage;
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
    public void AddHealth()
    {
        float health = PlayerHitController.healthAmount * heartMax;

        for (int i = 0; i < activeHearts.Count; i++)
        {
            if(health > 0)
            {
                
                if (healthSlider[i].value == healthSlider[i].maxValue)
                {
                    Debug.Log("Already Full");
                    continue;
                }
                else if ((healthSlider[i].maxValue - healthSlider[i].value) < health)
                {
                    health -= (healthSlider[i].maxValue - healthSlider[i].value);
                    StartCoroutine(HeartPSDelay(i));
                    healthSlider[i].value = healthSlider[i].maxValue;
                }
                else
                {
                    StartCoroutine(HeartPSDelay(i));
                    healthSlider[i].value += health;
                    health = 0;
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator HeartPSDelay(int index)
    {
        activeHearts[index].GetComponent<HeartController>().AddHearts();
        yield return new WaitForSeconds(heartDelay);
        activeHearts[index].GetComponent<HeartController>().StopAdd();
        yield break;
    }
}
