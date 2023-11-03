using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [Min(0)]
    [SerializeField] int collIndex;
    
    [Space(10)]
    [Min(0)]
    [SerializeField] int scoreWhenCollected;



    public int GetCollectableIndex() => collIndex;
    
    public int GetScoreWhenCollected() => scoreWhenCollected;
}
