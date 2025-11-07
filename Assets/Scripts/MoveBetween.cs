using System;
using System.Collections;
using UnityEngine;

public class MoveBetween : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Back = Animator.StringToHash("Back");
    [Tooltip("Origen")] public Transform origin;
    [Tooltip("Destino")] public Transform destination;
    [Tooltip("Speed")][Range(0.001f, 10f)] public float speed = 1;
    [Tooltip("Animator")] public Animator animator;
    
    private bool isReturning = true;
    private bool isWaiting = true;
    
    /// <summary>
    /// Controla la velocidad de movimiento global
    /// </summary>
    private float gSpeed = 1;

    private void Start()
    {
        UpdateSpeed();
        GameManager.Instance.onSpeedChanged.AddListener(UpdateSpeed);
        StartCoroutine(SwitchDirectionWithDelay(1f));
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
        animator.speed = gSpeed;
    }
    
    private IEnumerator SwitchDirectionWithDelay(float delay)
    {
        isWaiting = true;
        animator.SetBool(Moving, false);
        yield return new WaitForSeconds(delay);
        animator.SetBool(Moving, true);
        isReturning = !isReturning;
        animator.SetTrigger(isReturning ? Back : Forward);
        isWaiting = false;
    }
}
