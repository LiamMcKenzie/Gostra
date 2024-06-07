/// <summary>
/// controls the animation and rotation of the player character.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode] //this makes this script run in the editor
public class PlayerAnimator : MonoBehaviour
{
    [Range(-1,1)]
    public float playerRotation; //-1 and 1 are the limits for player rotation, if rotation is over 1 or less than -1 that means the player has fallen
    public float maxRotation = 45; //how much the player will rotate when the playerRotation is 1 or -1 (eg 45 degrees)

    [Range(-1,1)]
    public float fallThreshold = 0.9f; //the amount the player has to rotate before falling 

    [Range(0,3)]
    public float speedThreshold = 0.3f; //the amount the player has to slow down to before falling

    public Ragdoll ragdoll;

    public bool isFalling = false;

    public Animator animator;

    [SerializeField] private MoveAlongSpline moveAlongSpline;

    void Update()
    {
        if(isFalling == false)
        {
            playerRotation = Mathf.Clamp(playerRotation, -maxRotation, maxRotation); //this makes sure the player rotation is between -1 and 1
            Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, playerRotation * -maxRotation);
            transform.localRotation = rotation; //this rotates the player based on the playerRotation value
        }

        if(Application.isPlaying && isFalling == false)
        {
            if(playerRotation > fallThreshold || playerRotation < -fallThreshold || moveAlongSpline.Speed <= speedThreshold) 
            {
                isFalling = true;
                
                //animator.enabled = false; //disable the animator component. which causes the player to become a ragdoll
            }
        }

        ragdoll.isRagdoll(isFalling);  //enable/disable the ragdoll
    }
}
