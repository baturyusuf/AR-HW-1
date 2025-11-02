using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMove : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Destroy(gameObject, 8.0f);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Force);
        }
    }
}
