using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] AudioClip mainEngineSFX;

    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;

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
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        myAudioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightEngineParticles.Stop();
        leftEngineParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftEngineParticles.isPlaying)
        {
            rightEngineParticles.Stop();
            leftEngineParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightEngineParticles.isPlaying)
        {
            leftEngineParticles.Stop();
            rightEngineParticles.Play();
        }
    }

    private void ApplyRotation(float rotateThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotateThisFrame * Time.fixedDeltaTime);
        myRigidbody.freezeRotation = false;
    }

    private void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngineSFX);
        }

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }
}
