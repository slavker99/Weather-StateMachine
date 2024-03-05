using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ����� WeatherView
///  �������� ���������� �������� �������� �������, ������� ����� ����������.
///  � ��� ���������: ���������, ������� (�����, ����), ��������� �����.
///  ����� ���� ����� �������� �� ����������� ���� �������� ����� �� �������.
/// </summary>

public class WeatherView : MonoBehaviour
{
    // ������� ������
    [Header("���������")]
    public MeshRenderer CloudsSkybox;
    public GameObject FonCity;

    [Header("�������")]
    public ParticleSystem RainParticles;
    public ParticleSystem SnowParticles;

    [Header("���������")]
    public Light MainLight;
    public Light ZakatLight;
    public Light NightLight;
    public Light NightRoadLight;
    public Light SkyboxLight;

    [Header("��� ����������� �� �������")]
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
