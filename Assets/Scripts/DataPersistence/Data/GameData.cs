using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
   public long lastUpdated;
     public Vector3 playerPosition;
     public int score;
     public int health;
     public SerializableDictionary<string, bool> coinsCollected;
     public AttributesData playerAttributesData;
          
      
 
 

 public GameData()
 {
       playerPosition = Vector3.zero;
       score = 0;
       health = 13;
       coinsCollected = new SerializableDictionary<string, bool>();
       playerAttributesData = new AttributesData();

       
        
     
  } 

  public int GetPercentageComplete()
  {
   int totalCollected = 0;
   foreach (bool collected in coinsCollected.Values)
   {
      if (collected)
      {
         totalCollected++;
      }
   }
   int percentageCompleted = -1;
   if (coinsCollected.Count != 0)
   {
      percentageCompleted = (totalCollected * 100 / coinsCollected.Count);
   }
   return percentageCompleted;
  }
}
