using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField]
    GameObject[] floors;
    GameMasterController gameMaster;
    [SerializeField]
    float floorSize;
    GameObject floorTrigger;

    [Header("Floor Population")]
    public float jackLanturnDensity = 0.1f;
    public float treeDensity = 1f;
    public float graveDensity = 1f;
    public float grassDensity = 1f;

    [Header("Spawn Prefabs")]
    public GameObject Jack;
    public GameObject Lanturn;
    public GameObject[] trees;
    public GameObject[] graves;
    public GameObject[] grass;

    // Start is called before the first frame update
    void Start()
    {
        floorTrigger = gameObject.transform.GetChild(0).gameObject;
        gameMaster = FindObjectOfType<GameMasterController>();
        floorSize = Mathf.Abs(transform.GetChild(1).GetChild(0).position.x);
        
        floors = new GameObject[transform.GetChild(1).childCount];
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] = transform.GetChild(1).GetChild(i).gameObject;
            PopulateFloors(floors[i]);
        }

    }

    public void FloorTriggered(Vector3 playerPos)
    {
        Vector3 relativePos = playerPos - floorTrigger.transform.position;
        
        if(relativePos.z > (floorSize / 2.0))
        {
            gameMaster.GetComponent<GameMasterController>().SetActiveFloor("N");
            GameObject[] FloorsToMove = { floors[6], floors[7], floors[8] };
            foreach (GameObject floor in FloorsToMove)
            {
                floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, floor.transform.position.z + floorSize * 3f);
            }
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z + floorSize);
            GameObject[] tempFloors = { floors[6], floors[7], floors[8], floors[0], floors[1], floors[2], floors[3], floors[4], floors[5] };
            floors = tempFloors;
        }
        else if (relativePos.z < -(floorSize / 2.0))
        {
            gameMaster.GetComponent<GameMasterController>().SetActiveFloor("S");
            GameObject[] FloorsToMove = { floors[0], floors[1], floors[2] };
            foreach (GameObject floor in FloorsToMove)
            {
                floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, floor.transform.position.z - floorSize * 3f);
            }
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z - floorSize);
            GameObject[] tempFloors = { floors[3], floors[4], floors[5], floors[6], floors[7], floors[8], floors[0], floors[1], floors[2] };
            floors = tempFloors;
        }
        if (relativePos.x > (floorSize / 2.0))
        {
            gameMaster.GetComponent<GameMasterController>().SetActiveFloor("E");
            GameObject[] FloorsToMove = { floors[0], floors[3], floors[6] };
            foreach (GameObject floor in FloorsToMove)
            {
                floor.transform.position = new Vector3(floor.transform.position.x + floorSize * 3f, floor.transform.position.y, floor.transform.position.z);
            }
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x + floorSize, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
            GameObject[] tempFloors = { floors[1], floors[2], floors[0], floors[4], floors[5], floors[3], floors[7], floors[8], floors[6] };
            floors = tempFloors;
        }
        else if (relativePos.x < -(floorSize / 2.0))
        {
            gameMaster.GetComponent<GameMasterController>().SetActiveFloor( "W");
            GameObject[] FloorsToMove = { floors[2], floors[5], floors[8] };
            foreach (GameObject floor in FloorsToMove)
            {
                floor.transform.position = new Vector3(floor.transform.position.x - floorSize * 3f, floor.transform.position.y, floor.transform.position.z);
            }
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x - floorSize, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
            GameObject[] tempFloors = { floors[2], floors[0], floors[1], floors[5], floors[3], floors[4], floors[8], floors[6], floors[7] };
            floors = tempFloors;
        }
    }

    public GameObject[] GetFloors()
    {
        return floors;
    }

    void PopulateFloors(GameObject floor)
    {
        Transform[] TreeSpawnPoints = floor.transform.GetChild(1).GetComponentsInChildren<Transform>();
        Transform[] GraveSpawnPoints = floor.transform.GetChild(2).GetComponentsInChildren<Transform>();
        Transform[] GrassSpawnPoints = floor.transform.GetChild(3).GetComponentsInChildren<Transform>();

        foreach(Transform point in TreeSpawnPoints)
        {
            if(point == TreeSpawnPoints[0]) continue; // getComponentsInChildren includes self.
            int randomSpawn = Random.Range(0, 100);
            int randomPrefab = Random.Range(0, trees.Length - 1);
            if (randomSpawn < (int)(treeDensity * 100))
            {
                GameObject tree = Instantiate(trees[randomPrefab], point.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), point.transform);
                randomSpawn = Random.Range(0, 100);
                if (randomSpawn < (int)(jackLanturnDensity * 100))
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Instantiate(Jack, tree.transform.GetChild(0).transform);
                    }
                    else
                    {
                        Instantiate(Lanturn, tree.transform.GetChild(0).transform);
                    }
                }
            }
            
        }

        foreach (Transform point in GraveSpawnPoints)
        {
            if (point == GraveSpawnPoints[0]) continue; // getComponentsInChildren includes self.
            int randomSpawn = Random.Range(0, 100);
            int randomPrefab = Random.Range(0, graves.Length - 1);
            if (randomSpawn < (int)(graveDensity * 100)) Instantiate(graves[randomPrefab], point.position, Quaternion.Euler(new Vector3(Random.Range(0,10), Random.Range(0, 360), Random.Range(0, 10))), floor.transform.GetChild(2).transform);
        }

        foreach (Transform point in GrassSpawnPoints)
        {
            if (point == GrassSpawnPoints[0]) continue; // getComponentsInChildren includes self.
            int randomSpawn = Random.Range(0, 100);
            int randomPrefab = Random.Range(0, grass.Length - 1);
            if (randomSpawn < (int)(grassDensity * 100)) Instantiate(grass[randomPrefab], point.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), floor.transform.GetChild(3).transform);
        }

    }
}
