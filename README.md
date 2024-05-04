  ## Описание

  Мне нужно было сделать простую погодную систему, которая могла бы плавно менять погоду или время суток. В планах иметь в игре только 3 состояния погоды: дождь, снег и ночь. Состояния не будут накладываться друг на друга: в конкретный момент возможно только одно состояние погоды.

  ## Структура

  Главный класс системы: WeatherStateMachine, он содержит ссылки на состояния и способен управлять их переходами с помощью метода SetState().

  Состояния реализованы через абстрактный класс WeatherState. Наследуюясь от этого класса, любое состояние получает возможность иметь значение его состояния (0 - состояние неактивно, 1 - активно полностью). За счёт этого можно плавно изменять значение, прибавляя дробные значения величины (например при значении 0.5 состояние будет активно лишь наполовину). Также имеется возможность выбирать скорость изменения состояния.

  Класс WeatherView хранит ссылки на объекты сцены, которые должны изменяться: скайбоксы, источники света, погодные частицы.

  **UML-диаграмма классов**
  ![UML-диаграмма классов](https://github.com/slavker99/Weather-StateMachine/blob/main/WeatherStateMachine(1).drawio.png?raw=true)

  ## Реализация

    (Общий скрин игрока)

  Работа погодной системы начинается с объекта класса WeatherStateMachine. Он должен иметь ссылку на объект WeatherView, а также на объекты состояний: NightState, RainState и др. Для хранения состояний используется словарь statesDictionary.
  Для перехода в состояние используется метод SetState, который принимает аргументами тип состояния, целевое значение состояния и скорость его изменения.

  ```c#
  public void SetState(WeatherType weather, float targetValue, float speed)
  {
      statesDictionary[weather].SetState(targetValue, speed);
  }
  ```

  Класс WeatherState также имеет метод SetState. При его вызове запускается корутина, в результате работы которой меняется состояние.

  ```c#
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
            currentValue = (float)System.Math.Round(Mathf.Lerp(startValue, value, currentCount), 2);
            yield return StartCoroutine(ChangeValuesCorutine(currentValue));
            yield return new WaitForSeconds(speed / 100);
        }
        yield return null;
    }
  ```

  Корутина ChangeStateCorutine запускает цикл, который за несколько срабатываний меняет состояние погоды. Изменение значений, таких как яркость солнца, прозрачность облаков и т.д., изменяются в абстрактной  корутине ChangeValuesCorutine, метод которой реализуется в классах наследниках. Вот, например, реализация в классе NightState:

  ```c#
   protected override IEnumerator ChangeValuesCorutine(float value)
   {
       NightEvent.Invoke(value);
       if (value == 0.6f)
       {
           isNight = !isNight;
           SimpleNightEvent.Invoke(isNight);
           Debug.Log("Ивент SimpleNight: " + isNight);
           stateMachine.View.Flare.enabled = !isNight;
       }
       yield return new WaitForFixedUpdate();
       mainLight.intensity = (float)Math.Round(Mathf.Lerp(mainLightDefVal, mainLightFinVal, value), 2);
       yield return new WaitForFixedUpdate();
       skyboxZakatLight.intensity = (float)Math.Round(Mathf.Lerp(skyboxZakatDefVal, skyboxZakatFinVal, value), 2);
       yield return new WaitForFixedUpdate();
       nightRoadLight.intensity = (float)Math.Round(Mathf.Lerp(nightRoadLightDefVal, nightRoadLightFinVal, value), 2);
       yield return new WaitForFixedUpdate();
       skyboxLight.intensity = (float)Math.Round(Mathf.Lerp(skyboxLightDefVal, skyboxLightFinVal, value), 2);
       yield return new WaitForFixedUpdate();
       RenderSettings.ambientLight = Color.Lerp(globalIntensityDefColor, globalIntensityFinColor, value);
       yield return new WaitForFixedUpdate();
       fonCityMt.color = Color.Lerp(fonCityDefColor, fonCityFinColor, value);
       yield return new WaitForFixedUpdate();
       sun.transform.localPosition = new Vector3(
           sun.transform.localPosition.x,
           Mathf.Lerp(sunDefPosY, sunFinPosY, value),
           sun.transform.localPosition.z);
       yield return new WaitForFixedUpdate();
       mainLight.transform.localRotation = Quaternion.Euler(
           Mathf.Lerp(mainLightDefAngleX, mainLightFinAngleX, value),
           mainLight.transform.localEulerAngles.y,
           mainLight.transform.localEulerAngles.z);
       yield return new WaitForFixedUpdate();
       var color = cloudsSkybox.material.color;
       cloudsSkybox.material.color = new Color(color.r, color.g, color.b, Mathf.Lerp(cloudsDefTransp, cloudsFinalTransp, value));
       yield return new WaitForFixedUpdate();
       foreach (var mt in stateMachine.View.NightMaterials)
       {
           ChangeTransparent(mt, value);
       }
       yield return null;
   }
  ```

  Использование корутины здесь обусловлено оптимизацией, поскольку так каждое изменение происходит с задержкой в один кадр, что снижает нагрузку на видеопроцессор.
  
  Для плавной смены погоды используются такие параметры, как:

  + Прозрачность материалов (скайбоксов).
  + Интенсивность источников света (Light.Intensity).
  + Глобальное освещение сцены (RenderSettings.ambientLight).
  + Количество частиц осадков (emmissionRate).
  + Положение объекта солнца на сцене.


  ## Недостатки системы и возможные улучшения

  На данный момент система не может накладывать сразу два состояния погоды (например дождь во время ночи). Это обусловлено тем, что некоторые состояния имеют общие объекты, на которые они воздействуют (например, состояние Ночь и Дождь меняют один и тот же параметр яркости освещения). Также пока нет необходимости этого делать с точки зрения геймплея, потому что меня устраивало такое поведение погоды. Но как вариант, я рассматриваю модернизацию текущей системы для улучшения реалистичности погоды. Для этого я планирую сделать состояние День/Ночь независимым от остальных параметров, чтобы ночь могла наступать совместно с дождём, снегом и т.д. Также планирую добавить и другие состояния, например Ветер и Туман.

  


