using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Tooltip("Instancia")] public static GameManager Instance;
    
    /// <summary>
    /// Velocidad Global del Juego
    /// </summary>
    public float GlobalSpeed
    {
        get => globalSpeed;
        set
        {
            globalSpeed = value;
            onSpeedChanged.Invoke();
        }
    }

    [Tooltip("Velocidad del Juego")][SerializeField][Range(0, 1f)] private float globalSpeed = 1;
    [Tooltip("Evento cambio de velocidad")] public UnityEvent onSpeedChanged;

    private void Awake()
    {
        if (!Instance) Instance = this;
        onSpeedChanged ??= new UnityEvent();
    }

    private void OnDisable()
    {
        Instance =  null;
    }
}
