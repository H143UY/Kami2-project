using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Score;
    private int currentscore;
    private int addscore;
    private void Awake()
    {
        this.RegisterListener(EventID.addScore, (sender, param) =>
        {
            AddScore(100);
        }); 
    }
    void Update()
    {
        if (Score != null)
        {
            Score.text = currentscore.ToString();
        }
    }
    public void AddScore(int addScore)
    {
        currentscore += addScore;
    }
}
