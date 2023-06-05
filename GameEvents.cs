using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameEvents : MonoBehaviour
{
    public GameObject[] rooms;

    // number of rooms
    private int roomNo;

    public LayerMask layerMask;
    
    private Vector3 currentRoomPosition = Vector3.zero;

    public bool roomFinished = false;

    void Start()
    {
        currentRoomPosition.y += -2;
        SpawnRoom();
        StartCoroutine(routine());
    }
    
    IEnumerator routine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        
        while (true)
        {
            // every second
            yield return wait;
            if (roomFinished)
            {
                SpawnRoom();
                // reset roomFinished because there is a new room
                roomFinished = false; 
            }
        }
    }
    
    private void SpawnRoom()
    {
        GameObject room = PickRoomType();
        // give each room a sequential name
        room.name = "room_" + roomNo;

        // generate a random number from 0 to 3
        int direction = GenerateDirection();
        
        // position of the new room (-500, -500, -500 will be returned if there are no available spaces)
        Vector3 position = NewRoomPosition(direction);

        // if position isn't null
        if (!(position == new Vector3(-500, -500, -500)))
        {
            Instantiate(room, position, Quaternion.identity);
            if (roomNo != 0) OpenDoor(direction);
            currentRoomPosition = position;
            roomNo++;
        }
    }
 
    // pick the prefab room which the next room will be
    private GameObject PickRoomType()
    {
        return rooms[Random.Range(0, rooms.Length)];
    }

    // return a vector 3 which is in an adjacent position of the current room.
    private Vector3 NewRoomPosition(int direction)
    {
        if (roomNo == 0) return Vector3.zero;
        
        Vector3 position = currentRoomPosition;
        switch (direction)
        {
            case 0:
                position.x += -50;
                break;
            case 1:
                position.z += 50;
                break;
            case 2:
                position.x += 50;
                break;
            case 3:
                position.z += -50;
                break;
            // no room available
            case -1:
                return new Vector3(-500, -500, -500);
                break;
        }

        return position;
    }
    
    // Directions: 0 = left, 1 = up, 2 = right, 3 = down
    private int GenerateDirection()
    {
        // create a 4 long array of ints with numbers 1-4 in random order
        // go through each number in the array one by one and use roomispresent to check if it is available
        // if none are available return -1
        // if one is available return the value of the first available

        int[] randomOrder = RandomIntArray(4);

        for (var i = 0; i < randomOrder.Length; i++)
        {
            if (!RoomIsPresent(NewRoomPosition(randomOrder[i])))
            {
                return randomOrder[i];
            }
        }

        return -1;
    }

    // return an integer array of random values;
    private int[] RandomIntArray(int length)
    {
        int[] randomOrder = NewIntArray(length);
        
        for (var i = 0; i < randomOrder.Length; i++)
        {
            bool valid = false;

            while (!valid)
            {
                int random = Random.Range(0, length);
                
                if (!randomOrder.Contains(random))
                {
                    valid = true;
                    randomOrder[i] = random;
                }
            }
        }

        return randomOrder;
    }

    // return an integer array of the length of the int param, which has default values of -1
    private int[] NewIntArray(int length)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; i++) array[i] = -1;
        return array;
    }

    // use a raycast to check if a room is present at the position of the parameter vector3
    private bool RoomIsPresent(Vector3 origin)
    {
        origin.x += 25;
        origin.z -= 25;
        origin.y += 10;
        return Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 50f, layerMask);
    }

    private void OpenDoor(int direction)
    {
        GameObject room = GameObject.Find("room_" + (roomNo-1) + "(Clone)");
        GameObject new_room = GameObject.Find("room_" + (roomNo) + "(Clone)");
        
        GameObject door = room.transform.GetChild(direction).gameObject;
        GameObject new_door = new_room.transform.GetChild(NewRoomDoor(direction)).gameObject;
        door.transform.position = new Vector3(-500, -500, -500);
        new_door.transform.position = new Vector3(-500, -500, -500);
        
    }

    private int NewRoomDoor(int direction)
    {
        switch (direction)
        {
            case 0:
                return 2;
                break;
            case 1:
                return 3;
                break;
            case 2:
                return 0;
                break;
            case 3:
                return 1;
                break;
        }

        return -1;
    }

}
