using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Planet : MonoBehaviour
{
    public float Mass;
    
    public float DistanceFromTheSun;
    [Header("(milyon kilometre cinsinden)")]
    public float DistanceFromTheSunInAu;
    public float Volume;
    public float OneDayPeriod;
    public GameObject Star;
    public GameObject Manager;
    float FastForwardMultiplier;
    public float timepast;
    public float trueangleindegrees;

    float MassOfStar;
    public float angle;
    float G = 6.67f * 10e-12f;
    public float angularSpeed;
    void Start()
    {
        SetPresetValues();
    }

    void Update()
    {
        UpdatePresetValues();
        CircularMotion();
    }


    void SetPresetValues()
    {
        FastForwardMultiplier = Manager.GetComponent<SolarSystemManager>().FastForwardMultiplier;
        if (DistanceFromTheSun!=0)
        {
            DistanceFromTheSunInAu = DistanceFromTheSun / 1.496e+2f;
        }
        MassOfStar = Star.GetComponent<Star>().Mass;
        angularSpeed = Mathf.Sqrt(G * MassOfStar / Mathf.Pow(DistanceFromTheSun * 10e+8f, 3f));
    }
    void UpdatePresetValues()
    {
        FastForwardMultiplier = Manager.GetComponent<SolarSystemManager>().FastForwardMultiplier;
        DistanceFromTheSunInAu = DistanceFromTheSun / 1.496e+2f;
        angularSpeed = Mathf.Sqrt(G * MassOfStar / Mathf.Pow(DistanceFromTheSun * 10e+8f, 3f));
        MassOfStar = Star.GetComponent<Star>().Mass;
        timepast = Time.time;
    }

    void CircularMotion()
    {
        angle += angularSpeed * Time.deltaTime * FastForwardMultiplier;
        transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * DistanceFromTheSun;
        trueangleindegrees = (angle / Mathf.PI * 180) % 360;
    }

}
