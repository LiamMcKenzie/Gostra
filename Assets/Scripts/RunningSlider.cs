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
    //The starting slider speed. Current slider speed is also kept in this variable
    private float sliderSpeed = 0.01f;
    //How often the slider speed increases in seconds
    private const float TIMEINCREMENTS = 1f;
    //How much the slider speed increases by
    private const float SPEEDINCREASE = 0.001f;

    //The target position of the slider. 1f is at the top and 0f is at the bottom
    public GameObject Target;

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
        placeTarget(.8f); //Hardcoding for now
        StartCoroutine(Movement());
        StartCoroutine(IncreaseSpeed());
    }

    private void placeTarget(float targetPos)
    {
        //Height of the slider
        float height = GetComponent<RectTransform>().rect.height;
        //Height of the target
        float targetHeight = Target.GetComponent<RectTransform>().rect.height;

        //This removes the height of the target so it doesn't go halfway off the slider
        height -= targetHeight;
        //To get the position I need to multiply the target position by the height of the slider so that I get the scale along it
        //and then subtract the height of the slider divided by 2 because half of the slider is in negative numbers (100 length slider is from 50 to -50)

        float position = (targetPos * height) - height / 2;

        // Set the targets position
        Target.transform.localPosition = new Vector3(Target.transform.localPosition.x, position, Target.transform.localPosition.z);
    }

    /// <summary>
    /// IEnumerator that runs forever and increases the speed of the slider
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseSpeed()
    {
        do
        {
            yield return new WaitForSeconds(TIMEINCREMENTS);
            sliderSpeed += SPEEDINCREASE;
            Debug.Log(sliderSpeed);
        } while (true);
    }

    /// <summary>
    /// IEnumerator that runs forever and moves the slider up and down
    /// </summary>
    /// <returns></returns>
    private IEnumerator Movement()
    {
        do
        {
            placeTarget();
            if (increasing)
            {
                slider.value += sliderSpeed;
                if (slider.value >= 1)
                {
                    increasing = false;
                }
            }
            else
            {
                slider.value -= sliderSpeed;
                if (slider.value <= 0)
                {
                    increasing = true;
                }
            }
            yield return new WaitForSeconds(0.01f);
        } while (true);
    }

    public void RunningInteract()
    {
        
    }
}
