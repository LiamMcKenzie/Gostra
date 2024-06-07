/// <remarks>
/// Author: Palin Wiseman
/// Date Created: May 31, 2024
/// Bugs: None known at this time.
/// </remarks>
/// <summary>
/// Code for the running meter
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RunningSlider : MonoBehaviour
{
    private Slider slider;
    //If the slider is increasing or decreasing
    private bool increasing = true;
    //The starting slider speed. Current slider speed is also kept in this variable
    private float sliderSpeed = 0.01f;
    //How often the slider speed increases in seconds
    private const float TIME_INCREMENTS = 1f;
    //How much the slider speed increases by
    private const float SLIDER_SPEED_INCREASE = 0.001f;
    //These two just have temporary values in them. Not sure what movement speed we will be wanting.
    private const float STARTING_MOVEMENT_SPEED = 1f;
    private const float MOVEMENT_SPEED_CHANGE = 1f;
    //The range of the slider that the target can be in to speed up
    private float targetRange;
    //The target on the slider
    [SerializeField]private GameObject Target;
    private float sliderHeight;
    private float targetHeight;
    private float targetPosition;
    //Movement speed of the player
    private float movementSpeed;
    //Public getter as the player script will need to know the movement speed
    public float MovementSpeed
    {
        get { return movementSpeed; }
    }
    [SerializeField] private TextMeshProUGUI resultText;

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
        movementSpeed = STARTING_MOVEMENT_SPEED;
        resultText.text = "Movement Speed: " + movementSpeed;
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
        //TODO: Rework this into increasing based on position on the pole rather than time
        do
        {
            yield return new WaitForSeconds(TIME_INCREMENTS);
            sliderSpeed += SLIDER_SPEED_INCREASE;
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
                    movementSpeed -= MOVEMENT_SPEED_CHANGE;
                    if (movementSpeed < 0)
                    {
                        movementSpeed = 0;
                    }
                    resultText.text = "Failure. \n Movement Speed: " + movementSpeed;
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
        if (!context.performed || !increasing)
        {
            return;
        }
        //Gettingthe low and high ends of the target
        float low = targetPosition - targetRange / 2;
        float high = targetPosition + targetRange / 2;
        increasing = false;
        if (slider.value >= low && slider.value <= high)
        {
            movementSpeed += MOVEMENT_SPEED_CHANGE;
            resultText.text = "Success! \n Movement Speed: " + movementSpeed;
        }
        else
        {
            movementSpeed -= MOVEMENT_SPEED_CHANGE;
            if (movementSpeed < 0)
            {
                movementSpeed = 0;
            }
            resultText.text = "Failure. \n Movement Speed: " + movementSpeed;
        }
    }
}
