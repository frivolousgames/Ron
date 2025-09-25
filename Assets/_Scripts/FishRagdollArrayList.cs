using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRagdollArrayList : MonoBehaviour
{
    //[SerializeField]
    //GameObject[] fishRagdolls;
    [SerializeField]
    GameObject[] seaTurtleRagdolls;
    [SerializeField]
    GameObject[] sturgeonRagdolls;
    [SerializeField]
    GameObject[] marlinRagdolls;
    [SerializeField]
    GameObject[] jellyfishRagdolls;
    [SerializeField]
    GameObject[] smallRagdoll;
    [SerializeField]
    GameObject[] largeRagdoll;

    public static Dictionary<string, GameObject[]> creatureRagdolls;

    private void Awake()
    {
        creatureRagdolls = new Dictionary<string, GameObject[]> // Add new ragdolls when creating new background creatures and ref strings in scripts
        {
            {"Small", smallRagdoll },
            {"Large", largeRagdoll },
            //{"Fish", fishRagdolls},
            {"Sea Turtle", seaTurtleRagdolls },
            {"Sturgeon", sturgeonRagdolls },
            {"Marlin", marlinRagdolls },
            {"Jellyfish", jellyfishRagdolls }
        };
    }
}
