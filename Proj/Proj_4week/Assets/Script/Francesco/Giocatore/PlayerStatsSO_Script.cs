using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Stats (S.O.)", fileName = "PlayerStats_SO")]
public class PlayerStatsSO_Script : ScriptableObject
{
    [SerializeField] int maxLives = 5;
    [SerializeField] int score;
    int lives;

    [Header("—— Collezionabili ——")]
    [SerializeField] List<bool> butterflyCollected;

    [Header("—— Salvataggio ——")]
    [SerializeField] Vector3 checkpointPos = Vector3.zero;
    [Space(10)]
    [SerializeField] bool isLoadingASave;



    #region Funz. Set personalizzate

    public void RemoveLife()
    {
        lives--;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void SetButterflyCollected(int index, bool value)
    {
        butterflyCollected[index] = value;
    }

    public void SetCheckpointPos(Vector3 newPos)
    {
        checkpointPos = newPos;
    }


    public void LoadCheckpointPos(Vector3 newPos)
    {
        checkpointPos = newPos;

        isLoadingASave = true;
    }

    public void LoadButterflyCollected(int index, bool value)
    {
        butterflyCollected[index] = value;
    }

    #endregion


    #region Funz. Get personalizzate

    public int GetLives() => lives;

    public int GetScore() => score;

    public List<bool> GetButterflyCollected() => butterflyCollected;

    public Vector3 GetCheckpointPos() => checkpointPos;

    public bool GetIsLoadingASave() => isLoadingASave;

    #endregion


    #region Funzioni Reset

    public void ResetLives()
    {
        lives = maxLives;
    }

    public void ResetScore()
    {
        score = 0;
    }

    #endregion
}
