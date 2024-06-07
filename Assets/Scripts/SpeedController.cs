using UnityEngine;
using UnityEngine.Splines;
public class SpeedController : MonoBehaviour
{
    [SerializeField] private MoveAlongSpline moveAlongSpline;
    [SerializeField] private float speedReductionFactor = 0.5f;
    [SerializeField] private Transform playerPosition;
    private float playerStartPosY;

    private float PlayerPosY => playerPosition.position.y;
    // Start is called before the first frame update
    void Start()
    {
        playerStartPosY = PlayerPosY;
    }

    // Update is called once per frame

    void Update()
    {
        float adjustedPlayerPosY = PlayerPosY - playerStartPosY;
        if (adjustedPlayerPosY > 0)
        {
            float newSpeed = moveAlongSpline.Speed - (adjustedPlayerPosY * speedReductionFactor);
            moveAlongSpline.Speed = Mathf.Max(newSpeed, 0); // Ensure speed doesn't go below 0
        }
    }

}
