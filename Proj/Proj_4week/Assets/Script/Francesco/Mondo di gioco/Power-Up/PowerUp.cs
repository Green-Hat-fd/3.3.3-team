using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowUpType_Enum
    {
        FireShoot,
        AmplifiedJump
    }

    [SerializeField] PowUpType_Enum powerUpType;

    [Space(10)]
    [Min(0)]
    [SerializeField] int scoreWhenCollected;


    [Header("—— Feedback ——")]
    [SerializeField] AudioSource pickUpSfx;



    public PowUpType_Enum GetPowerUpType() => powerUpType;

    public int GetScoreWhenCollected() => scoreWhenCollected;
}
