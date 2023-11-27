using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
  public GameObject objectToSpawn; // The object you want to spawn
  public float minX, maxX, minY, maxY; // The range for the spawn position

  private void Start()
  {
    SpawnObject();
  }

  public void SpawnObject()
  {
    // Generate a random position within the specified range
    // float x = Random.Range(minX, maxX);
    // float y = Random.Range(minY, maxY);

    // Create a new instance of the object at the random position
    GameObject newObject = Instantiate(objectToSpawn, new Vector3(5, 5, 0), Quaternion.identity);
    // GameObject newObject = Instantiate(objectToSpawn, new Vector3(x, y, 0));

  }
}
