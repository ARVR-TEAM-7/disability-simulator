using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public GameObject disk;
    public GameObject diskShattered;
    public Color[] diskColors;

    private int scoreIncrementation = 0;

    public void SetRandomColor()
    {
        Material diskMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            diskMaterial.color = Color.red;
            scoreIncrementation = -2;
        }
        else
        {
            diskMaterial.color = Color.green;
            scoreIncrementation = 1;
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
        diskShattered.transform.position = disk.transform.position;
        disk.SetActive(false);
        diskShattered.SetActive(true);
        DisabilityEvent.instance.IncrementScore(scoreIncrementation);
    }
}
