using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour, IDataPersistence
{ 
 
    public int score;
    public Text scoreDisplay;
 
 public void LoadData(GameData data)
 {
score = data.score;
 }

 public void SaveData( GameData data)
 {
 data.score = score;
 }
    private void Update()
    {
        scoreDisplay.text = "Счёт: " + score.ToString();
    }
    public void Kill()
    {
        score++;
    }
}