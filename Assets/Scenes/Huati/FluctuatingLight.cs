using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FluctuatingLight : MonoBehaviour
{
    private Light2D _light;
    public Vector2 lightIntensity, radiusFluctuation;
    public float lightSpeed = 0.5f, radiusSpeed;
    public float _targetIntensity = 0;
    public bool _waxing = true;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _targetIntensity = Random.Range(lightIntensity.x, lightIntensity.y);
    }
    void Update()
    {
        if(Mathf.Approximately(_light.intensity, _targetIntensity)) { _targetIntensity = Random.Range(lightIntensity.x, lightIntensity.y); }
        _light.intensity = Mathf.Lerp(_light.intensity, _targetIntensity, lightSpeed);

        if (_light.pointLightOuterRadius >= radiusFluctuation.y) { _waxing = false; }
        else if(_light.pointLightOuterRadius <= radiusFluctuation.x) { _waxing = true; }


        if (_waxing) { _light.pointLightOuterRadius -= Time.deltaTime * radiusSpeed; }
        else { _light.pointLightOuterRadius += Time.deltaTime * radiusSpeed; }
    }
}
