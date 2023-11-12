using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // For URP

public class DisabilityManager : MonoBehaviour
{
    [HeaderAttribute("Disabilities")]
    public bool protanopia = false;     // Missing Red
    public bool deuteranopia = false;   // Missing Green
    public bool tritanopia = false;     // Missing Blue

    private Volume postProcessingVolume;

    private void Awake()
    {
        postProcessingVolume = gameObject.GetComponent<Volume>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void applyColorBlindness()
    {
        if (postProcessingVolume.profile.TryGet<ColorCurves>(out var colorCurves))
        {
            colorCurves.red.overrideState = protanopia;
            colorCurves.green.overrideState = deuteranopia;
            colorCurves.blue.overrideState = tritanopia;
        }
    }

    // Update is called once per frame
    void Update()
    {
        applyColorBlindness();
    }
}
