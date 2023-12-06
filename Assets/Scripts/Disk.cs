using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
  public GameObject disk;
  public GameObject diskShattered;
  public Color[] diskColors;

  public AudioSource[] shatterSounds; // expected length = 3
  public AudioSource successSound;
  public AudioSource failSound;

  private int scoreIncrementation = 0;
  private AudioSource soundToPlay;

  public void SetRandomColor()
  {
    Material diskMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

    int randomNumber = Random.Range(0, 2);
    if (randomNumber == 0)
    {
      // red
      diskMaterial.color = new Color(231f/255f, 166f/255f, 112f/255f);
      scoreIncrementation = -2;
      soundToPlay = failSound;
    }
    else
    {
      // green
      diskMaterial.color = new Color(64f/255f, 166f/255, 112f/255f);
      scoreIncrementation = 1;
      soundToPlay = successSound;
    }

    diskMaterial.SetFloat("_Metallic", 1f);
    diskMaterial.SetFloat("_Glossiness", 0.5f);

    disk.GetComponent<MeshRenderer>().material = diskMaterial;
    foreach (Transform shatteredDisks in diskShattered.transform)
    {
      shatteredDisks.GetComponent<MeshRenderer>().material = diskMaterial;
    }
  }

  [ContextMenu("Shatter Disk")]
  public void ShatterDisk()
  {
    int randomNumber = Random.Range(0, 3);
    shatterSounds[randomNumber].PlayOneShot(shatterSounds[randomNumber].clip, 0.5f);
    soundToPlay.PlayOneShot(soundToPlay.clip, 0.3f);
    diskShattered.transform.position = disk.transform.position;
    disk.SetActive(false);
    diskShattered.SetActive(true);
    DisabilityEvent.instance.IncrementScore(scoreIncrementation);
  }
}
