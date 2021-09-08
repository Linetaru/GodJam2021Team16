using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycleManager : MonoBehaviour
{
    public static DayNightCycleManager current;

    [Header("References")]
    [SerializeField] private Light2D dayLight;

    [Header("Day Settings")]
    [SerializeField] float numberOfSecondsInOneDay;
    // summon a total of X enemie for N day
    [SerializeField] List<int> numberOfEnemieToSpawnPerDay;

    [Header("Night Settings")]
    [SerializeField] float numberOfSecondsInOneNight;

    [Header("Events")]
    [SerializeField] private UnityEvent onDayStart;
    [SerializeField] private UnityEvent onNightStart;

    private int _currentDay = 1;
    private float _currentTime = 0f;
    private float _changingTime = 0f;
    private bool _isItDay = true;
    private int _numberEnemieSpawned = 0;
    private SpawnerManager _spawnerManager;

    private void Awake()
    {
        current = this;
        _spawnerManager = SpawnerManager.current;
    }

    void Start()
    {
        _currentTime = Time.time;
        _changingTime = Time.time + numberOfSecondsInOneDay;
        float timeIntervalBetweenSpawn = numberOfSecondsInOneDay / (numberOfEnemieToSpawnPerDay[_currentDay - 1] + 1);
        InvokeRepeating("SpawnRepeating", timeIntervalBetweenSpawn, timeIntervalBetweenSpawn);
    }

    public int getCurrentDay()
    {
        return _currentDay;
    }

    public float getCurrentTime()
    {
        return _currentTime;
    }

    public bool isDay()
    {
        return _isItDay;
    }

    void Update()
    {
        _currentTime = Time.time;
        if (_currentTime >= _changingTime)
        {
            _isItDay = !_isItDay;
            if (_isItDay)
            {
                _currentDay++;
                _changingTime = Time.time + numberOfSecondsInOneDay;

                InvokeRepeating("AugmentLight", 0, 0.1f);

                float timeIntervalBetweenSpawn = numberOfSecondsInOneDay / (numberOfEnemieToSpawnPerDay[_currentDay - 1] + 1);
                InvokeRepeating("SpawnRepeating", timeIntervalBetweenSpawn, timeIntervalBetweenSpawn);

                if (onDayStart != null)
                    onDayStart.Invoke();
            } else if (!_isItDay)
            {
                _changingTime = Time.time + numberOfSecondsInOneNight;
                print("ITS the start of night!!!");
                
                InvokeRepeating("ReduceLight", 0, 0.1f);
                if (onNightStart != null)
                    onNightStart.Invoke();
            }
        }
    }

    void SpawnRepeating()
    {
        if (_numberEnemieSpawned < numberOfEnemieToSpawnPerDay[_currentDay - 1])
        {
            _spawnerManager.MakeSpawnerSpawnAEntity();
            _numberEnemieSpawned++;
        }
    }

    void ReduceLight()
    {
        dayLight.intensity -= 0.1f;
        if (dayLight.intensity <= 0f)
        {
            dayLight.intensity = 0f;
            CancelInvoke();
            _numberEnemieSpawned = 0;
        }
    }

    void AugmentLight()
    {
        dayLight.intensity += 0.1f;
        if (dayLight.intensity >= 1)
        {
            dayLight.intensity = 1f;
        }
    }
}
