using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
///  Класс WeatherStateMachine
///  Основной класс погодной системы.
///  Содержит состояния погоды и изменяет их.
/// </summary>

public class WeatherStateMachine : MonoBehaviour
{
    [Header("Îáúåêòû ïîãîäû")]
    public WeatherView View;

    [Header("Ñîñòîÿíèÿ")]
    [SerializeField] private List<WeatherState> states;
    private List<WeatherState> currentStates;
    private Dictionary<WeatherType, WeatherState> statesDictionary = new Dictionary<WeatherType, WeatherState>();

    private void Awake()
    {
        foreach (var state in states)
            statesDictionary.Add(state.type, state);
    }

    private void Start()
    {
        StartCoroutine(ChangingWeatherCorutine());
    }

    public void SetState(WeatherType weather, float value, float speed)
    {
        statesDictionary[weather].SetState(value, speed);
    }

    private IEnumerator ChangingWeatherCorutine()
    {
        SetState(WeatherType.Rain, 0, 0.5f);
        SetState(WeatherType.Night, 0, 0.5f);
        yield return new WaitForSeconds(10);
        while (true)
        {
            SetState(WeatherType.Rain, 1f, 10);
            yield return new WaitForSeconds(40);
            SetState(WeatherType.Rain, 0, 10);
            yield return new WaitForSeconds(40);

            SetState(WeatherType.Night, 1, 20);
            yield return new WaitForSeconds(40);
            SetState(WeatherType.Night, 0, 20);
            yield return new WaitForSeconds(40);
        }

        yield return null;
    }
}

public enum WeatherType
{
    Default,
    Rain,
    Snow,
    Night
}
