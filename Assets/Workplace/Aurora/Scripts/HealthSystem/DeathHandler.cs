using System;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class DeathHandler : MonoBehaviour {

    private IHealth health;

    private void Awake() => health = GetComponent<IHealth>(); // grab local compononet
    private void OnEnable() => health.OnDeath += HandleDeath; //subscribe to action
    private void OnDisable() => health.OnDeath -= HandleDeath; // unsubscribe to action
    private void HandleDeath() {
        
        //Destroy(gameObject); Uncomment when we're testing death.
    }
}