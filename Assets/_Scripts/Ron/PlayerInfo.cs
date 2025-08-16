using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    /////////WEAPONS//////////
    //Add each weapon here


    public static int[] weaponID;

    int fistsID = 1;
    int butcherKnifeID = 2;
    int revolverID = 3;
    int shotgunID = 4;
    int reamerID = 5;
    int acousticID = 6;
    int punisherID = 7;
    int uziID = 8;

    public static int[] weaponPower;

    int fistsPower = 3;
    int butcherKnifePower = 5;
    int revolverPower = 7;
    int shotgunPower = 10;
    int reamerPower = 6;
    int acousticPower = 7;
    int punisherPower = 1;
    int uziPower = 4;

    public static float[] shootWait;

    float fistsShootWait = .4f;
    float butcherKnifeShootWait = .475f;
    float revolverShootWait = .55f;
    float shotgunShootWait = .625f;
    float reamerShootWait = .257f;
    float acousticShootWait = .257f;
    float punisherShootWait = .5715f;
    float uziShootWait = .1f;

    public static float[] raiseWait;

    float fistsRaiseWait = .292f;
    float butcherKnifeRaiseWait = .292f;
    float revolverRaiseWait = .25f;
    float shotgunRaiseWait = .25f;
    float reamerRaiseWait = .292f;
    float acousticRaiseWait = .292f;
    float punisherRaiseWait = 0f;
    float uziRaiseWait = .25f;

    public static int[] weaponLayer;

    int fistsLayer = 1;
    int butcherKnifeLayer = 2;
    int revolverLayer = 3;
    int shotgunLayer = 4;
    int reamerLayer = 5;
    int acousticLayer = 6;
    int punisherLayer = 7;
    int uziLayer = 8;

    public static bool[] aimable;

    bool fistsAimable = false;
    bool butcherKnifeAimable = false;
    bool revolverAimable = true;
    bool shotgunAimable = false;
    bool reamerAimable = false;
    bool acousticAimable = false;
    bool punisherAimable = false;
    bool uziAimable = true;

    public static bool[] automatic;

    bool fistsAuto = false;
    bool butcherKnifeAuto = false;
    bool revolverAuto = true;
    bool shotgunAuto = true;
    bool reamerAuto = false;
    bool acousticAuto = false;
    bool punisherAuto = false;
    bool uziAuto = true;

    public static int[] bulletSpawn;

    int fistsSpawn = -1;
    int butcherKnifeSpawn = -1;
    int revolverSpawn = 0;
    int shotgunSpawn = -1;
    int reamerSpawn = -1;
    int acousticSpawn = -1;
    int punisherSpawn = -1;
    int uziSpawn = 1;

    public static bool[] spendShells;

    bool fistsSpendShells = false;
    bool butcherSpendShells = false;
    bool revolverSpendShells = false;
    bool shotgunSpendShells = true;
    bool reamerSpendShells = false;
    bool acousticSpendShells = false;
    bool punisherSpendShells = false;
    bool uziSpendShells = true;

    public static bool[] hasKnife;

    bool fistsHasKnife = false;
    bool butcherHasKnife = true;
    bool revolverHasKnife = false;
    bool shotgunHasKnife = false;
    bool reamerHasKnife = false;
    bool acousticHasKnife = false;
    bool punisherHasKnife = false;
    bool uziHasKnife = false;


    public static bool[] hasFists;

    bool fistsHasFists = true;
    bool butcherHasFists = false;
    bool revolverHasFists = false;
    bool shotgunHasFists = false;
    bool reamerHasFists = false;
    bool acousticHasFists = false;
    bool punisherHasFists = false;
    bool uziHasFists = false;

    public static bool[] hasMeleeTwoHanded;

    bool fistsHasMelee2 = false;
    bool butcherHasMelee2 = false;
    bool revolverHasMelee2 = false;
    bool shotgunHasMelee2 = false;
    bool reamerHasMelee2 = true;
    bool acousticHasMelee2 = true;
    bool punisherHasMelee2 = false;
    bool uziHasMelee2 = false;

    public static bool[] hasDildo;

    bool fistsHasDildo = false;
    bool butcherHasDildo = false;
    bool revolverHasDildo = false;
    bool shotgunHasDildo = false;
    bool reamerHasDildo = false;
    bool acousticHasDildo = false;
    bool punisherHasDildo = true;
    bool uziHasDildo = false;

    //Vehicle Guns

    public static int[] vehicleWeaponID;

    int gummyAirplaneGun = 1;
    int spermGun = 2;

    public static int[] vehicleWeaponPower;
    
    int gummyAirplaneGunPower = 1;
    int spermGunPower = 1;

    public static float[] vehicleWeaponShootWait;

    float gummyAirplaneGunShootWait = .2f;
    float spermGunShootWait = .2f;

    private void Awake()
    {
        weaponID = new int[]
        {
            fistsID,
            butcherKnifeID,
            revolverID,
            shotgunID,
            reamerID,
            acousticID,
            punisherID,
            uziID
        };
        weaponPower = new int[]
        {
            fistsPower,
            butcherKnifePower,
            revolverPower,
            shotgunPower,
            reamerPower,
            acousticPower,
            punisherPower,
            uziPower
        };
        raiseWait = new float[]
        {
            fistsRaiseWait,
            butcherKnifeRaiseWait,
            revolverRaiseWait,
            shotgunRaiseWait,
            reamerRaiseWait,
            acousticRaiseWait,
            punisherRaiseWait,
            uziRaiseWait
        };
        shootWait = new float[]
        {
            fistsShootWait,
            butcherKnifeShootWait,
            revolverShootWait,
            shotgunShootWait,
            reamerShootWait,
            acousticShootWait,
            punisherShootWait,
            uziShootWait
        };
        weaponLayer = new int[]
        {
            fistsLayer,
            butcherKnifeLayer,
            revolverLayer,
            shotgunLayer,
            reamerLayer,
            acousticLayer,
            punisherLayer,
            uziLayer
        };

        aimable = new bool[]
        {
            fistsAimable,
            butcherKnifeAimable,
            revolverAimable,
            shotgunAimable,
            reamerAimable,
            acousticAimable,
            punisherAimable,
            uziAimable
        };

        automatic = new bool[]
        {
            fistsAuto,
            butcherKnifeAuto,
            revolverAuto,
            shotgunAuto,
            reamerAuto,
            acousticAuto,
            punisherAuto,
            uziAuto
        };

        bulletSpawn = new int[]
        {
            fistsSpawn,
            butcherKnifeSpawn,
            revolverSpawn,
            shotgunSpawn,
            reamerSpawn,
            acousticSpawn,
            punisherSpawn,
            uziSpawn
        };

        spendShells = new bool[]
        {
            fistsSpendShells,
            butcherSpendShells,
            revolverSpendShells,
            shotgunSpendShells,
            reamerSpendShells,
            acousticSpendShells,
            punisherSpendShells,
            uziSpendShells
        };

        hasFists = new bool[]
        {
            fistsHasFists,
            butcherHasFists,
            revolverHasFists,
            shotgunHasFists,
            reamerHasFists,
            acousticHasFists,
            punisherHasFists,
            uziHasFists
        };

        hasKnife = new bool[]
        {
            fistsHasKnife,
            butcherHasKnife,
            revolverHasKnife,
            shotgunHasKnife,
            reamerHasKnife,
            acousticHasKnife,
            punisherHasKnife,
            uziHasKnife
        };

        hasMeleeTwoHanded = new bool[]
        {
            fistsHasMelee2,
            butcherHasMelee2,
            revolverHasMelee2,
            shotgunHasMelee2,
            reamerHasMelee2,
            acousticHasMelee2,
            punisherHasMelee2,
            uziHasMelee2
        };

        hasDildo = new bool[]
        {
            fistsHasDildo,
            butcherHasDildo,
            revolverHasDildo,
            shotgunHasDildo,
            reamerHasDildo,
            acousticHasDildo,
            punisherHasDildo,
            uziHasDildo
        };

        vehicleWeaponID = new int[]
        {
            gummyAirplaneGun,
            spermGun
        };

        vehicleWeaponPower = new int[]
        {
            gummyAirplaneGunPower,
            spermGunPower
        };

        vehicleWeaponShootWait = new float[]
        {
            gummyAirplaneGunShootWait,
            spermGunShootWait
        };
    }
}
