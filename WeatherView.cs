using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Класс WeatherView
///  Содержит визуальные элементы погодной системы, которые могут изменяться.
///  К ним относятся: скайбоксы, частицы (дождь, снег), источники света.
///  Также этот класс отвечает за перемещение этих объектов вслед за игроком.
/// </summary>

public class WeatherView : MonoBehaviour
{
    // Объекты погоды
    [Header("Скайбоксы")]
    public MeshRenderer CloudsSkybox;
    public GameObject FonCity;

    [Header("Частицы")]
    public ParticleSystem RainParticles;
    public ParticleSystem SnowParticles;

    [Header("Освещение")]
    public Light MainLight;
    public Light ZakatLight;
    public Light NightLight;
    public Light NightRoadLight;
    public Light SkyboxLight;

    [Header("Для перемещения за игроком")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform lightsTransform;
    [SerializeField] private Transform skyboxesTransform;
    [SerializeField] private Transform particlesTransform;

    private void Update()
    {
        if (playerTransform != null)
            MoveObjects();
    }

    private void MoveObjects()
    {
        skyboxesTransform.SetPositionAndRotation(playerTransform.position, Quaternion.Euler(Vector3.zero));
        lightsTransform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
        particlesTransform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
    }
}
