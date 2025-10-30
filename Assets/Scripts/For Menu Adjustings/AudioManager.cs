using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioClip[] audioClips;

    void Start()
    {
        StartCoroutine(PlayCustomSequence());
    }

    IEnumerator PlayCustomSequence()
    {
        audioSource1.clip = audioClips[0];
        audioSource1.Play();
        yield return new WaitForSeconds(audioClips[0].length);

        audioSource1.clip = audioClips[1];
        audioSource2.clip = audioClips[2];
        audioSource1.Play();
        audioSource2.Play();
        yield return new WaitForSeconds(Mathf.Max(audioClips[1].length, audioClips[2].length));

        audioSource1.clip = audioClips[3];
        audioSource1.Play();
        yield return new WaitForSeconds(audioClips[3].length);

        audioSource1.clip = audioClips[4];
        audioSource1.Play();
        yield return new WaitForSeconds(audioClips[4].length);
    }
}
