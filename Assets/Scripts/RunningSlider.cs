/// <summary>
/// Code for the running meter
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    //The range of the slider that the target can be in to speed up
    private float targetRange;

    //The target on the slider
    public GameObject Target;
    private float sliderHeight;
    private float targetHeight;
    private float targetPosition = 0.8f;

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
        //Height of the slider
        sliderHeight = GetComponent<RectTransform>().rect.height;
        //Height of the target
        targetHeight = Target.GetComponent<RectTransform>().rect.height;
        //Getting the range of the slider that the target covers
        targetRange = targetHeight / sliderHeight;
        Debug.Log(targetRange);
        placeTarget(.8f); //Hardcoding for now
        StartCoroutine(Movement());
        StartCoroutine(IncreaseSpeed());
    }

    //Placing the target on the slider
    private void placeTarget(float target)
    {
        targetPosition = target;
        //This removes the height of the target so it doesn't go halfway off the slider
        float adjHeight = sliderHeight - targetHeight;
        //To get the position I need to multiply the target position by the height of the slider so that I get the scale along it
        //and then subtract the height of the slider divided by 2 because half of the slider is in negative numbers (100 length slider is from 50 to -50)

        float position = (targetPosition * adjHeight) - adjHeight / 2;

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

    /// <summary>
    /// Called when the interact key is hit
    /// </summary>
    public void RunningInteract(InputAction.CallbackContext context)
    {
        //This is so it doesn't call it multiple times when the key is pressed
        if (!context.performed)
        {
            return;
        }
        //Gettingthe low and high ends of the target
        float low = targetPosition - targetRange / 2;
        float high = targetPosition + targetRange / 2;

        if (slider.value >= low && slider.value <= high)
        {
            Debug.Log("Succeed: " + slider.value);
        }
        else
        {
            Debug.Log("Failed: " + slider.value);
        }
    }
}
