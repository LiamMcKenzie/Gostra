/// <remarks>
/// Author: Palin Wiseman
/// Date Created: June 5, 2024
/// Bugs: None known at this time.
/// </remarks>
/// <summary>
/// Code for the running meter
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class BalanceBeam : MonoBehaviour
{
    //The slider attached to this gameobject
    private Slider slider;
    //Transform of the handle
    private RectTransform handleRect; 
    //This is the value of the slider scaled to be between -1 and 1
    [HideInInspector] public float sliderValue;
    //Set to true when the user is on the pole
    public bool onPole;
    //If the slider is already moving
    private bool moving;
    //Set this to true to stop the slider from moving
    private bool stopped;
    //Speed of slider movement
    private float speed = 1f;
    private System.Random rand = new System.Random();
    //Where the slider is trying to move to
    private float target;
    //If the player has slipped off
    [HideInInspector] public bool slipped;
    //Temporary text display for slipping off
    public GameObject slipText;

    private void Start()
    {
        slider = GetComponent<Slider>();
        handleRect = slider.handleRect;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        sliderValue = 0;
        slider.value = 0.5f;
        onPole = false;
        moving = false;
        stopped = false;
        slipped = false;
        StartCoroutine(Slipping());
    }

    private void OnSliderValueChanged(float value)
    {
        // Calculate the rotation angle based on the slider's value
        float angle = value * -70f;

        // Apply the rotation to the handle
        handleRect.localRotation = Quaternion.Euler(0, 0, angle);

        //Scaling the value to be between -1 and 1
        sliderValue = (value - 0.5f) * 2;
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private IEnumerator Slipping()
    {
        while(!slipped)
        {
            //Check if the user is on the pole there isn't already a slidermove coroutine running
            if(onPole && !moving)
            {
                //Getting a random float between -.3 and .3. This is ugly but idk how else to do it atm
                float randFloat = (float)rand.Next(-30, 30) * .01f;
                float newValue = slider.value + randFloat;
                target = Mathf.Clamp(newValue, 0, 1);
                StartCoroutine(SliderMove());
            }
            yield return new WaitForSeconds((float)rand.Next(0, 20)*.01f); //Getting a random float between 0 and 2
        }
    }

    /// <summary>
    /// Smoothly moves the slider to the target value
    /// </summary>
    /// <param name="target">The target value</param>
    private IEnumerator SliderMove()
    {
        moving = true;
        while(slider.value != target)
        {
            slider.value = Mathf.MoveTowards(slider.value, target, speed * Time.deltaTime);
            if(slider.value < .1f || slider.value > .9f)
            {
                slipped = true;
                slipText.SetActive(true);
                //Round slider value to the nearest whole number (0 or 1)
                slider.value = Mathf.Round(slider.value);
            }
            yield return null;
        }
        moving = false;
    }

    /// <summary>
    /// Called by the input system movement action
    /// </summary>
    public void PlayerBalancing(InputAction.CallbackContext context)
    {
        if (!context.performed || slipped)
        {
            return;
        }        
        if(context.ReadValue<Vector2>().x > 0)
        {
            target = slider.value + 0.1f;
        }
        else
        {
            target = slider.value - 0.1f;
        }
        if (!moving)
        {
            StartCoroutine(SliderMove());
        }
    }

}
