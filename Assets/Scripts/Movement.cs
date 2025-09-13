using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    [Header("VFX / SFX")]
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] AudioClip mainEngineSFX;

    [Header("Input (Input System)")]
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [Header("Tuning")]
    [SerializeField] float thrustStrength = 100f;      // force applied for thrust (you can tweak)
    [SerializeField] float rotationStrength = 120f;    // degrees per second

    // Optional quick-tweaks:
    // [SerializeField] bool snapVisualToOrigin = false;
    // [SerializeField] bool autoSetCenterOfMassToRenderer = false;
    // [SerializeField] float autoAngularDrag = 1f;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;
    Renderer rend;

    private void OnEnable()
    {
        if (thrust != null) thrust.Enable();
        if (rotation != null) rotation.Enable();
    }

    private void OnDisable()
    {
        if (thrust != null) thrust.Disable();
        if (rotation != null) rotation.Disable();
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        rend = GetComponentInChildren<Renderer>();

        // Smooth visuals:
        myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        // OPTIONAL: snap visible child to origin if your mesh is offset (uncomment to try)
        // if (snapVisualToOrigin && rend != null && rend.transform != transform)
        // {
        //     rend.transform.localPosition = Vector3.zero;
        //     rend.transform.localRotation = Quaternion.identity;
        // }

        // OPTIONAL: auto-set COM to renderer center (useful when colliders offset COM)
        // if (autoSetCenterOfMassToRenderer && rend != null)
        // {
        //     Vector3 localRendererCenter = myRigidbody.transform.InverseTransformPoint(rend.bounds.center);
        //     myRigidbody.centerOfMass = localRendererCenter;
        // }

        // OPTIONAL: set angular drag to prevent small residual spin
        // myRigidbody.angularDrag = autoAngularDrag;
    }

    private void Start()
    {
        // safety: if InputActions were not assigned in inspector, try to avoid null refs
        if (thrust == null) Debug.LogWarning("Thrust InputAction is null on Movement.");
        if (rotation == null) Debug.LogWarning("Rotation InputAction is null on Movement.");
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust != null && thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = 0f;
        if (rotation != null)
        {
            rotationInput = rotation.ReadValue<float>();
        }

        if (rotationInput < 0f)
        {
            RotateRight(rotationInput);
        }
        else if (rotationInput > 0f)
        {
            RotateLeft(rotationInput);
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        if (rightEngineParticles != null) rightEngineParticles.Stop();
        if (leftEngineParticles != null) leftEngineParticles.Stop();
    }

    private void RotateLeft(float rotationInput)
    {
        // rotationInput is > 0 here
        ApplyRotation_MoveRotation(rotationInput);
        if (leftEngineParticles != null && !leftEngineParticles.isPlaying)
        {
            if (rightEngineParticles != null) rightEngineParticles.Stop();
            leftEngineParticles.Play();
        }
    }

    private void RotateRight(float rotationInput)
    {
        // rotationInput is < 0 here
        ApplyRotation_MoveRotation(rotationInput);
        if (rightEngineParticles != null && !rightEngineParticles.isPlaying)
        {
            if (leftEngineParticles != null) leftEngineParticles.Stop();
            rightEngineParticles.Play();
        }
    }

    // ---- Stable rotation: MoveRotation (no accumulating angular velocity) ----
    private void ApplyRotation_MoveRotation(float rotationInput)
    {
        // rotationInput is in range [-1, 1] (depending on your InputAction mapping)
        // rotationStrength is degrees per second
        float rotationThisStep = rotationInput * rotationStrength * Time.fixedDeltaTime; // degrees this physics step
        // Build delta rotation about Z axis (assuming rocket faces up on +Y)
        Quaternion delta = Quaternion.Euler(0f, 0f, -rotationThisStep); // negative to match typical input sign (flip if needed)
        myRigidbody.MoveRotation(myRigidbody.rotation * delta);
    }

    // ---- Old torque option (commented). Use only if you want physics-driven torque:
    // private void ApplyRotation_WithTorque(float rotationInput)
    // {
    //     // Convert to torque/acceleration and use ForceMode.Acceleration (mass-respecting)
    //     float rotateDegPerSec = rotationInput * rotationStrength;
    //     float torqueAmount = rotateDegPerSec * 0.5f * Time.fixedDeltaTime; // tune multiplier
    //     myRigidbody.AddRelativeTorque(Vector3.forward * torqueAmount, ForceMode.Acceleration);
    // }

    private void StopThrusting()
    {
        if (myAudioSource != null) myAudioSource.Stop();
        if (mainEngineParticles != null) mainEngineParticles.Stop();
    }

    private void StartThrusting()
    {
        // keep previous behaviour: AddRelativeForce with Time.fixedDeltaTime (feel free to tweak)
        if (myRigidbody != null)
            myRigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

        if (myAudioSource != null && mainEngineSFX != null && !myAudioSource.isPlaying)
            myAudioSource.PlayOneShot(mainEngineSFX);

        if (mainEngineParticles != null && !mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
    }
}
