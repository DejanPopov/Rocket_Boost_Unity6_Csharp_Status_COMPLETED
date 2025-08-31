using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;

    private void OnEnable()
    {
        thrust.Enable();
    }

    private void Update()
    {
        bool isThrustPressed = thrust.IsPressed();
        Debug.Log("Thrust is " + isThrustPressed);
    }
}
