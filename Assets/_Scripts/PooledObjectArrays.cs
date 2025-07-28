using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectArrays : MonoBehaviour
{
    ///Player Weapon///
    [SerializeField]
    GameObject[] bullets;
    [SerializeField]
    GameObject[] bulletDust;
    [SerializeField]
    GameObject[] bulletHoles;
    [SerializeField]
    GameObject[] bloodHits;
    [SerializeField]
    GameObject[] muzzleFlashes;
    [SerializeField]
    GameObject[] gunSmokes;
    [SerializeField]
    GameObject[] shellCasings;
    [SerializeField]
    GameObject[] chunkHits;
    [SerializeField]
    GameObject[] sparkHits;
    [SerializeField]
    GameObject[] fractureHits;

    public static GameObject[] bulletsArray;
    public static GameObject[] bulletDustArray;
    public static GameObject[] bulletHolesArray;
    public static GameObject[] bloodHitsArray;
    public static GameObject[] muzzleFlashesArray;
    public static GameObject[] gunsSmokesArray;
    public static GameObject[] shellCasingsArray;
    public static GameObject[] chunkHitsArray;
    public static GameObject[] sparkHitsArray;
    public static GameObject[] fractureHitsArray;

    ///Generic Organs///
    [SerializeField]
    GameObject[] livers;
    [SerializeField]
    GameObject[] intestines;

    public static GameObject[] liversArray;
    public static GameObject[] intestinesArray;


    ///Katie Zombie///
    [SerializeField]
    GameObject[] zombieArms;
    [SerializeField]
    GameObject[] zombieLegs;

    public static GameObject[] zombieArmsArray;
    public static GameObject[] zombieLegsArray;


    ///Demon Child///
    [SerializeField]
    GameObject[] demonChildArms;
    [SerializeField]
    GameObject[] demonChildLegs;
    [SerializeField]
    GameObject[] demonChildHeads;

    public static GameObject[] demonChildArmsArray;
    public static GameObject[] demonChildLegsArray;
    public static GameObject[] demonChildHeadsArray;

    ///Candyland///
    [SerializeField]
    GameObject[] squirts;
    [SerializeField]
    GameObject[] squirtHits;
    [SerializeField]
    GameObject[] squirtSpatters;

    public static GameObject[] squirtsArray;
    public static GameObject[] squirtHitsArray;
    public static GameObject[] squirtSpattersArray;

    [SerializeField]
    GameObject[] airplaneBullets;

    public static GameObject[] airplaneBulletsArray;

    ///Toilet Man///
    [SerializeField]
    GameObject[] poop;
    [SerializeField]
    GameObject[] poopHit;
    [SerializeField]
    GameObject[] poopSpatterHit;

    public static GameObject[] poopArray;
    public static GameObject[] poopHitArray;
    public static GameObject[] poopSpatterHitArray;

    [SerializeField]
    GameObject[] throwPoop;

    ///Gooch///
    [SerializeField]
    GameObject[] goochPoop;

    public static GameObject[] goochPoopArray;
    public static GameObject[] throwPoopArray;

    ///Aunt Pam
    [SerializeField]
    GameObject[] milkBullets;
    [SerializeField]
    GameObject[] milkSpatters;
    [SerializeField]
    GameObject[] milkMarks;
    [SerializeField]
    GameObject[] gooSpatters;
    [SerializeField]
    GameObject[] acidSmoke;
    [SerializeField]
    GameObject[] gooParticles;

    public static GameObject[] milkBulletArrays;
    public static GameObject[] milkSpatterArrays;
    public static GameObject[] milkMarkArrays;
    public static GameObject[] gooSpatterArrays;
    public static GameObject[] acidSmokeArrays;
    public static GameObject[] gooParticleArrays;

    ///Under Water///
    [SerializeField]
    GameObject[] spermBullets;

    public static GameObject[] spermBulletArrays;

    private void Awake()
    {
        ///Player Weapon///
        bulletsArray = bullets;
        bulletDustArray = bulletDust;
        bulletHolesArray = bulletHoles;
        bloodHitsArray = bloodHits;
        muzzleFlashesArray = muzzleFlashes;
        gunsSmokesArray = gunSmokes;
        shellCasingsArray = shellCasings;
        chunkHitsArray = chunkHits;
        sparkHitsArray = sparkHits;
        fractureHitsArray = fractureHits;

        ///Generic Organs///
        liversArray = livers;
        intestinesArray = intestines;

        ///Katie Zombie///
        zombieArmsArray = zombieArms;
        zombieLegsArray = zombieLegs;

        ///Demon Child///
        demonChildArmsArray = demonChildArms;
        demonChildLegsArray = demonChildLegs;
        demonChildHeadsArray = demonChildHeads;

        ///Candyland///
        squirtsArray = squirts;
        squirtHitsArray = squirtHits;
        squirtSpattersArray = squirtSpatters;
        airplaneBulletsArray = airplaneBullets;

        ///ToiletMan///
        poopArray = poop;
        poopHitArray = poopHit;
        poopSpatterHitArray = poopSpatterHit;

        throwPoopArray = throwPoop;

        ///Gooch///
        goochPoopArray = goochPoop;

        ///Aunt Pam///
        milkBulletArrays = milkBullets;
        milkSpatterArrays = milkSpatters;
        milkMarkArrays = milkMarks;
        gooSpatterArrays = gooSpatters;
        acidSmokeArrays = acidSmoke;
        gooParticleArrays = gooParticles;

        ///Under Water
        spermBulletArrays = spermBullets;
    }
}
