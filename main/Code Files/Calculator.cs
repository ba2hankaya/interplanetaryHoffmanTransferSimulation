using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    public GameObject O_Star;
    public GameObject O_Planet1;
    public GameObject O_Planet2;
    public GameObject O_Manager;
    Star Star;
    Planet Planet1;
    Planet Planet2;
    SolarSystemManager SolarSystem;
    float M;
    float G;

    float m1;
    float R1;
    float T1;

    float m2;
    float R2;
    float T2;

    float R1au;
    float R2au;

    float anglep1;
    float anglep2;

    [Header("Transfer Degerleri")]
    [Header("mesafe birimleri km, sure birimleri saniye")]
    public float a_transfer;
    public float e_transfer;
    public float vp1_transfer;
    public float vp1;
    public float vp1_infinity;
    public float vp2_transfer;
    public float vp2;
    public float vp2_infinity;

    [Header("P1 Senkronize Yorunge")]
    public float Rp1_senkronize_yorunge;
    public float v_p1_senkronize_yorunge;

    [Header("P1 Hiperbol Hizi Degerleri")]
    public float e_sonsuz_p1;
    public float v_p1_hiperbol_hiz;

    [Header("Delta V Cikis")]
    public float delta_v_cikis;

    [Header("P2 Senkronize Yorunge")]
    public float Rp2_senkronize_yorunge;
    public float v_p2_senkronize_yorunge;

    [Header("P2 Hiperbol Hizi Degerleri")]
    public float e_sonsuz_p2;
    public float v_p2_hiperbol_hiz;

    [Header("Delta V Giris")]
    public float delta_v_giris;

    [Header("Delta V")]
    public float DeltaV;

    [Header("Angle Between The Planets")]
    public float AngleBetweenThePlanets;

    [Header("Angle For Transfer")]
    public float AngleForTransfer;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void GetValues()
    {
        Star = O_Star.GetComponent<Star>();
        Planet1 = O_Planet1.GetComponent<Planet>();
        Planet2 = O_Planet2.GetComponent<Planet>();
        SolarSystem = O_Manager.GetComponent<SolarSystemManager>();
        M = Star.Mass;
        G = 6.67e-20f;
        m1 = Planet1.Mass;
        m2 = Planet2.Mass;
        R1 = Planet1.DistanceFromTheSun*1e+6f;
        R2 = Planet2.DistanceFromTheSun*1e+6f;
        R1au = Planet1.DistanceFromTheSunInAu;
        R2au = Planet2.DistanceFromTheSunInAu;
        T1 = Planet1.OneDayPeriod;
        T2 = Planet2.OneDayPeriod;
        anglep1 = Planet1.trueangleindegrees;
        anglep2 = Planet2.trueangleindegrees;
    }
    // Update is called once per frame
    void Update()
    {
        GetValues();
        TransferCalculations();
        DepartureCalculations();
        ArrivalCalculations();
        AngleCalculations();
        DeltaV = delta_v_cikis + delta_v_giris;
    }


    void TransferCalculations()
    {
        a_transfer = (R1 + R2) / 2;
        e_transfer = -G * M / (2 * a_transfer);
        vp1_transfer = Mathf.Sqrt(2 * (G * M / R1 + e_transfer));
        vp1 = Mathf.Sqrt(G * M / R1);
        vp1_infinity = Mathf.Abs(vp1_transfer - vp1);

        vp2_transfer = Mathf.Sqrt(2 * (G * M / R2 + e_transfer));
        vp2 = Mathf.Sqrt(G * M / R2);
        vp2_infinity = Mathf.Abs(vp2_transfer - vp2);
    }

    void DepartureCalculations()
    {
        Rp1_senkronize_yorunge = Mathf.Pow(m1 * G * Mathf.Pow(T1, 2f) / (4 * Mathf.Pow(Mathf.PI,2f)), 0.3333f);
        v_p1_senkronize_yorunge = Mathf.Sqrt(m1*G/Rp1_senkronize_yorunge);
        e_sonsuz_p1 = Mathf.Pow(vp1_infinity, 2f) / 2f;
        v_p1_hiperbol_hiz = Mathf.Sqrt(2*(G*m1/Rp1_senkronize_yorunge+e_sonsuz_p1));
        delta_v_cikis = Mathf.Abs(v_p1_hiperbol_hiz-v_p1_senkronize_yorunge);
    }

    void ArrivalCalculations()
    {
        Rp2_senkronize_yorunge = Mathf.Pow(m2 * G * Mathf.Pow(T2, 2f) / (4 * Mathf.Pow(Mathf.PI, 2f)), 0.3333f);
        v_p2_senkronize_yorunge = Mathf.Sqrt(m2 * G / Rp2_senkronize_yorunge);
        e_sonsuz_p2 = Mathf.Pow(vp2_infinity, 2f) / 2f;
        v_p2_hiperbol_hiz = Mathf.Sqrt(2 * (G * m2 / Rp2_senkronize_yorunge + e_sonsuz_p2));
        delta_v_giris = Mathf.Abs(v_p2_hiperbol_hiz - v_p2_senkronize_yorunge);
    }

    void AngleCalculations()
    {
        float anglediff = Mathf.Abs(anglep1 - anglep2);
        if(anglediff > 180) 
        {
            AngleBetweenThePlanets = 360 - anglediff;
        }else if (anglediff < 180)
        {
            AngleBetweenThePlanets = anglediff;
        }
        AngleForTransfer = 180 - (vp2 * Mathf.Sqrt(Mathf.Pow( ((R1au+R2au)/2f), 3f)) * 365 * 24 * 3600) / (R2 * 2)/Mathf.PI*180;
    }
}
