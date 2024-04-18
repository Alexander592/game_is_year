using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : Menu
{

[Header("Menu Navigation")]
[SerializeField] private SaveSlotsMenu saveSlotsMenu;

    
    [Header("Menu Buttons")]
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueGameButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button loadGameButton;

    
    private void Start()
{
     DisableButtonsDependingOnData();
}

public void DisableButtonsDependingOnData()
{
     if (!DataPersistenceManager.instance.HasGameData())
    {
     ContinueGameButton.interactable = false;
     loadGameButton.interactable = false;
    }
}
   public void OnNewGameClicked()
   {
     saveSlotsMenu.ActivateMenu(false);
     this.DeactivateMenu();
   } 

   public void OnLoadGameClicked()
   {
    saveSlotsMenu.ActivateMenu(true);
    this.DeactivateMenu();
   }

   public void OnContinueGameClicked()
   {

    DisableMenuButtons();
    DataPersistenceManager.instance.SaveGame();
    SceneManager.LoadSceneAsync("SampleScene");
   }

   public void OnSaveGameClicked()
   {
    DataPersistenceManager.instance.SaveGame();
   }
   
   
  
    public void OpenSettings()
    {
         DisableMenuButtons();
         SceneManager.LoadSceneAsync("Settings");
    }
    public void ExitGame()
    {
         
        Application.Quit();
    }
    private void DisableMenuButtons()
{
    NewGameButton.interactable = false;
    ContinueGameButton.interactable = false;
    OptionButton.interactable = false; 
}

public void ActivateMenu() 
{
 this.gameObject.SetActive(true);
 DisableButtonsDependingOnData();
}

public void DeactivateMenu()
{
 this.gameObject.SetActive(false);
}
}
