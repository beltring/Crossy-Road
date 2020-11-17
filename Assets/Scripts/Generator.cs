using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private ObjectPooler OP;
    private List<GameObject> currentTerrains = new List<GameObject>();
    
    [Header("Minimum distance from player")]
    public int minDistanceFromPlayer;

    [HideInInspector]
    public Vector3 currentPosition = new Vector3(0, 0, 0);

    [Header("Maximum number of terrain elements")]
    public int maxTerrainCount;

    void Start()
    {
        OP = ObjectPooler.SharedInstance;

        for (int i = 0; i < maxTerrainCount; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0));
        }
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            this.SpawnTerrain(false, new Vector3(0, 0, 0));
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.SpawnTerrain(false, new Vector3(0, 0, 0));
        }
#endif
    }

    public void SpawnTerrain( bool isStart, Vector3 playerPosition)
    {
        if ((currentPosition.x - playerPosition.x < minDistanceFromPlayer) || isStart)
        {
            GameObject terrain = OP.GetPooledObject(Random.Range(0, 3));
            currentTerrains.Add(terrain);
            terrain.transform.position = currentPosition;
            terrain.SetActive(true);

            if (!isStart)
            {
                if (currentTerrains.Count > maxTerrainCount)
                {
                    currentTerrains[0].SetActive(false);
                    currentTerrains.RemoveAt(0);
                }
            }

            currentPosition.x++;
        }
    }
}
