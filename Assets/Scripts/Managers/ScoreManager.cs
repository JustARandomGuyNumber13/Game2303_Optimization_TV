using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    static int score;
    static string _scoreText;
    static Text text;
    [SerializeField] Text textRef;
    

    void Start ()
    {
        score = 0;
        _scoreText = "";
        text = textRef;
    }

    public static void UpdateScore(int value)   // Change to event base Update, prevent generating GC Allocate by reusing _scoreText as string text _TV_
    {
        score += value;
        _scoreText = "Score: " + score;
        text.text = _scoreText;
    }
}
