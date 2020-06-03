using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManagerScript : MonoBehaviour
{
    public int NrOfDetections;

    public AudioSource NormalMusicAudioSource;
    public AudioSource BattleAudioSource;

    [Range(0, 1)] public float NormalMusicMaxVolume;
    [Range(0, 1)] public float BattleMaxVolume;

    public float TransitionSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        NrOfDetections = 0;
        NormalMusicAudioSource.volume = NormalMusicMaxVolume;
        BattleAudioSource.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (NrOfDetections == 0)
        {
            if (BattleAudioSource.volume <= 0)
            {
                if (NormalMusicAudioSource.volume < NormalMusicMaxVolume)
                {
                    NormalMusicAudioSource.volume += 0.01f * TransitionSpeed * Time.deltaTime;
                }
            }
            else
            {
                BattleAudioSource.volume -= 0.01f * TransitionSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (NormalMusicAudioSource.volume <= 0)
            {
                if (BattleAudioSource.volume < BattleMaxVolume)
                {
                    BattleAudioSource.volume += 0.01f * TransitionSpeed * Time.deltaTime;
                }
            }
            else
            {
                NormalMusicAudioSource.volume -= 0.01f * TransitionSpeed * Time.deltaTime;
            }
        }
    }
}
