using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public ToSaveStuff scores;

    string path = "Assets/Resources/highScore.txt";
    string JSONString;
    TextAsset textFile;   

    private void Start()
    {
        if (!File.Exists(path))
        {
            int[] scoresNums = new int[5];
            for (int i = 0; i < scoresNums.Length; i++)
            {
                scoresNums[i] = i;
            }
            scores = new ToSaveStuff(scoresNums);
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(JSONString);
            }
            JSONString = JsonUtility.ToJson(scores);
        }
        else
        {
            textFile = Resources.Load("highScore") as TextAsset;
            scores = JsonUtility.FromJson<ToSaveStuff>(textFile.text);
        }
    }    
}

[System.Serializable]
public class ToSaveStuff
{
    public int[] scores;
    public ToSaveStuff(int[] newScore) { scores = newScore; }
    public ToSaveStuff() { scores = new int[5]; }
}