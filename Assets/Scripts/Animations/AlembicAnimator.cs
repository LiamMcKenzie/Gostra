/// <remarks>
/// Author: Johnathan
/// Date Created: 13/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the animation of the alembic file
/// </summary>

using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

/// <summary>
/// Control an Alembic stream player component
/// </summary>
public class AlembicAnimator : MonoBehaviour
{
    [SerializeField] private AlembicStreamPlayer alembicStreamPlayer;
    [SerializeField] private float animationSpeed = 1.0f;   // speed of the animation
    [SerializeField] private float startTime = 0.0f;    // start time of the animation
    [SerializeField] private float endTime = 0.0f;  // end time of the animation

    private float currentTime = 0.0f;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (alembicStreamPlayer == null)
        {
            return;
        }

        // increment the time
        currentTime += Time.deltaTime * animationSpeed;

        // loop back to the start if we reach the end
        if (currentTime > endTime)
        {
            currentTime = startTime;
        }

        // set the time on the Alembic stream player
        alembicStreamPlayer.CurrentTime = currentTime;
    }
}