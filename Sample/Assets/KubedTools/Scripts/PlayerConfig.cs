using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerConfig : ScriptableObject {

    public static PlayerConfig playerConfig;

    public float autoaimAngle = 10f;
    public float autoaimForce = 2f;

    //время перед уменьшением разброса
    public float accuracyOffsetTime = 0.1f;

    public Vector3 thirdPersonOffset = new Vector3(0.5f, 0f, -1.5f);
    public float folowSpeed = 10f;

    public float sniperNoAimAccuracy = 20f;
    public float sensitivityY = 1f;

    public float spreadSize = 0.2f;
    public int maxHealth = 100;

    public float height = 1.8f;
    public float heightCrounch = 1.5f;

    public float autoFireDist = 30;
    public float autoFireDistAim = 90;
    public float sensitivityAim = 0.5f;
    public float sensitivitySniperAim = 0.25f;
    public float fullShotDelay = 1f;
    public float thirdPersonYMul = 0.8f;

    public float headHeight = 1.43f;
    public float teleportDist = 2f;
    public float anticheatDamageDist = 4f;
    public float anticheatFromDist = 4f;

    public float strafeSpeed = 4f;

    public int damagePing = 200;
    public float newMeleeShortcutDist = 2f;
    public float damegeDelay = 3f;

    public float maxWallPlaceDist = 1f;

    //perk - regen
    public float regenerationDelta = 3f;

    // perk explosion
    public float explosionDamage = 100;

    [Tooltip("Percent 0-100")]
    public float addClipPerk = 50;//prc 0-100
    [Tooltip("Percent 0-100")]
    public float hitDamagePerk_Damage = 30;//prc 0-100
    [Tooltip("Percent 0-100")]
    public float hitDamagePerk_Accuracy = 30;//prc 0-100

    [Tooltip("mul 0-1")]
    public float addMoneyPerk = 0.5f;// mul 0-1

    public float respawnTime =5f;

    public float rayCastDist = 0.5f;

    public float delaySniperShooting = 1f; // Задержка выстрела снайперки
    public float aimCrosshair = 20f;       // Сведение снайперского прицела

    [System.Serializable]
    public class ShakeCfg
    {

        public float Amplitude = 7f;//How far away from the normal position the camera will wobble. 
        public float Duration = 0.1f;//How far away from the normal position the camera will wobble. 
    }

    public ShakeCfg damageShake;

    public float maxPredictTime = 1f; // how much time we can predict
    public float lerpTime = 0.05f;//how long do lepr


    public float GROUND_FRICTION = 95f;
    public float AIR_FRICTION = 25f;

    public float airAccelerate = 25f;
}
