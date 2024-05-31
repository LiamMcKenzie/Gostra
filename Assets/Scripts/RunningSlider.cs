/// <summary>
/// Code for the running meter
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningSlider : MonoBehaviour
{
    private Slider slider;
    //If the slider is increasing or decreasing
    private bool increasing = true;
    //The starting speed. Current speed is also kept in this variable
    private float speed = 0.01f;
    //How often the speed increases in seconds
    private const float TIMEINCREMENTS = 1f;
    //How much the speed increases by
    private const float SPEEDINCREASE = 0.001f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get the slider component attached to this gameobject
        try
        {
            slider = GetComponent<Slider>();
            slider.value = 0;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        StartCoroutine(Movement());
        StartCoroutine(IncreaseSpeed());
    }

    private IEnumerator IncreaseSpeed()
    {
        do
        {
            yield return new WaitForSeconds(TIMEINCREMENTS);
            speed += SPEEDINCREASE;
            Debug.Log(speed);
        } while (true);
    }

    private IEnumerator Movement()
    {
        do
        {
            if (increasing)
            {
                slider.value += speed;
                if (slider.value >= 1)
                {
                    increasing = false;
                }
            }
            else
            {
                slider.value -= speed;
                if (slider.value <= 0)
                {
                    increasing = true;
                }
            }
            yield return new WaitForSeconds(0.01f);
        } while (true);
    }
}
