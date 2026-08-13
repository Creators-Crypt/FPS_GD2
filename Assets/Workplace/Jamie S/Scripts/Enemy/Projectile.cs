using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 5.0f;

    private Vector3 direction;
    private float speed;
    private float damage;
    

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Fire(Vector3 _direction, float _speed, float _Damage)
    {
        direction = _direction;
        speed = _speed;
        damage = _Damage;
        Destroy(this.gameObject, bulletLifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damagable = other.GetComponent<IDamageable>();
        if (damagable == null) return;

        damagable.OnDamage(damage);
        Destroy(this.gameObject);
    }
}
