using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycleManager : MonoBehaviour
{
    public static DayNightCycleManager current;

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

                float timeIntervalBetweenSpawn = numberOfSecondsInOneDay / (numberOfEnemieToSpawnPerDay[_currentDay - 1] + 1);
                InvokeRepeating("SpawnRepeating", timeIntervalBetweenSpawn, timeIntervalBetweenSpawn);

                if (onDayStart != null)
                    onDayStart.Invoke();
            } else if (!_isItDay)
            {
                _changingTime = Time.time + numberOfSecondsInOneNight;
                CancelInvoke();
                _numberEnemieSpawned = 0;
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
}
