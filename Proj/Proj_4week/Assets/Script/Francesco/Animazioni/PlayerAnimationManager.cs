using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] PlayerMovevent playerMovScr;
    [SerializeField] PlayerHoldItems holdItemsScr;
    [SerializeField] PlayerStatsManager statsMng;

    [Space(10)]
    [SerializeField] Animator playerAnim;

    bool debug_bool;
    float debug_float;



    void Update()
    {
        //Camminata e corsa
        playerAnim.SetBool("isWalking", true);
        playerAnim.SetBool("isRunning", true);

        //Salto
        playerAnim.SetFloat("jumpCharge", 2.5f);
        playerAnim.SetTrigger("jump");
        playerAnim.SetFloat("verticalVelocity", 2.5f);
        playerAnim.SetBool("isOnGround", true);

        //Nuoto
        playerAnim.SetBool("isSwimming", true);

        //Lingua
        playerAnim.SetTrigger("shootTongue");
        playerAnim.SetTrigger("returnTongue");
        playerAnim.SetBool("hasObject", true);

        //Danno e Morte
        playerAnim.SetTrigger("damage");
        playerAnim.SetTrigger("death");
    }
}
