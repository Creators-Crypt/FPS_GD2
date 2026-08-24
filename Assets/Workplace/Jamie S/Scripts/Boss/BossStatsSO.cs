using UnityEngine;

[CreateAssetMenu(fileName = "BossStatsSO", menuName = "Scriptable Obj/BossStatsSO")]
public class BossStatsSO : EnemyStatsSO
{
    [Header("When to change phases health%")]
    [Range(0f, 1f)] public float phase2HealthPercent = .75f;
    [Range(0f, 1f)] public float phase3HealthPercent = 0.05f;

    [Header("Material Phase Change")]
    public Material phase1Material;
    public Material phase2Material;
    public Material phase3Material;
    public Material stunMaterial;
    public float colorBlendTime = 1.25f;

    [Header("Eye/vulnerable")]
    public float eyeDamageMultiplier = 2f;
    public int eyeHitsToStun = 3;
    public float eyeHitTime = 5f;
    public float stunDuration = 4f;
    public float stunDamageMultiplier = 1.5f;

    [Header("Phase1 Slime Bullets")]
    public float p1StartDelay = 1.5f;
    public float p1TimeBetweenVolley = 1.5f;
    public int p1ShotsPerVolley = 3;
    public float p1TimeBetweenShots = .25f;
    public float p1SpreadDegrees = 4f;
    public float p1ProjectileSpeed = 18f;
    public float p1ProjectileDamage = 6f;
    public float bulletAimAheadOfPlayer = .6f;

    [Header("Phase1 Slime Mortars")]
    public GameObject mortarPrefab;
    public GameObject mortarHitPosDisplayPrefab;
    public float mortarCooldown = 6f;
    public int mortarShellsPreSalvo = 4;
    public float mortarTimeBetweenShells = .35f;
    public float mortarSpeed = 14f;
    public float mortarArcHeight = 6f;
    public float mortarSplashRadius = 4f;
    public float mortarDamage = 14f;
    public float mortarScatter = 3f;
    public float mortarAimAheadOfPlayer = 1f;



}
