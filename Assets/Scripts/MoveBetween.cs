using System;
using System.Collections;
using UnityEngine;

public class MoveBetween : MonoBehaviour
{
    [Tooltip("Origen")] public Transform origin;
    [Tooltip("Destino")] public Transform destination;
    [Tooltip("Speed")][Range(0.001f, 10f)] public float speed = 1;
    
    private bool isReturning = false;
    private bool isWaiting = false;
    
    /// <summary>
    /// Controla la velocidad de movimiento global
    /// </summary>
    private float gSpeed = 1;

    private void Start()
    {
        UpdateSpeed();
        GameManager.Instance.onSpeedChanged.AddListener(UpdateSpeed);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isWaiting) return;

        var target = isReturning ? origin.position : destination.position;
        var moveSpeed = speed * gSpeed * Time.fixedDeltaTime;

        var pos = transform.position;
        var nextPos = Vector3.MoveTowards(pos, target, moveSpeed);
        transform.position = nextPos;

        if (Vector3.Distance(pos, target) < 0.01f)
        {
            StartCoroutine(SwitchDirectionWithDelay(1f));
        }
    }

    private void UpdateSpeed()
    {
        gSpeed = GameManager.Instance.GlobalSpeed;
    }
    
    private IEnumerator SwitchDirectionWithDelay(float delay)
    {
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        isReturning = !isReturning;
        isWaiting = false;
    }
}
