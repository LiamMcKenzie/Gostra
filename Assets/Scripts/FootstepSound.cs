using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public AudioClip footstepSand;
    public AudioClip footstepMetal;
    private AudioSource audioSource;
    public float interval = 0.3f; 

    private float timer = 0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayFootstepSound()
    {
        audioSource?.PlayOneShot(footstepSand);
    }

    private void Update()
    {
        if(player.IsIdle)
        {
            timer = 0f;
            return;
        }
        timer += Time.deltaTime * player.Speed;

        if (timer >= interval)
        {
            PlayFootstepSound();

            timer = 0f;
        }
    }

}
