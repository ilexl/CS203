using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ownScoreGUI;
    [SerializeField] TextMeshProUGUI oppScoreGUI;
    [SerializeField] private int ownScore;
    [SerializeField] private int oppScore;
    public int GetOwnScore() { return ownScore; }
    public int GetOppScore() { return oppScore; }
    public void Reset() { ownScore = 0; oppScore = 0; }

    private void Update()
    {
        ownScoreGUI.text = ownScore + " : YOU";
        oppScoreGUI.text = "OPP : " + oppScore;
    }

    public void ChangeOwnScore(int change) { ownScore += change; }
    public void ChangeOppScore(int change) { oppScore += change; }

    // Hard Coded but can be changed if needed
    static readonly List<int> lettersScore = new List<int>()
    {
        1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10
    };
    public static int CalculateBaseScore(string word)
    {
        int score = 0;
        foreach(char c in word)
        {
            score += lettersScore[-65 + c];
        }
        return score;
    }
}
