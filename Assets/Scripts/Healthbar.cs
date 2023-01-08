using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : Health
{
    public Slider slider;

    private void Start()
    {
        SetMaxHealthBar(maxHealth);
    }

    public void SetMaxHealthBar(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealthBar(int health)
    {
        slider.value = health; 
    }
}
