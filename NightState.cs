using System;
using System.Collections;
using UnityEngine;

/// <summary>
///   ласс NightState
///  Ќаследник класса WeatherState
///  —одержит параметры элементов погоды, которые должны измен€тьс€ дл€ перехода в ночное состо€ние погоды.
///  »зменение происходит через выполнение метода ChangeValues в корутине базового класса
/// </summary>
/// 
public class NightState : WeatherState
{
    private Light mainLight;
    [SerializeField] private float mainLightDefVal = 4;
    [SerializeField] private float mainLightFinVal = 0;
    [SerializeField] private float mainLightDefAngleX = 10;
    [SerializeField] private float mainLightFinAngleX = -10;

    private Light nightLight;
    [SerializeField] private float nightLightDefVal = 0;
    [SerializeField] private float nightLightFinVal = 8;

    private Light nightRoadLight;
    [SerializeField] private float nightRoadLightDefVal = 0;
    [SerializeField] private float nightRoadLightFinVal = 0.4f;

    private Light skyboxLight;
    [SerializeField] private float skyboxLightDefVal = 0;
    [SerializeField] private float skyboxLightFinVal = 1f;

    private Light zakatLight;
    [SerializeField] private float zakatLightDefPosY = 209;
    [SerializeField] private float zakatLightFinPosY = -209;

    [SerializeField] private Color globalIntensityDefColor;
    [SerializeField] private Color globalIntensityFinColor;

    private void Awake()
    {
        currentValue = 0;
        if (base.stateMachine != null)
        {
            mainLight = base.stateMachine.View.MainLight;
            nightLight = base.stateMachine.View.NightLight;
            nightRoadLight = base.stateMachine.View.NightRoadLight;
            skyboxLight = base.stateMachine.View.SkyboxLight;
            zakatLight = base.stateMachine.View.ZakatLight;
        }
    }

    protected override void ChangeValues(float value)
    {
        //mainLight.intensity = (float)Math.Round(Mathf.Lerp(mainLightDefVal, mainLightFinVal, value), 2);
        if (value < 0.6)
            mainLight.intensity = mainLightDefVal;
        else
            mainLight.intensity = mainLightFinVal;
        nightLight.intensity = (float)Math.Round(Mathf.Lerp(nightLightDefVal, nightLightFinVal, value), 2);
        nightRoadLight.intensity = (float)Math.Round(Mathf.Lerp(nightRoadLightDefVal, nightRoadLightFinVal, value), 2);
        skyboxLight.intensity = (float)Math.Round(Mathf.Lerp(skyboxLightDefVal, skyboxLightFinVal, value), 2);
        RenderSettings.ambientLight = Color.Lerp(globalIntensityDefColor, globalIntensityFinColor, value);
        zakatLight.transform.localPosition = new Vector3(
            zakatLight.transform.localPosition.x, 
            Mathf.Lerp(zakatLightDefPosY, zakatLightFinPosY, value), 
            zakatLight.transform.localPosition.z);

        mainLight.transform.localRotation = Quaternion.Euler(
            Mathf.Lerp(mainLightDefAngleX, mainLightFinAngleX, value),
            mainLight.transform.localEulerAngles.y, 
            mainLight.transform.localEulerAngles.z);

    }
}
