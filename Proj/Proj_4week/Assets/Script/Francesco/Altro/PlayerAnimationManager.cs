using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] PlayerMovevent playerMovScr;
    [SerializeField] PlayerHoldItems holdItemsScr;

    [Space(10)]
    [SerializeField] Animator playerAnim;



    void Update()
    {
        //Camminata e corsa
        playerAnim.SetBool("isWalking", playerMovScr.GetIsWalking());
        playerAnim.SetBool("isRunning", playerMovScr.GetIsRunning());


        //Salto
        playerAnim.SetFloat("jumpCharge", playerMovScr.GetJumpCharge());
        playerAnim.SetBool("isOnGround", playerMovScr.GetIsGrounded());

        Vector3 plVel = playerMovScr.GetCharController().velocity;
        playerAnim.SetFloat("verticalVelocity", plVel.y);


        //Nuoto
        playerAnim.SetBool("isSwimming", playerMovScr.GetIsWalking() && playerMovScr.GetInWater());


        //Lingua
        playerAnim.SetBool("hasObject", holdItemsScr.GetIsHoldingItem());
    }


    //Salto
    public void TriggerJump()
    {
        playerAnim.SetTrigger("jump");
    }


    //Lingua
    public void TriggerShootTongue()
    {
        playerAnim.SetTrigger("shootTongue");
    }
    public void TriggerReturnTongue()
    {
        playerAnim.SetTrigger("returnTongue");
    }


    //Danno e Morte
    public void TriggerDamage()
    {
        playerAnim.SetTrigger("damage");
    }
    public void TriggerDeath()
    {
        playerAnim.SetTrigger("death");
    }
}
