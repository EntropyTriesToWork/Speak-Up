using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HuatiGameManager : MonoBehaviour
{
    public static HuatiGameManager Instance;

    private AudioClip _micClip;
    public int sampleWindow;
    public Microphone mic;
    public HuatiSettings settings;
    public float micVolFillSpeed = 0.95f;
    public Image volumeFill;

    public GameObject micOnImage, micOffImage;

    public GameObject controlPanel;

    public bool micActivated;

    public float CurrentMicVolume { get; private set; }
    public int micSelection = 0;
    public bool lockMicVolVisual = false;

    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        if (Microphone.devices.Length <= 0) { Debug.LogError("No microphones detected!"); }
        MicrophoneToAudioClip(micSelection);
    }
    private void Start()
    {
        controlPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            micActivated ^= true;
            micOnImage.SetActive(micActivated);
            micOffImage.SetActive(!micActivated);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { ReloadScene(); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { MicrophoneToAudioClip(micSelection++); }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { MicrophoneToAudioClip(micSelection--); }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controlPanel.SetActive(!controlPanel.activeSelf);
        }

        CurrentMicVolume = GetMicrophoneVolume();

        if(CurrentMicVolume > 0f && !lockMicVolVisual) { SetMicVolVisual(CurrentMicVolume); }
    }

    public void SetMicVolVisual(float micVol, bool lockVisual = false, bool lerp = true)
    {
        if (lerp) { volumeFill.fillAmount = Mathf.Lerp(volumeFill.fillAmount, micVol, micVolFillSpeed); }
        else { volumeFill.fillAmount = micVol; }
        lockMicVolVisual = lockVisual;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void MicrophoneToAudioClip(int micIndex)
    {
        Microphone.End(Microphone.devices[micSelection]);
        string micName = Microphone.devices[micIndex];
        _micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
        micActivated = true;
        micOffImage.SetActive(false);
        Debug.Log(Microphone.devices[micIndex]);
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
            if (waveData[i] > maxVol) { maxVol = waveData[i]; }
            meanVol += Mathf.Abs(waveData[i]);
        }
        float intensity = (meanVol + maxVol) / 2f / sampleWindow * settings.sensitivity;
        return intensity;
    }
}
