using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;


    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        // This is just to avoid an error that ocurs when the period value is zero, because Unity says that we cannot divide a number by 0 (Mathf.Epsilon is the smallest float value).
        if (period <= Mathf.Epsilon) { return; }
        
        // This three variables are fot defining the "oscillation" circle cycle and the time that it will take to repeat.
        float cycles = Time.time / period; // continually growing over time
        const float tau = Mathf.PI * 2; // constant value of PI * 2 (6.283...)
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // Recalculated to go from 0 to 1 so its cleaner
        
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
