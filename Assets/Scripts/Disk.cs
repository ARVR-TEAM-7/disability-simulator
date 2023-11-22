using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public GameObject disk;
    public GameObject diskShattered;

    [ContextMenu("Shatter Disk")]
    public void shatterDisk()
    {
        diskShattered.transform.position = disk.transform.position;
        disk.SetActive(false);
        diskShattered.SetActive(true);
    }
}
