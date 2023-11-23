using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TongueScript : MonoBehaviour
{
    LineRenderer line;
    
    [SerializeField] Transform endTongue;
    [SerializeField] Vector3 offset;
    
    
    
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        line.SetPosition(1, endTongue.localPosition + offset);
    }
}
