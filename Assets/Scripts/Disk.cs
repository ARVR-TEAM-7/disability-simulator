using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public GameObject disk;
    public GameObject diskShattered;
    public Color[] diskColors;

    public void SetRandomColor()
    {
        Material diskMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        diskMaterial.color = Color.red;
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
    }
}
