using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagnettoAnimManger : MonoBehaviour
{
    [SerializeField] RagnoIA ragnoIAScr;
    [SerializeField] Animator anim;



    void Update()
    {
        anim.SetBool("isWalking", ragnoIAScr.GetIsWalking());
    }
}
