using UnityEngine;

public class CallCataracts : MonoBehaviour
{
    public DisabilityManager dm;
    // This function is called when a collision occurs
    private void OnCollisionEnter()
    {
        // Call your function here
        dm.ApplyCataracts();
    }
}