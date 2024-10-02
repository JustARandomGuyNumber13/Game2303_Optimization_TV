using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    //public static int score;
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

    //void Update ()
    //{
    //    text.text = "Score: " + score;    // Create a new string every frame, cause to generate GC Allocate _TV_
    //}
}
