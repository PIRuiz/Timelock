using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Tooltip("Instancia")] public static GameManager Instance;
    
    [Tooltip("Lista de Objetivos")] public List<GameObject> targets;
    [Tooltip("Salida")] public GameObject exit;
    [Tooltip("Objetivos restantes")] public int remainingTargets;
    [Tooltip("Texto coleccionables")] [SerializeField]
    private TextMeshProUGUI collectiblesText;
    
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
    
    /// <summary>
    /// Contar objetos coleccionables
    /// </summary>
    private void CountCollectibles()
    {
        remainingTargets = targets.Count;
    }

    /// <summary>
    /// Cuenta cuantos artículos coleccionables quedan, actualiza el texto en canvas y devuelve el valor
    /// </summary>
    /// <returns>Cantidad de coleccionables restantes</returns>
    public int UpdateCollectibles()
    {
        CountCollectibles();
        collectiblesText.text = $"x {remainingTargets}";
        return remainingTargets;
    }
}
