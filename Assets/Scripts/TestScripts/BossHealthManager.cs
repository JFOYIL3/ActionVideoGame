using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{

    [SerializeField] public Slider bossHealthSlider;
    [SerializeField] public Slider damageHealthSlider;

    // Update is called once per frame
    void Update()
    {
        // test script to see if it works
        if (Input.GetKeyDown("space"))
        {
            bossHealthSlider.value -= .10f;
        }

        UpdateDamagedHealth();
    }

    void UpdateDamagedHealth()
    {
        if (damageHealthSlider.value > bossHealthSlider.value)
        {
            damageHealthSlider.value -= .001f;
        }
    }
}
