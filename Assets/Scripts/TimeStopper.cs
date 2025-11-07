using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class TimeStopper : MonoBehaviour
{
    [Tooltip("Referencia a Global Volume")] public Volume volume;
    [Tooltip("Referencia al ajuste de color")] public ColorAdjustments colorAdjustments;
    [Tooltip("Saturación mínima")] public float minHue = -100f;
    [Tooltip("Saturación máxima")] public float maxHue = 100f;
    [Tooltip("Velocidad de cambio de saturación")][Range(0.001f, 100)] public float hueShift = 5f;

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

    private void OnDisable()
    {
        if (shifRoutine != null) StopCoroutine(shifRoutine);
    }

    public void LookForColor()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            //Debug.Log("ColorAdjustments encontrado");
        }
    }

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
