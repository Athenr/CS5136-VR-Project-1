using UnityEngine;

public class ShakeMute : MonoBehaviour
{
    public float shakeThreshold = 2.0f;
    private Vector3 lastPosition;
    private AudioSource audioSource;
    private float cooldown = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        if (speed > shakeThreshold && cooldown <= 0f)
        {
            Debug.Log("SHOOK!!!");
            audioSource.mute = !audioSource.mute;
            cooldown = 3f;
        }
        cooldown -= Time.deltaTime;
        lastPosition = transform.position;
    }
}