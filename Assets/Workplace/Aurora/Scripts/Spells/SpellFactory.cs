using UnityEngine;

/// <summary>
/// 
/// FACTORY pattern: maps a SpellDeliveryKind to its strategy instance.
/// Strategies are stateless, so one shared instance per kind is enough.
/// This is the single place to register new delivery shapes.
/// 
/// </summary>
public static class SpellFactory {

    private static readonly ProjectileDelivery projectile = new ProjectileDelivery();
    private static readonly ArcProjectileDelivery arcProjectile = new ArcProjectileDelivery();
    private static readonly RayDelivery ray = new RayDelivery();
    private static readonly AoeDelivery aoe = new AoeDelivery();
    private static readonly HandDelivery hand = new HandDelivery();

    public static ISpellDeliveryStrategy GetDelivery(SpellDeliveryKind kind) {

        switch (kind) {

            case SpellDeliveryKind.Projectile:    return projectile;
            case SpellDeliveryKind.ArcProjectile: return arcProjectile;
            case SpellDeliveryKind.Ray:           return ray;
            case SpellDeliveryKind.Aoe:           return aoe;
            case SpellDeliveryKind.Hand:          return hand;
            default:
                Debug.LogWarning($"[SpellFactory] Unknown delivery kind {kind}, defaulting to Projectile.");
                return projectile;
        }
    }

    /// <summary> Create a runtime Spell straight from its SO with no overrides. </summary>
    public static Spell Create(SpellData data) { return new SpellBuilder(data).Build(); }
}

/// <summary>
/// 
/// BUILDER pattern: fluent construction of a runtime Spell from a SpellData SO,
/// with optional per-instance overrides (delivery shape, element, damage, multi-spawn...).
///
/// var spell = new SpellBuilder(fireballData)
///     .WithDelivery(SpellDeliveryKind.ArcProjectile)
///     .WithSpawnCount(3, spreadAngle: 45f)
///     .WithElement(SpellElement.Fire)
///     .Build();
///     
/// </summary>
public class SpellBuilder {

    private readonly SpellData source;

    private SpellDeliveryKind? delivery;
    private SpellElement? element;

    private float? damage;
    private float? staminaCost;
    private float? concentrationCost;
    private float? cooldown;
    private int? spawnCount;
    private float? spreadAngle;
    private float? spawnInterval;

    public SpellBuilder(SpellData source) { this.source = source; }

    public SpellBuilder WithDelivery(SpellDeliveryKind kind)       { delivery = kind; return this; }
    public SpellBuilder WithElement(SpellElement elementType)      { element = elementType; return this; }
    public SpellBuilder WithDamage(float dmg)                      { damage = dmg; return this; }
    public SpellBuilder WithStaminaCost(float stamina)             { staminaCost = stamina; return this; }
    public SpellBuilder WithConcentrationCost(float concentration) { concentrationCost = concentration; return this; }
    public SpellBuilder WithCooldown(float waitTime)               { cooldown = waitTime; return this; }

    public SpellBuilder WithSpawnCount(int count, float? spreadAngle = null, float? interval = null) {

        spawnCount = Mathf.Max(1, count);

        if (spreadAngle.HasValue) this.spreadAngle = spreadAngle.Value;

        if (interval.HasValue) spawnInterval = interval.Value;

        return this;
    }

    public Spell Build() {

        if (source == null) {

            Debug.LogError("[SpellBuilder] Cannot build a spell from null SpellData.");
            return null;
        }

        // Runtime copy so overrides never touch the shared SO asset.
        var runtimeData = Object.Instantiate(source);
        runtimeData.name = source.name + " (Runtime)";

        if (damage.HasValue)            runtimeData.damage = damage.Value;
        if (staminaCost.HasValue)       runtimeData.staminaCost = staminaCost.Value;
        if (concentrationCost.HasValue) runtimeData.concentrationCost = concentrationCost.Value;
        if (cooldown.HasValue)          runtimeData.cooldown = cooldown.Value;
        if (spawnCount.HasValue)        runtimeData.spawnCount = spawnCount.Value;
        if (spreadAngle.HasValue)       runtimeData.spreadAngle = spreadAngle.Value;
        if (spawnInterval.HasValue)     runtimeData.spawnInterval = spawnInterval.Value;
        if (delivery.HasValue)          runtimeData.delivery = delivery.Value;
        if (element.HasValue)           runtimeData.element = element.Value;

        var strategy = SpellFactory.GetDelivery(runtimeData.delivery);
        return new Spell(runtimeData, strategy);
    }
}

/// <summary>
/// 
/// Runtime spell instance: data + current delivery strategy + current element.
/// SetDelivery / SetElement change the spell's type on the fly.
/// 
/// </summary>
public class Spell {

    public SpellData Data { get; }
    public ISpellDeliveryStrategy Delivery { get; private set; }
    public SpellElement Element { get; private set; }
    public float CooldownRemaining { get; private set; }
    public bool IsReady => CooldownRemaining <= 0f;
    public Spell(SpellData data, ISpellDeliveryStrategy delivery) {

        Data = data;
        Delivery = delivery;
        Element = data.element;
    }

    /// <summary> Change the delivery shape at runtime (projectile -> ray, etc.). </summary>
    public void SetDelivery(SpellDeliveryKind kind) {

        Data.delivery = kind;
        Delivery = SpellFactory.GetDelivery(kind);
    }

    /// <summary> Change the element at runtime without rebuilding. </summary>
    public void SetElement(SpellElement element) => Element = element;

    /// <summary>Tick from the owning MonoBehaviour's Update.</summary>
    public void Tick(float deltaTime) { CooldownRemaining = Mathf.Max(0f, CooldownRemaining - deltaTime); }

    /// <summary>
    /// 
    /// Cast toward a direction. Returns false if on cooldown or misconfigured.
    /// Caller is responsible for paying stamina first (check Data.staminaCost).
    /// 
    /// </summary>
    public bool Cast(MonoBehaviour runner, Transform caster, Vector3 origin, Vector3 direction, float multiplier)
    {
        if (!IsReady || Data == null || Delivery == null) return false;

        if (direction.sqrMagnitude < 0.0001f) direction = Vector3.forward;

        var context = new SpellCastContext
        {

            data = Data,
            element = Element,
            caster = caster,
            origin = origin,
            direction = direction.normalized,
            runner = runner,
            multiplier = multiplier
        };

        Delivery.Cast(context);
        CooldownRemaining = Data.cooldown;
        return true;
    }
}