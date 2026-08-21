using UnityEngine;

public class FallingObjects : MonoBehaviour
{
    [SerializeField] float fallSpeed = 15f;
    [SerializeField] int damage = 25;
    [SerializeField] float resetDelay = 2f;

    Vector3 StartPos;
    Rigidbody rb;

    bool isFalling;
    bool hasHitPlayer;

    void Start()
    {
        StartPos = transform.position;
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        if (isFalling)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isFalling && other.CompareTag("Player"))
        {
            isFalling = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitPlayer && collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                // Take damage
            }

            Invoke(nameof(ResetObject), resetDelay);
        }
    }

    void ResetObject()
    {
        transform.position = StartPos;

        isFalling = false;
        hasHitPlayer = false;
    }
}