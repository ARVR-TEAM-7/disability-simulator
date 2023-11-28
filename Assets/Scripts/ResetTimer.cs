using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimer : MonoBehaviour
{
    [ContextMenu("Reset Timer")]
    public void Reset()
    {
        DisabilityEvent.instance.StartNewSession();
    }
}
