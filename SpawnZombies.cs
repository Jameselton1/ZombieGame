using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    
    public int roundNo = 1;
    
    public Transform[] spawnpoints;

    public GameObject zombiePrefab;

    public bool roomFinished = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        SpawnEnemies();
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        
        while (true)
        {
            // perform every second
            yield return wait;
            if (ZombieNo() == 0)
            {
                // if there have been less than 5 rounds
                if (roundNo < 5)
                {
                    NewRound();
                }
                else
                {
                    FinishRoom();
                }
            }
        }
    }

    // return the number of zombies
    private int ZombieNo()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // start a new round if there are no zombies left
    private void NewRound()
    {
        roundNo++;
        SpawnEnemies();
    }
    
    // spawn enemies at each spawnpoint
    private void SpawnEnemies()
    {
        foreach (var spawnpoint in spawnpoints)
        {
            SpawnZombie(spawnpoint);
        }
    }
    
    // spawn a zombie
    private void SpawnZombie(Transform spawnpoint)
    {
        GameObject zombie = zombiePrefab;
            
        zombie.name = "Zombie";
        zombie.tag = "Enemy";

        Instantiate(zombie, spawnpoint.position, Quaternion.identity);
    }

    // mark the room as finished
    private void FinishRoom()
    {
        GameObject events = GameObject.Find("GameEvents");
        events.GetComponent<GameEvents>().roomFinished = true;
        Destroy(this);
    }
    
}
