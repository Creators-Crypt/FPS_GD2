using UnityEngine;


/// <summary>
/// 
/// Generic spell projectile. Works for straight shots gravity = false
/// and arcing shots gravity = true. Prefab needs: Rigidbody & Collider
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpellProjectile : MonoBehaviour {

    [Tooltip("Optional VFX spawned on impact.")]
    [SerializeField] private GameObject impactVfxPrefab;
    [Tooltip("Destroy on the first thing hit, or pierce through everything.")]
    [SerializeField] private bool destroyOnHit = true;

    private Rigidbody rb;
    private SpellData data;
    private SpellElement element;
    private float multiplier = 1f;
    private void Awake() {

        rb = GetComponent<Rigidbody>();
        // start the rb with dynamic
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>Called by the delivery strategy right after Instantiate.</summary>
    public void Launch(SpellData spellData, SpellElement spellElement, Vector3 direction , float damageMultiplier)
    {
        data = spellData;
        element = spellElement;
        multiplier = damageMultiplier;
        rb.useGravity = false;
        rb.linearVelocity = direction.normalized * spellData.projectileSpeed;

        // Face travel direction; arcing shots keep re-facing in Update.
        FaceVelocity(rb.linearVelocity);

        Destroy(gameObject, spellData.projectileLifetime);
    }
    public void Launch(SpellData spellData, SpellElement spellElement, Vector3 direction, bool gravityState, float damageMultiplier) { 
        data = spellData;
        element = spellElement;
        multiplier = damageMultiplier;

        rb.useGravity = gravityState;
        rb.linearVelocity = direction.normalized * spellData.projectileSpeed;

        // Face travel direction; arcing shots keep re-facing in Update.
        FaceVelocity(rb.linearVelocity);

        Destroy(gameObject, spellData.projectileLifetime);
    }

    private void Update() {
    
        if (rb.linearVelocity.sqrMagnitude > 0f) {
            if (rb.useGravity) FaceVelocity(rb.linearVelocity);
        }
    }

    private void FaceVelocity(Vector3 velocity) {

        if (velocity.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data == null) return;

        // Only react to layers this spell is allowed to hit.
        if ((data.hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent(out IDamageable dmg))
            dmg.OnDamage(data.damage * multiplier);

        if (impactVfxPrefab != null)
        {
            var vfx = Instantiate(impactVfxPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        if (destroyOnHit) Destroy(gameObject);
    }
}