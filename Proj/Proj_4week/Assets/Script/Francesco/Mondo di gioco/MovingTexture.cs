using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MovingTexture : MonoBehaviour
{
    Material matToMove;

    //Con il range
    [Range(-0.5f, 0.5f)]
    [SerializeField] float scorrimX = 0.25f,
                           scorrimY = 0.25f;

    [Space(20)]
    [SerializeField] bool withoutRange = false;

    //Senza range
    [Space(10)]
    [SerializeField] float scorrimX_NoLimits = 0.25f;
    [SerializeField] float scorrimY_NoLimits = 0.25f;



    private void Awake()
    {
        matToMove = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float moveX = withoutRange
                         ? scorrimX_NoLimits * Time.time
                         : scorrimX * Time.time;
        float moveY = withoutRange
                         ? scorrimY_NoLimits * Time.time
                         : scorrimY * Time.time;


        matToMove.mainTextureOffset = new Vector2(moveX, moveY);
    }
}
