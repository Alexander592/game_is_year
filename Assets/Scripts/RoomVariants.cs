using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms; 
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public GameObject key;
    public GameObject gun;

    [HideInInspector] public List<GameObject> rooms;
    
    private void Start() 
    {
        StartCoroutine(KeySpawner());
    }
    IEnumerator KeySpawner()
    {
        yield return new WaitForSeconds(5f);
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        GameObject[] roomsForKey = GameObject.FindGameObjectsWithTag("ManyWays");
        int rand = Random.Range(0, roomsForKey.Length - 1);
        
        Instantiate(key, roomsForKey[rand].transform.position, Quaternion.identity);
        Instantiate(gun, roomsForKey[roomsForKey.Length - 1].transform.position, Quaternion.identity);
        roomsForKey[roomsForKey.Length - 1].GetComponent<AddRoom>().roomBeforeBoss = true;
        
         

         lastRoom.door.SetActive(true);
        lastRoom.bossRoom = true;



       
    }
}
