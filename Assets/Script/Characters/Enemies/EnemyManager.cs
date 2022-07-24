using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance = null;

    public static EnemyManager Inst { get => instance; }

    private ShotgunKin shotgunKin_Red;
    public ShotgunKin ShotgunKin_Red { get => shotgunKin_Red; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        shotgunKin_Red = FindObjectOfType<ShotgunKin>();
    }
}
