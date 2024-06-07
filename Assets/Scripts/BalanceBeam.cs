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

//Enum for the side of the balance meter the arrow is on and where the slider will be moving to
public enum Direction
{
    Left = -1,
    Right = 1
}

/// <summary>
/// Class for dealing with the balance meter
/// </summary>
public class BalanceBeam : MonoBehaviour
{
    //Transform of the handle
    [SerializeField]private RectTransform handleRect; 
    //This is the value of the current rotation between -1 and 1
    private float rotation;
    //Public getter and setter for rotation
    public float Rotation
    {
        get { return rotation; }
        set 
        {
            rotation = value;
            OnRotation(value);
        }
    }
    //Public setter for being on the pole
    public bool OnPole
    {
        set { 
            if (value)
            {
                StartCoroutine(Balancing());
            }
        }
    }
    //Speed of slider movement
    private float speed;
    //Starting speed
    private const float START_SPEED = .5f;
    //Speed increase value
    private const float SPEED_INCREASE = .1f;
    private System.Random rand = new System.Random();
    //Where the slider is trying to move to
    private float target;
    //If the player has slipped off
    private bool slipped;
    //Public getter for slipped. Will be used for the animations
    public bool Slipped
    {
        get { return slipped; }
        set { slipped = value; }
    }
    //Temporary text display for slipping off
    [SerializeField]private GameObject slipText;
    //The current direction that the player is leaning
    private Direction direction;
    public Direction Direction
    {
        get { return direction; }
        set {
            //If the player is leaning in a different direction increase the speed
                if (value != direction)
                {
                    speed += SPEED_INCREASE;
                }
                direction = value;
            }
    }

    private void Start()
    {
        Rotation = 0;
        speed = START_SPEED;
        slipped = false;
    }

    private void OnRotation(float value)
    {
        //Apply the rotation to the handle
        handleRect.localRotation = Quaternion.Euler(0, 0, value * 35);
    }

    /// <summary>
    /// Smoothly moves the slider towards the targeted side
    /// </summary>
    /// <param name="target">The target value</param>
    private IEnumerator Balancing()
    {
        while(!slipped)
        {
            switch (Rotation)
            {
                //If the rotation is leaning left or right then set the direction that way as well 
                case < 0:
                    Direction = Direction.Left;
                    break;
                case > 0:
                    Direction = Direction.Right;
                    break;
                default:
                    //If it is exactly in the middle pick a random direction
                    if (rand.Next(0, 2) == 0)
                    {
                        Direction = Direction.Left;
                    }
                    else
                    {
                        Direction = Direction.Right;
                    }
                    break;
            }
            //Rotates it towards the direction the player is leaning
            //It is using move towards so it moves smoothly
            Rotation = Mathf.MoveTowards(Rotation, (float)Direction, speed * Time.deltaTime);
            //If the gauge is in the red then the player has slipped
            if(Rotation < -.8f || Rotation > .8f)
            {
                slipped = true;
                slipText.SetActive(true);
                //Round rotation to the nearest whole number (-1 or 1)
                Rotation = Mathf.Round(Rotation);
            }
            yield return null;
        }
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
        //Change by a random number between 0.1 and 0.3
        //Calculation from GitHub Copilot
        float change = (float)rand.NextDouble() * 0.2f + 0.1f;
        if(context.ReadValue<Vector2>().x > 0)
        {
            Rotation = Rotation - change;
        }
        else
        {
            Rotation = Rotation + change;
        }
    }
}
