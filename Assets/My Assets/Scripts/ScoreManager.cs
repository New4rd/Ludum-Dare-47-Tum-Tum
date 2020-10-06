using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager _inst;
    static public ScoreManager Inst
    {
        get { return _inst; }
    }

    [SerializeField] Text scoreText;


    private void Awake()
    {
        _inst = this;
    }


    public void UpdateScore(int value)
    {
        scoreText.text = "Score: " + value.ToString();
    }
}
