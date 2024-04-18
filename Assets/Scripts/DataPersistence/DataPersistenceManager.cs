using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
[Header("Debugging")]
[SerializeField] private bool disableDataPersistence = false;
[SerializeField] private bool initializeDataIfNull = false;
[SerializeField] private bool overrideSelectedProfileId = false;
[SerializeField] private string testSelectedProfileId = "test";

[Header("File Storage Config")]
[SerializeField] private string fileName;
[SerializeField] private bool useEncryption;

[Header("Auto Saving Configuration")]
[SerializeField] private float autoSaveTimeSeconds = 60f;


    private GameData gameData;
    private List<IDataPersistence> dataPersistencesObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

   private Coroutine AutoSaveCoroutine;

    public static DataPersistenceManager instance {get; private set;}

    private void Awake()
{
    if(instance != null)
    {
        Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newst one.");
        Destroy(this.gameObject);
        return;
    }
    instance = this;
    DontDestroyOnLoad(this.gameObject);
    if (disableDataPersistence)
    {
        Debug.LogWarning("Data Persistence is currently disabled!");
    }

    this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

    initializeSelectedProfileId();
}

private void OnEnable()
{
  SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
 SceneManager.sceneLoaded -= OnSceneLoaded;
}

public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
   
this.dataPersistencesObjects = FindAllDataPersistenceObjects();
    LoadGame();

    if (AutoSaveCoroutine != null)
{
  StopCoroutine(AutoSaveCoroutine);
}
AutoSaveCoroutine = StartCoroutine(AutoSave());
}


public void ChangeSelectedProfileId(string NewProfileId)
{
    this.selectedProfileId = NewProfileId;
    LoadGame();
}

public void DeleteProfileData(string profileId)
{
  dataHandler.Delete(profileId);

  initializeSelectedProfileId();

  LoadGame();
}

 private void initializeSelectedProfileId()
{
  this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
    if(overrideSelectedProfileId)
    {
        this.selectedProfileId = testSelectedProfileId;
        Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
    }
}

public void NewGame()
{
   this.gameData = new GameData();
}

public void LoadGame()
{
  if (disableDataPersistence)
  {
    return;
  }

    this.gameData = dataHandler.Load(selectedProfileId);
   if (this.gameData == null && initializeDataIfNull)
   {
    NewGame();
   }

   if (this.gameData == null)
   {
    Debug.Log("No data was found. Initializing data to defaults.");
    return;
   }

   foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
   {
    dataPersistenceObj.LoadData(gameData);
   }
    
}

public void SaveGame()
{
 if (disableDataPersistence)
  {
    return;
  }

   if (this.gameData == null)
   {
    Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
    return;
   }

  foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
  {
    dataPersistenceObj.SaveData(gameData);
  }

  gameData.lastUpdated = System.DateTime.Now.ToBinary();


  dataHandler.Save(gameData, selectedProfileId);
}

private void OnApplicationQuit()
{
    SaveGame();
}

private List<IDataPersistence> FindAllDataPersistenceObjects()
{
    IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

    return new List<IDataPersistence>(dataPersistencesObjects);
}
public bool HasGameData()
{
    return gameData != null;
}

public Dictionary<string, GameData> GetAllProfilesGameData()
{
    return dataHandler.LoadAllProfiles();
}

private IEnumerator AutoSave()
{
   while (true)
   {
      yield return new WaitForSeconds(autoSaveTimeSeconds);
      SaveGame();
      Debug.Log("Auto Saved Game");
   }
}
}
