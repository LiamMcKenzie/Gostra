using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateHandle : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public RectTransform handleRect; // Reference to the handle RectTransform

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        if (handleRect == null)
        {
            handleRect = slider.handleRect;
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Calculate the rotation angle based on the slider's value
        float angle = value * 180f; // 360 degrees for a full rotation

        // Apply the rotation to the handle
        handleRect.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
