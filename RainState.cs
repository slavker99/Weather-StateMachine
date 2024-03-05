using System;
using UnityEngine;
using System.Collections;

/// <summary>
///  Класс RainState
///  Наследник класса WeatherState
///  Содержит параметры элементов погоды, которые должны изменяться для перехода в состояние дождя.
///  Изменение происходит через выполнение метода ChangeValues в корутине базового класса
/// </summary>
/// 
public class RainState : WeatherState
{
    private Light mainLight;
    [SerializeField] private float mainLightDefVal = 8;
    [SerializeField] private float mainLightFinalVal = 0;

    [SerializeField] private Color mainLightDefColor;
    [SerializeField] private Color mainLightFinColor;

    private ParticleSystem rainParticles;
    [SerializeField] private float particlesDefCount = 0;
    [SerializeField] private float particlesFinalCount = 1000;

    private MeshRenderer cloudsSkybox;
    [SerializeField] private float cloudsDefTransp = 0;
    [SerializeField] private float cloudsFinalTransp = 1;


    private void Awake()
    {
        if (base.stateMachine != null)
        {
            mainLight = base.stateMachine.View.MainLight;
            rainParticles = base.stateMachine.View.RainParticles;
            cloudsSkybox = base.stateMachine.View.CloudsSkybox;
        }
    }

    protected override void ChangeValues(float value)
    {
        mainLight.intensity = (float)Math.Round(Mathf.Lerp(mainLightDefVal, mainLightFinalVal, value), 2);
        mainLight.color = Color.Lerp(mainLightDefColor, mainLightFinColor, value);

        rainParticles.emissionRate = (float)Math.Round(Mathf.Lerp(particlesDefCount, particlesFinalCount, value), 2);

        var color = cloudsSkybox.material.color;
        var cloudsCurState = Mathf.Lerp(cloudsDefTransp, cloudsFinalTransp, value);
        cloudsSkybox.material.color = new Color(color.r, color.g, color.b, cloudsCurState);
    }
}
