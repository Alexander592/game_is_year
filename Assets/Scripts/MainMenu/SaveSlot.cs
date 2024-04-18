using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SaveSlot : MonoBehaviour
{
  private Button saveSlotButton;

  private void Awake()
  {
    saveSlotButton = this.GetComponent<Button>();
  }

   [Header("Profile")]
   [SerializeField] private string profileId = "";

   [Header("Content")]
   [SerializeField] private GameObject noDataContent;
   [SerializeField] private GameObject hasDataContent;
   [SerializeField] private TextMeshProUGUI percentageCompleteText;
   [SerializeField] private TextMeshProUGUI ScoreCountText;

[Header("Clear Button Data")]
[SerializeField] private Button ClearButton;


public bool hasData {get; private set;} =  false;



public void SetData(GameData data)
{
    if (data == null)
    {
      hasData = false;
     noDataContent.SetActive(true);
     hasDataContent.SetActive(false);
     ClearButton.gameObject.SetActive(false);
    }
    else
    {
      hasData = true;
    noDataContent.SetActive(false);
    hasDataContent.SetActive(true);
    ClearButton.gameObject.SetActive(true);

    percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
    ScoreCountText.text = "SCORE COUNT: " + data.score;

    
    }
}
public string GetProfileId()
{
    return this.profileId;
}

public void SetInteractable(bool interactable)
{
    saveSlotButton.interactable = interactable;
    ClearButton.interactable = interactable;
}
}
