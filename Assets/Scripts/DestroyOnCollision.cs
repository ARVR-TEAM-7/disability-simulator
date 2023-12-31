using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
  public GameObject objectToSpawn;
  public float minX, maxX, minY, maxY; // The range for the spawn position


  private void OnCollisionEnter(Collision collision)
  {
        //Debug.Log(collision.gameObject.name);
    // Check if the object has a specific tag, for example "Enemy"
    if (collision.gameObject.tag == "DestroyCube")
    {
      // If it does, destroy the object
      Destroy(collision.gameObject);
      // Generate a random position within the specified range
      float x = Random.Range(minX, maxX);
      float y = Random.Range(minY, maxY);
      Instantiate(objectToSpawn, new Vector3(x, y, 0), Quaternion.identity);
    }
    else if (collision.gameObject.tag == "Disk")
    {
        Disk disk = collision.gameObject.GetComponentInParent<Disk>();
        disk.ShatterDisk();
    }
    else if (collision.gameObject.tag == "Reset")
    {
        ResetTimer timer = collision.gameObject.GetComponent<ResetTimer>();
        timer.Reset();
        gameObject.SetActive(false);
    }
    else if (collision.gameObject.tag == "Interaction")
        {
            VisionImpairmentCollider visionImpairmentScript = collision.gameObject.GetComponent<VisionImpairmentCollider>();
            if (null != visionImpairmentScript)
            {
                DisabilityManager.instance.SetDisability(visionImpairmentScript.impairmentName);
                gameObject.SetActive(false);
            }
        }
  }
}
