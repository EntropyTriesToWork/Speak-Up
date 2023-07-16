using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HuatiSettingsController : MonoBehaviour
{
    public HuatiSettings settings;

    public TMP_InputField sensitivityInput, thresholdInput, minJumpTimeInput, jumpForceInput;
    public Slider sensitivitySlider, thresholdSlider, minJumpSlider, jumpForceSlider;

    private void Start()
    {
        ChangeSensitivity(settings.sensitivity);
        ChangeThreshold(settings.jumpThreshhold);
        ChangeMinJumpTime(settings.minJumpTime);
        ChangeJumpForce(settings.jumpForce);
    }

    public void ChangeSensitivity(float value)
    {
        settings.sensitivity = value;
        sensitivityInput.text = value.ToString("n2");
        sensitivitySlider.value = value;
    }
    public void ChangeSensitivity(string value)
    {
        float num = float.Parse(value);

        sensitivitySlider.value = num;
        sensitivityInput.text = value;
        settings.sensitivity = num;
    }
    public void ChangeThreshold(float value)
    {
        settings.jumpThreshhold = value;
        thresholdInput.text = value.ToString("n2");
        thresholdSlider.value = value;
    }
    public void ChangeThreshold(string value)
    {
        float num = float.Parse(value);

        thresholdSlider.value = num;
        thresholdInput.text = value;
        settings.jumpThreshhold = num;
    }
    public void ChangeJumpForce(float value)
    {
        settings.jumpForce = value;
        jumpForceInput.text = value.ToString("n2");
        jumpForceSlider.value = value;
    }
    public void ChangeJumpForce(string value)
    {
        float num = float.Parse(value);

        jumpForceSlider.value = num;
        jumpForceInput.text = value;
        settings.jumpForce = num;
    }
    public void ChangeMinJumpTime(float value)
    {
        settings.minJumpTime = value;
        minJumpTimeInput.text = value.ToString("n2");
        minJumpSlider.value = value;
    }
    public void ChangeMinJumpTime(string value)
    {
        float num = float.Parse(value);

        minJumpSlider.value = num;
        minJumpTimeInput.text = value;
        settings.minJumpTime = num;
    }

    public void ResetToDefaultSettings()
    {
        ChangeSensitivity(3f);
        ChangeThreshold(0.1f);
        ChangeJumpForce(20f);
        ChangeMinJumpTime(0.5f);
    }
}
