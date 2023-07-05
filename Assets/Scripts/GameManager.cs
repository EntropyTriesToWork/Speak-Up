using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private AudioClip _micClip;
    public int sampleWindow;
    public Microphone mic;
    public int sensibility = 100;

    public Image micIntensityFill;
    public GameObject micOnImage, micOffImage;

    public float currentMicrophoneIntensity;

    public bool micActivated;

    public void Awake()
    {
        if(Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        if(Microphone.devices.Length <= 0) { Debug.LogError("No microphones detected!"); }
        MicrophoneToAudioClip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            micActivated ^= true;
            micOnImage.SetActive(micActivated);
            micOffImage.SetActive(!micActivated);
        }
    }

    private void FixedUpdate()
    {
        currentMicrophoneIntensity = GetMicrophoneVolume();
        if (micIntensityFill.fillAmount <= 0.05f) { micIntensityFill.fillAmount = currentMicrophoneIntensity; }
        else { micIntensityFill.fillAmount = Mathf.Lerp(currentMicrophoneIntensity, micIntensityFill.fillAmount, 0.95f); }
    }

    public void MicrophoneToAudioClip()
    {
        string micName = Microphone.devices[0];
        _micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
        micActivated = true;
        micOffImage.SetActive(false);
    }

    public float GetMicrophoneVolume()
    {
        if (!micActivated) { return 0; }
        return GetAudioIntensity(Microphone.GetPosition(Microphone.devices[0]), _micClip);
    }

    public float GetAudioIntensity(int clipPos, AudioClip audioClip)
    {
        int startPos = clipPos - sampleWindow;
        float[] waveData = new float[sampleWindow];
        audioClip.GetData(waveData, startPos);

        float meanVol = 0;
        float maxVol = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            if(waveData[i] > maxVol) { maxVol = waveData[i]; }
            meanVol += Mathf.Abs(waveData[i]);
        }
        float intensity = (meanVol + maxVol) / 2f / sampleWindow * sensibility;
        return intensity;
    }
}
