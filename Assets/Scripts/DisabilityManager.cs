using System;
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
    public static DisabilityManager instance { get; private set; }
    //[HeaderAttribute("Disabilities")]
    //public bool protanopia = false;     // Missing Red
    //public bool deuteranopia = false;   // Missing Green
    //public bool tritanopia = false;     // Missing Blue
    [Header("Full Screen Shaders")]
    public Material cataractsMaterial;

    [Header("Audio Sources")]
    public AudioSource protanopiaSound;
    public AudioSource deuteranopiaSound;
    public AudioSource tritanopiaSound;
    public AudioSource glaucomaSound;
    public AudioSource cataractsSound;
    public AudioSource normalVisionSound;

    private Volume postProcessingVolume;

    private Dictionary<string, Dictionary<string, Vector3>> colorBlindnessProfiles;
    private List<Action> disabilities;

    private int lastDisability;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

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

        disabilities = new List<Action>();
        disabilities.Add(ApplyProptanopia);
        disabilities.Add(ApplyDeuteranopia);
        disabilities.Add(ApplyTritanopia);
        disabilities.Add(ApplyGlaucoma);
        disabilities.Add(ApplyCataracts);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void ApplyColorBlindness(string colorblindnessName)
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


    public void ApplyColorBlindness(ColorBlindessTypes colorBlindNessName)
    {
        ApplyColorBlindness(colorBlindNessName.ToString());
    }

    [ContextMenu("Remove VisionImpairments")]
    public void ClearAllVisionImpairments()
    {
        ApplyColorBlindness(ColorBlindessTypes.normalvision);
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = false;
        };
        //if (postProcessingVolume.profile.TryGet<Bloom>(out var bloom))
        //{
        //    bloom.active = false;
        //};
        cataractsMaterial.SetFloat("_Intensity", 0);
    }

    [ContextMenu("Apply Proptanopia")]
    public void ApplyProptanopia()
    {
        protanopiaSound.PlayOneShot(protanopiaSound.clip, 1f);
        ApplyColorBlindness(ColorBlindessTypes.proptanopia);
        //ApplyColorBlindness("proptanopia");
    }

    [ContextMenu("Apply Deuteranopia")]
    public void ApplyDeuteranopia()
    {
        deuteranopiaSound.PlayOneShot(deuteranopiaSound.clip, 1f);
        ApplyColorBlindness(ColorBlindessTypes.deuteranopia);
        //ApplyColorBlindness("deuteranopia");
    }

    [ContextMenu("Apply Tritanopia")]
    public void ApplyTritanopia()
    {
        tritanopiaSound.PlayOneShot(tritanopiaSound.clip, 1f);
        ApplyColorBlindness(ColorBlindessTypes.tritanopia);
        //ApplyColorBlindness("tritanopia");
    }

    [ContextMenu("Apply NormalVision")]
    public void ApplyNormalVision()
    {
        normalVisionSound.PlayOneShot(normalVisionSound.clip, 1f);
        ClearAllVisionImpairments();
    }

    [ContextMenu("Apply Glaucoma")]
    public void ApplyGlaucoma()
    {
        glaucomaSound.PlayOneShot(glaucomaSound.clip, 1f);
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = true;
        };
    }

    [ContextMenu("Apply Cataracts")]
    public void ApplyCataracts()
    {
        cataractsSound.PlayOneShot(cataractsSound.clip, 1f);
        //if (postProcessingVolume.profile.TryGet<Bloom>(out var bloom))
        //{
        //    bloom.active = true;
        //};
        cataractsMaterial.SetFloat("_Intensity", 1);
    }


    [ContextMenu("Apply Macular Degeneration")]
    public void ApplyMacularDegeneration()
    {
        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.active = true;
        };
    }


    public void SetRandomDisability()
    {
        ClearAllVisionImpairments();
        int randomNumber = UnityEngine.Random.Range(0, disabilities.Count-2);
        Action disability = disabilities[randomNumber];
        disability();
        disabilities.RemoveAt(randomNumber);
        disabilities.Add(disability);

        Debug.Log("Disability Appied");
        //Debug.Log(disabilities);
    }

}
