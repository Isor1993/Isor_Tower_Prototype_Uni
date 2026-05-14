/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SkyController.cs
* Date    : xx.xx.2026
* Author  : Eric Rosenberg
*
* Description :
* 
* 
* 
*
* History :
* xx.xx.2026 ER Created
******************************************************************************/
using UnityEngine;

public class SkyController : MonoBehaviour
{
    private const float FULL_ROTATION_DEGREES = 360f;

    [Header("Settings")]
    [SerializeField] private Transform _celestialPivot;
    [SerializeField] private DayNightCycle _dayNightCycle;

    [SerializeField] private float _rotationOffset = -90f;

    [Header("Debug (Runtime)")]
    [SerializeField] private float _currentAngle;
    [SerializeField] private float _dayProgress;

    private void Update()
    {
        _dayProgress = _dayNightCycle.DayProgress;
        UpdateCelestialRotation();
    }

    private void UpdateCelestialRotation()
    {
        _currentAngle =
            (_dayProgress * FULL_ROTATION_DEGREES) +
            _rotationOffset;

        _currentAngle %= FULL_ROTATION_DEGREES;

        _celestialPivot.localRotation =
            Quaternion.Euler(-_currentAngle, 0f, 0f);
    }
}