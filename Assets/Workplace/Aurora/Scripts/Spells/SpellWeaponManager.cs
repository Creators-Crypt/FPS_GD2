using UnityEngine;
using UnityEngine.InputSystem;

public class SpellWeaponManager : MonoBehaviour {

    [Header("Weapon Slots")]
    [Tooltip("Max amount of weapons is 2!")]
    [SerializeField] private SpellWeaponData[] weaponSlots = new SpellWeaponData[2];

    [Header("Swap Settings")]
    [Tooltip("How many seconds the Player needs to wait for swap.")]
    [SerializeField] private float swapCooldown = 0.5f;

    [Header("Input")]
    [SerializeField] private InputAction scrollAction;

    private int activeSlotIndex = 0;
    private float swapTimer = 0f;
    private SpellCaster spellCaster;

    public SpellWeaponData ActiveWeapon => weaponSlots[activeSlotIndex];
    public bool CanSwap => swapTimer <= 0f;

    private void Awake() {
        spellCaster = GetComponent<SpellCaster>();
    }
    private void Start() { UpdateCasterWeapon(); }
    private void Update() { 
        
        if (swapTimer > 0f) {
            swapTimer -= Time.deltaTime;
        }

        HandleWeaponInput(); 
    }
    private void HandleWeaponInput() {

        Vector2 scrollValue = scrollAction.ReadValue<Vector2>();

        if (Mathf.Abs(scrollValue.y) > 0.1f) CycleWeapon();

    }
    private void CycleWeapon() {

        if (!CanSwap) return;

        activeSlotIndex = (activeSlotIndex == 1) ? 0 : 1;

        UpdateCasterWeapon();
        swapTimer = swapCooldown;
    }
    public void EquipWeapon(int slotIndex, SpellWeaponData newWeapon) {

        if (slotIndex < 0 || slotIndex > 1) return;

        weaponSlots[slotIndex] = newWeapon;

        if (slotIndex == activeSlotIndex) UpdateCasterWeapon();
    }
    private void UpdateCasterWeapon() {
        spellCaster.SetWeapon(ActiveWeapon);
    }
}