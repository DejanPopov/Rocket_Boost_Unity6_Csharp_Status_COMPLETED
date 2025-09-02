using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float       thrustStrength = 100f;
    [SerializeField] float       rotationStrength = 5f;

    Rigidbody                    myRigidbody;
    AudioSource                  myAudioSource;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        myRigidbody   = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() // For physics calculations, FixedUpdate is better
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            myRigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play();
            }
        }
        else 
        {
            myAudioSource.Stop();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            ApplayRotation(rotationStrength);
        }
        else if (rotationInput > 0)
        {
            ApplayRotation(-rotationStrength);
        }
    }

    private void ApplayRotation(float rotateThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotateThisFrame * Time.fixedDeltaTime);
        myRigidbody.freezeRotation = false;
    }
}
