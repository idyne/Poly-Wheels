using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using System.Linq;

public class BuildingGroup : MonoBehaviour
{
    private static RaceLevel levelManager = null;
    private static Vector3[] positions = null;

    private void Awake()
    {
        if (!levelManager)
            levelManager = (RaceLevel)LevelManager.Instance;
        if (positions == null)
        {
            positions = new Vector3[7];
            positions[0] = new Vector3(-40, -50, -5);
            positions[1] = new Vector3(-10, -50, -15);
            positions[2] = new Vector3(30, -50, -20);
            positions[3] = new Vector3(-5, -50, 5);
            positions[4] = new Vector3(25, -50, 20);
            positions[5] = new Vector3(-15, -50, 35);
            positions[6] = new Vector3(10, -50, 40);
        }
        Generate();
    }


    private void Generate()
    {
        List<Vector3> shuffledList = positions.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < shuffledList.Count; i++)
        {
            Instantiate(levelManager.BuildingPrefabs[i], transform.position + shuffledList[i], levelManager.BuildingPrefabs[i].transform.rotation, transform);
        }
    }
}
