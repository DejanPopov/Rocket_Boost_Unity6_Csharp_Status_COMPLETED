using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float   speed;

    Vector3 startPosistion;
    Vector3 endPosition;
    float   movementFactor;


    void Start()
    {
        startPosistion = transform.position;
        endPosition   = startPosistion + movementVector;
    }

    void Update()
    {
        movementFactor      = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPosistion, endPosition, movementFactor);
    }
}
