using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvents : MonoBehaviour
{
    [SerializeField] BossScript bossScr;

    public void End_StartAnim()
    {
        bossScr.SetStart_Anim(false);
        print("Fine start");
    }

    public void MeleeAttack()
    {
        bossScr.ActivateMeleeAttack();
    }

    public void ShootAttack()
    {
        bossScr.ActivateShootAttack();
    }
}
