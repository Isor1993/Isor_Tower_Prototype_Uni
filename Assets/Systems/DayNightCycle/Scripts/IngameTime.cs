/*****************************************************************************
* Project : Isors Tower Prototype
* File    : IngameTime.cs
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

public class IngameTime : MonoBehaviour
{

    public static IngameTime Instance { get; private set; }

    /// <summary>
    /// Get the seconds from IngameTime
    /// </summary>
    public int Seconds => _currentSeconds;

    /// <summary>
    /// Get the minutes from IngameTime
    /// </summary>
    public int Minutes => _currentMinutes;

    /// <summary>
    /// Get the hours from IngameTime
    /// </summary>
    public int Hours => _currentHours;

    /// <summary>
    /// Get the days from IngameTime
    /// </summary>
    public int Days => _currentDays;

    /// <summary>
    /// 
    /// </summary>
    public int SecondsPerDay =>
     _timeConversionRateHours *
     _timeConversionRateMinutes *
     _timeConversionRateSeconds;

    /// <summary>
    /// 
    /// </summary>
    public int CurrentTotalSeconds =>
    _currentHours * _timeConversionRateMinutes * _timeConversionRateSeconds +
    _currentMinutes * _timeConversionRateSeconds +
    _currentSeconds;

    /// <summary>
    /// TimeScale is the multiplicator for the speed of time
    /// </summary>
    public float TimeScale
    {
        get => _timeScale;
        set => _timeScale = Mathf.Max(0f, value);
    }

    [Header("Time Settings")]

    [Range(0.01f, 100f)]
    [Tooltip("How many real-time seconds equal one in-game second")]
    [SerializeField] private float _realSecondsPerIngameSecond = 1f;
    [Range(1, 100000)]
    [Tooltip("How many ingame seconds equal 1 ingame minute")]
    [SerializeField] private int _timeConversionRateSeconds = 60;
    [Range(1, 100000)]
    [Tooltip("How many ingame minutes equal 1 ingame hour")]
    [SerializeField] private int _timeConversionRateMinutes = 60;
    [Range(1, 100000)]
    [Tooltip("How many ingame hours equal 1 ingame day")]
    [SerializeField] private int _timeConversionRateHours = 24;

    private int _currentSeconds;
    private int _currentMinutes;
    private int _currentHours;
    private int _currentDays;
    private int _lastCurrentSecond;

    private float _elapsedTime;
    // TODO später in testing tool
    [SerializeField] private float _timeScale;

    //TODO Later auslagern
    [SerializeField] private bool _isPlaying=true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _elapsedTime = 0f;
        if (_timeScale <= 0f)
        {
            TimeScale = 1f;
        }
        LoadSavedTime();
    }

    private void Update()
    {
        if (!_isPlaying)
        {
            return;
        }
        _elapsedTime += Time.deltaTime * _timeScale;

        while (_elapsedTime >= _realSecondsPerIngameSecond)
        {
            _currentSeconds += 1;
            _elapsedTime -= _realSecondsPerIngameSecond;
        }

        ConvertTimeUnit(ref _currentSeconds, _timeConversionRateSeconds, ref _currentMinutes);
        ConvertTimeUnit(ref _currentMinutes, _timeConversionRateMinutes, ref _currentHours);
        ConvertTimeUnit(ref _currentHours, _timeConversionRateHours, ref _currentDays);

#if UNITY_EDITOR
        if (_currentSeconds != _lastCurrentSecond)
        {
            Debug.Log($"Day : {_currentDays} Time:[{_currentHours}:{_currentMinutes}:{_currentSeconds}]");
            _lastCurrentSecond = _currentSeconds;
        }
#endif
    }

    /// <summary>
    /// Saves the time todo: später richtig connecten und benutzen
    /// </summary>
    private void SaveTime()
    {
        //TODO
#if (UNITY_EDITOR)
        Debug.Log("Saved game !");
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentUnit"></param>
    /// <param name="conversionRate"></param>
    /// <param name="nextUnit"></param>
    private void ConvertTimeUnit(ref int currentUnit, int conversionRate, ref int nextUnit)
    {
        while (currentUnit >= conversionRate)
        {
            currentUnit -= conversionRate;
            nextUnit += 1;
        }
    }

    /// <summary>
    /// Loads the saved time
    /// </summary>
    private void LoadSavedTime()
    {
        //TODO 
#if (UNITY_EDITOR)
        Debug.Log("Load saved Time !");
#endif
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SaveTime();
            Instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        if (Instance == this)
        {
            SaveTime();
        }
    }
}