using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //this makes this script run in the editor
public class PlayerAnimator : MonoBehaviour
{
    [Range(-1,1)]
    public float playerRotation; //-1 and 1 are the limits for player rotation, if rotation is over 1 or less than -1 that means the player has fallen
    public float maxRotation = 45; //how much the player will rotate when the playerRotation is 1 or -1 (eg 45 degrees)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerRotation = Mathf.Clamp(playerRotation, -maxRotation, maxRotation); //this makes sure the player rotation is between -1 and 1
        transform.localRotation = Quaternion.Euler(0, 0, playerRotation * -maxRotation); //this rotates the player based on the playerRotation value
    }
}
