using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using PackageCreator.Event;

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
    [SerializeField] private GameEvent onDayStart;
    [SerializeField] private GameEvent onNightStart;

    private int _currentDay = 1;
    private float _currentTime = 0f;
    private float _changingTime = 0f;
    private bool _isItDay = true;
    private int _numberEnemieSpawned = 0;
    private int _lastValidDay = 0;
    private SpawnerManager _spawnerManager;
    private int _nbOfEnemieForTheDay;

    private void Awake()
    {
        current = this;
        _spawnerManager = SpawnerManager.current;
    }

    void Start()
    {
        _currentTime = Time.time;
        _changingTime = Time.time + numberOfSecondsInOneDay;
        _nbOfEnemieForTheDay = numberOfEnemieToSpawnPerDay[_currentDay - 1];
        float timeIntervalBetweenSpawn = numberOfSecondsInOneDay / (_nbOfEnemieForTheDay + 1);
        InvokeRepeating("SpawnRepeating", timeIntervalBetweenSpawn, timeIntervalBetweenSpawn);
        print("Enemy for the day : " + _nbOfEnemieForTheDay);
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

                _nbOfEnemieForTheDay = 5;
                if (_currentDay > numberOfEnemieToSpawnPerDay.Count)
                {
                    _nbOfEnemieForTheDay = numberOfEnemieToSpawnPerDay[_lastValidDay] 
                        + (numberOfEnemieToSpawnPerDay[_lastValidDay] 
                        - ((_lastValidDay - 1) < 0 
                        ? 0 
                        : numberOfEnemieToSpawnPerDay[_lastValidDay - 1]));
                } else
                {
                    _lastValidDay = _currentDay - 1;
                    _nbOfEnemieForTheDay = numberOfEnemieToSpawnPerDay[_lastValidDay];
                }


                print("Enemy for the day : " + _nbOfEnemieForTheDay);

                float timeIntervalBetweenSpawn = numberOfSecondsInOneDay / (_nbOfEnemieForTheDay + 1);
                InvokeRepeating("SpawnRepeating", timeIntervalBetweenSpawn, timeIntervalBetweenSpawn);

                if (onDayStart != null)
                    onDayStart.Raise();
            } else if (!_isItDay)
            {
                _changingTime = Time.time + numberOfSecondsInOneNight;
                
                InvokeRepeating("ReduceLight", 0, 0.1f);
                if (onNightStart != null)
                    onNightStart.Raise();
            }
        }
    }

    void SpawnRepeating()
    {
        if (_numberEnemieSpawned < _nbOfEnemieForTheDay)
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
