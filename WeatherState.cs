using System.Collections;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
///   ласс WeatherState
///  Ѕазовый класс состо€ний дл€ погодной стейт машины.
///  —одержит метод SetState, который запускает корутину плавного изменени€ параметров погоды.
/// </summary>

public abstract class WeatherState : MonoBehaviour
{
    public WeatherType type;
    [SerializeField] protected WeatherStateMachine stateMachine;
    [ReadOnly] public float currentValue = 0;

    public void SetState(float value, float speed)
    {
        StartCoroutine(ChangeStateCorutine(value, speed));
    }

    protected IEnumerator ChangeStateCorutine(float value, float speed)
    {
        float currentCount = 0;
        float startValue = currentValue;
        while (currentCount < 1)
        {
            currentCount = currentCount + 0.01f;
            currentValue = Mathf.Lerp(startValue, value, currentCount);
            ChangeValues(currentValue);
            yield return new WaitForSeconds(speed / 100);
        }
        yield return null;
    }

    abstract protected void ChangeValues(float value);
}
