using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] float thrustStrength = 100f;
    Rigidbody myRigidbody;

    private void OnEnable()
    {
        thrust.Enable();
    }

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() // For physics calculations, FixedUpdate is better
    {
        if (thrust.IsPressed())
        {
            myRigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        }    
    }
}
