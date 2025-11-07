using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.UI;

public class TimeStopper : MonoBehaviour
{
    [Header("Saturation")]
    [Tooltip("Referencia a Global Volume")] public Volume volume;
    [Tooltip("Referencia al ajuste de color")] public ColorAdjustments colorAdjustments;
    [Tooltip("Saturación mínima")] public float minHue = -100f;
    [Tooltip("Saturación máxima")] public float maxHue = 100f;
    [Tooltip("Velocidad de cambio de saturación")][Range(0.001f, 100)] public float hueShift = 5f;
    [Header("Energy")]
    [Tooltip("Nivel de energía")] [Range(0, 1f)] public float energy = 1;
    [Tooltip("Velocidad de desgaste")] [Range(0, 10f)] public float energyExpenditure = 1;
    [Tooltip("Velocidad de recuperación")] [Range(0, 10f)] public float energyReplenish = 2;
    [Tooltip("Imagen de medidor de energía")] public Image energyMeter;

    /// <summary>
    /// Rutina de cambio de saturación
    /// </summary>
    private Coroutine shifRoutine;

    public bool stopped;

    private void OnAttack(InputValue input)
    {
        shifRoutine = StartCoroutine(!stopped ? EnableTimeStop() : DisableTimeStop());
    }

    private void Start()
    {
        colorAdjustments.saturation.value = 25;
        LookForColor();
        stopped = false;
    }

    private void Update()
    {
        EnergyManagement();
    }

    private void OnDisable()
    {
        if (shifRoutine != null) StopCoroutine(shifRoutine);
    }
    
    /// <summary>
    /// Controla la energía del personaje
    /// </summary>
    private void EnergyManagement()
    {
        if (stopped)
        {
            energy -= Time.deltaTime * energyExpenditure;
            if (energy <= 0) shifRoutine = StartCoroutine(DisableTimeStop());
        }
        else
        {
            if (energy < 1) energy += Time.deltaTime * energyReplenish;
            else energy = 1;
        }
        if (energyMeter) energyMeter.fillAmount = energy;
    }

    /// <summary>
    /// Busca el perfil de color en el volumen
    /// </summary>
    public void LookForColor()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            //Debug.Log("ColorAdjustments encontrado");
        }
    }

    /// <summary>
    /// Habilitar la parada del tiempo
    /// </summary>
    private IEnumerator EnableTimeStop()
    {
        stopped = true;
        while (colorAdjustments.saturation.value > minHue)
        {
            yield return new WaitForEndOfFrame();
            var newHue = Mathf.Max(colorAdjustments.saturation.value - hueShift, minHue);
            colorAdjustments.saturation.value = newHue;
            GameManager.Instance.GlobalSpeed = (newHue + 100f) / 200f;;
        }
        shifRoutine = null;
    }
    
    /// <summary>
    /// Deshabilitar la parada del tiempo
    /// </summary>
    private IEnumerator DisableTimeStop()
    {
        stopped = false;
        while (colorAdjustments.saturation.value < maxHue)
        {
            yield return new WaitForEndOfFrame();
            var newHue = Mathf.Min(colorAdjustments.saturation.value + hueShift, maxHue);
            colorAdjustments.saturation.value = newHue;
            GameManager.Instance.GlobalSpeed = (newHue + 100f) / 200f;
        }
        shifRoutine = null;
    }
}
