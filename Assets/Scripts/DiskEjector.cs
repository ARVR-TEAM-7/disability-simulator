using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskEjector : MonoBehaviour
{
    // [Range(500f, 1500f)]
    [Header("Ejection Force")]
    public float minMagnitude = 500f;
    public float maxMagnitude = 1500f;
    [Header("Time Between Ejections")]
    public float minTimeTillNewDiskSeconds = 10f;
    public float maxTimeTillNewDiskSeconds = 30f;
    [Header("Prefab")]
    public GameObject diskPrefab;
    private MeshRenderer boundary;
    private float timeNow = 0f;
    private float timeSinceLastDisk = 0f;
    private float timeTillNewDisk = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        boundary = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeNow = Time.realtimeSinceStartup - timeSinceLastDisk;

        if (timeNow > timeTillNewDisk)
        {
            timeSinceLastDisk = Time.realtimeSinceStartup;
            timeTillNewDisk = Random.Range(minTimeTillNewDiskSeconds, maxTimeTillNewDiskSeconds);
            EjectDisk();
        }
        
    }
    
    // ChatGPT4
    Vector3 GetRandomPositionInMesh(MeshRenderer meshRender) {
        Bounds bounds = meshRender.bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );
        // If the collider is complex, you might need to ensure the point is within the mesh.    
        // This can be done using a Physics.Raycast or similar method to check if the point is inside the collider.
            return randomPosition;
    } 

    [ContextMenu("Spawn Disk")]
    public void EjectDisk()
    {
        GameObject disk = Instantiate(diskPrefab, GetRandomPositionInMesh(boundary), Quaternion.Euler(-90, 0, 0));
        disk.GetComponent<Disk>().SetRandomColor();
        Rigidbody diskRigidBody = disk.GetComponent<Disk>().disk.GetComponent<Rigidbody>();
        diskRigidBody.AddForce(Vector3.up * Random.Range(minMagnitude, maxMagnitude), ForceMode.Force);
        Destroy(disk, 30f); // in seconds
    }
}
