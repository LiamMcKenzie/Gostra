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
        if(audioSource == null)
        {
            return;
        }
        audioSource.pitch = Random.Range(0.9f, 1.1f); //randomizes the pitch
        audioSource.PlayOneShot(footstepSand, Random.Range(0.8f, 1.2f)); //plays the sound once, randomizes volume
    }

    private void Update()
    {
        if(player.IsIdle || player.IsFalling)
        {
            timer = 0f;
            return;
        }
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
