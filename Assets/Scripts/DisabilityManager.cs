using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // For URP

public enum ColorBlindessTypes
{
    normalvision,
    proptanopia,
    deuteranopia,
    tritanopia,
}

public class DisabilityManager : MonoBehaviour
{
    //[HeaderAttribute("Disabilities")]
    //public bool protanopia = false;     // Missing Red
    //public bool deuteranopia = false;   // Missing Green
    //public bool tritanopia = false;     // Missing Blue

    private Volume postProcessingVolume;

    private Dictionary<string, Dictionary<string, Vector3>> colorBlindnessProfiles;

    private void Awake()
    {
        postProcessingVolume = gameObject.GetComponent<Volume>();
        colorBlindnessProfiles = new Dictionary<string, Dictionary<string, Vector3>>
        {
            { "normalvision", new Dictionary<string, Vector3>{
                { "r", new Vector3 (100, 0  , 0  ) },
                { "g", new Vector3 (0  , 100, 0  ) },
                { "b", new Vector3 (0  , 0  , 100) },
            } },
            { "proptanopia", new Dictionary<string, Vector3>{ 
                { "r", new Vector3 (56.667f, 43.333f, 0      ) },
                { "g", new Vector3 (55.833f, 44.167f, 0      ) },
                { "b", new Vector3 (0,       24.167f, 75.833f) },
            } },
            { "deuteranopia", new Dictionary<string, Vector3>{
                { "r", new Vector3 (62.5f, 37.5f, 0 ) },
                { "g", new Vector3 (70,    30,    0 ) },
                { "b", new Vector3 (0,     30,    70) },
            } },
            { "tritanopia", new Dictionary<string, Vector3>{
                { "r", new Vector3 (95, 5,       0      ) },
                { "g", new Vector3 (0,  43.333f, 56.667f) },
                { "b", new Vector3 (0,  47.5f,   52.5f  ) },
            } },
        };
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void applyColorBlindness(string colorblindnessName)
    {
        //if (postProcessingVolume.profile.TryGet<ColorCurves>(out var colorCurves))
        //{
        //    colorCurves.red.overrideState = protanopia;
        //    colorCurves.green.overrideState = deuteranopia;
        //    colorCurves.blue.overrideState = tritanopia;
        //}
        if (postProcessingVolume.profile.TryGet<ChannelMixer>(out var channelMixer))
        {
            channelMixer.redOutRedIn.value     = colorBlindnessProfiles[colorblindnessName]["r"][0];
            channelMixer.redOutGreenIn.value   = colorBlindnessProfiles[colorblindnessName]["r"][1];
            channelMixer.redOutBlueIn.value    = colorBlindnessProfiles[colorblindnessName]["r"][2];

            channelMixer.greenOutRedIn.value   = colorBlindnessProfiles[colorblindnessName]["g"][0];
            channelMixer.greenOutGreenIn.value = colorBlindnessProfiles[colorblindnessName]["g"][1];
            channelMixer.greenOutBlueIn.value  = colorBlindnessProfiles[colorblindnessName]["g"][2];

            channelMixer.blueOutRedIn.value    = colorBlindnessProfiles[colorblindnessName]["b"][0];
            channelMixer.blueOutGreenIn.value  = colorBlindnessProfiles[colorblindnessName]["b"][1];
            channelMixer.blueOutBlueIn.value   = colorBlindnessProfiles[colorblindnessName]["b"][2];
        }
    }


    public void applyColorBlindness(ColorBlindessTypes colorBlindNessName)
    {
        applyColorBlindness(colorBlindNessName.ToString());
    }

    [ContextMenu("Remove VisionImpairments")]
    public void clearAllVisionImpairments()
    {
        applyColorBlindness(ColorBlindessTypes.normalvision);
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = false;
        };
        if (postProcessingVolume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.active = false;
        };
    }

    [ContextMenu("Apply Proptanopia")]
    public void applyProptanopia()
    {
        applyColorBlindness(ColorBlindessTypes.proptanopia);
        //applyColorBlindness("proptanopia");
    }

    [ContextMenu("Apply Deuteranopia")]
    public void applyDeuteranopia()
    {
        applyColorBlindness(ColorBlindessTypes.deuteranopia);
        //applyColorBlindness("deuteranopia");
    }

    [ContextMenu("Apply Tritanopia")]
    public void applyTritanopia()
    {
        applyColorBlindness(ColorBlindessTypes.tritanopia);
        //applyColorBlindness("tritanopia");
    }

    [ContextMenu("Apply NormalVision")]
    public void applyNormalVision()
    {
        applyColorBlindness(ColorBlindessTypes.normalvision);
    }

    [ContextMenu("Apply Glaucoma")]
    public void applyGlaucoma()
    {
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = true;
        };
    }

    [ContextMenu("Apply Cataracts")]
    public void applyCataracts()
    {
        if (postProcessingVolume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.active = true;
        };
    }


    [ContextMenu("Apply Macular Degeneration")]
    public void applyMacularDegeneration()
    {
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = true;
        };
    }


    // Update is called once per frame
    void Update()
    {
       // applyColorBlindness("proptanopia");
    }
}
