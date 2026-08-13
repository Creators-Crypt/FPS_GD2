using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Everything a delivery strategy needs to execute one cast.
/// Built once per cast by Spell and handed to the strategy.
/// 
/// </summary>
public struct SpellCastContext {

    public SpellData data;
    public SpellElement element;      // May differ from data.element (runtime override)
    public float multiplier;
    public Transform caster;
    public Vector3 origin;
    public Vector3 direction;         // Normalized aim direction
    public MonoBehaviour runner;      // Coroutine host for staggered spawns / beam fade
}

/// <summary>
/// 
/// STRATEGY pattern: one implementation per delivery shape.
/// A Spell holds a strategy and can swap it at runtime to change its type.
/// 
/// </summary>
public interface ISpellDeliveryStrategy {

    SpellDeliveryKind Kind { get; }
    void Cast(SpellCastContext context);
}

/// <summary>
/// 
/// Shared multi-spawn plumbing: fires Execute() spawnCount times,
/// fanned across spreadAngle, optionally staggered by spawnInterval.
/// 
/// </summary>
public abstract class SpellDeliveryStrategyBase : ISpellDeliveryStrategy {

    public abstract SpellDeliveryKind Kind { get; }
    public void Cast(SpellCastContext context) {

        int count = Mathf.Max(1, context.data.spawnCount);

        if (count == 1) {

            Execute(context, context.direction);
            return;
        }

        if (context.data.spawnInterval > 0f && context.runner != null)
            context.runner.StartCoroutine(StaggeredCast(context, count));
        else
            for (int i = 0; i < count; i++)
                Execute(context, FanDirection(context, i, count));
    }
    private IEnumerator StaggeredCast(SpellCastContext context, int count) {
    
        for (int i = 0; i < count; i++) {
            Execute(context, FanDirection(context, i, count));
            yield return new WaitForSeconds(context.data.spawnInterval);
        }
    }
    private static Vector2 FanDirection(SpellCastContext context, int index, int count) {
    
        float spread = context.data.spreadAngle;
        float t = count > 1 ? (float)index / (count - 1) : 0.5f;
        float angle = Mathf.Lerp(-spread * 0.5f, spread * 0.5f, t);
        return Quaternion.Euler(0f, 0f, angle) * context.direction;
    }
    /// <summary>Fire one instance of this delivery in the given direction. </summary>
    protected abstract void Execute(SpellCastContext context, Vector3 direction);
    /// <summary>Damage every IDamageable in a circle. Shared by AOE and Hand. </summary>
    protected static void DamageCircle(SpellCastContext context, Vector3 center, float radius) {

        var hits = Physics2D.OverlapCircleAll(center, radius, context.data.hitLayers);
        foreach (var hit in hits) {
            if (hit.TryGetComponent(out IDamageable dmg))
                dmg.OnDamage(context.data.damage * context.multiplier);
        }
    }
}

// ------------------------------------------------------------------
// Concrete strategies
// ------------------------------------------------------------------

/// <summary> Straight-flying projectile. </summary>
public class ProjectileDelivery : SpellDeliveryStrategyBase {

    public override SpellDeliveryKind Kind => SpellDeliveryKind.Projectile;
    protected override void Execute(SpellCastContext context, Vector3 direction) {
        
        if (context.data.projectilePrefab == null) {
            Debug.LogWarning($"[Spell] '{context.data.spellName}' has no projectilePrefab assigned.");
            return;
        }

        var proj = Object.Instantiate(context.data.projectilePrefab, context.origin, Quaternion.identity);
        proj.Launch(context.data, context.element, direction, context.multiplier);
    }
}

/// <summary> Gravity-affected lobbed projectile. </summary>
public class ArcProjectileDelivery : SpellDeliveryStrategyBase {

    public override SpellDeliveryKind Kind => SpellDeliveryKind.ArcProjectile;
    protected override void Execute(SpellCastContext ctx, Vector3 direction) {
        
        if (ctx.data.projectilePrefab == null) {
            Debug.LogWarning($"[Spell] '{ctx.data.spellName}' has no projectilePrefab assigned.");
            return;
        }

        // Tilt the aim upward by arcLaunchAngle (mirrored for leftward aim).
        float sign = direction.x >= 0f ? 1f : -1f;
        Vector3 arced = Quaternion.Euler(0f, 0f, ctx.data.arcLaunchAngle * sign) * direction;

        var proj = Object.Instantiate(ctx.data.projectilePrefab, ctx.origin, Quaternion.identity);
        proj.Launch(ctx.data, ctx.element, arced, true, ctx.multiplier);
    }
}

/// <summary> Instant hitscan ray/laser with a brief LineRenderer visual. </summary>
public class RayDelivery : SpellDeliveryStrategyBase {

    public override SpellDeliveryKind Kind => SpellDeliveryKind.Ray;

    protected override void Execute(SpellCastContext context, Vector3 direction) {

        Vector3 endPosition = context.origin + direction * context.data.rayDistance;

        var hits = Physics2D.RaycastAll(context.origin, direction, context.data.rayDistance, context.data.hitLayers);
        foreach (var hit in hits) {
            if (hit.collider.TryGetComponent(out IDamageable dmg))
                dmg.OnDamage(context.data.damage * context.multiplier);
        }

        // Beam visual - short-lived LineRenderer.
        var beam = new GameObject($"SpellBeam_{context.data.spellName}");
        var lineRenderer = beam.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, context.origin);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.startWidth = context.data.rayWidth;
        lineRenderer.endWidth = context.data.rayWidth;
        lineRenderer.material = context.data.rayMaterial != null
            ? context.data.rayMaterial
            : new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = context.data.rayColor;
        lineRenderer.endColor = context.data.rayColor;
        lineRenderer.sortingOrder = 50;

        Object.Destroy(beam, Mathf.Max(0.02f, context.data.rayVisualDuration));
    }
}

/// <summary> Area burst at the caster or a point along the aim direction. </summary>
public class AoeDelivery : SpellDeliveryStrategyBase {
    public override SpellDeliveryKind Kind => SpellDeliveryKind.Aoe;

    protected override void Execute(SpellCastContext context, Vector3 direction) {

        Vector3 center = context.origin + direction * context.data.aoeCastDistance;
        DamageCircle(context, center, context.data.aoeRadius);

        if (context.data.aoeVfxPrefab != null) {

            var vfx = Object.Instantiate(context.data.aoeVfxPrefab, center, Quaternion.identity);
            Object.Destroy(vfx, 3f);
        }
    }
}

/// <summary> Close-range touch burst in front of the caster. </summary>
public class HandDelivery : SpellDeliveryStrategyBase {

    public override SpellDeliveryKind Kind => SpellDeliveryKind.Hand;

    protected override void Execute(SpellCastContext context, Vector3 direction) {

        Vector3 center = context.origin + direction * context.data.handRange;
        DamageCircle(context, center, context.data.handRadius);

        if (context.data.handVfxPrefab != null) {
            var vfx = Object.Instantiate(context.data.handVfxPrefab, center, Quaternion.identity);
            Object.Destroy(vfx, 2f);
        }
    }
}