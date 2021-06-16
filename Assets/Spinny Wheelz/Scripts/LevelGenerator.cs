using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class LevelGenerator : MonoBehaviour
{
    private RaceLevel levelManager = null;
    [SerializeField] private float raceTrackLength = 1000;
    [Header("Parent Objects")]
    [SerializeField] private Transform roadParent = null;
    [SerializeField] private Transform rampParent = null;
    [SerializeField] private Transform ringParent = null;
    [SerializeField] private Transform bonusParent = null;
    [SerializeField] private Transform finishLineParent = null;
    [SerializeField] private Transform buildingGroupParent = null;
    [SerializeField] private Transform gemParent = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject roadPrefab = null;
    [SerializeField] private GameObject rampPrefab = null;
    [SerializeField] private GameObject ringPrefab = null;
    [SerializeField] private GameObject bonusPrefab = null;
    [SerializeField] private GameObject finishLinePrefab = null;
    [SerializeField] private GameObject buildingGroupPrefab = null;
    [SerializeField] private GameObject gemPrefab = null;


    public float RaceTrackLength { get => raceTrackLength; }

    private void Awake()
    {
        levelManager = (RaceLevel)LevelManager.Instance;
        Generate();
    }
    private void Generate()
    {
        GenerateRoad();
        GenerateRamps();
        GenerateRings();
        GenerateBuildingGroups();
        GenerateGems();
    }

    private void GenerateRoad()
    {
        Transform road = Instantiate(roadPrefab, Vector3.zero, roadPrefab.transform.rotation, roadParent).transform;
        road.GetComponent<Road>().Generate((int)(raceTrackLength / 20));
        Instantiate(finishLinePrefab, Vector3.forward * raceTrackLength, finishLinePrefab.transform.rotation, finishLineParent);
        Instantiate(bonusPrefab, Vector3.forward * raceTrackLength, roadPrefab.transform.rotation, bonusParent);
    }
    private void GenerateRamps()
    {
        float cursor = 15;
        while (cursor < raceTrackLength - 20)
        {
            Instantiate(rampPrefab, new Vector3(Random.Range(-levelManager.RaceTrackWidth / 2 + 1, -2), 0, cursor), rampPrefab.transform.rotation, rampParent);
            cursor += cursor < 150 ? Random.Range(13f, 16f) : Random.Range(50f, 60f);
        }
        cursor = 15;
        while (cursor < raceTrackLength - 20)
        {
            Instantiate(rampPrefab, new Vector3(Random.Range(2, levelManager.RaceTrackWidth / 2 - 1), 0, cursor), rampPrefab.transform.rotation, rampParent);
            cursor += cursor < 150 ? Random.Range(13f, 16f) : Random.Range(50f, 60f);
        }
    }

    private void GenerateRings()
    {
        float cursor = raceTrackLength / 5;
        float lastHeight = Random.Range(5f, 15f);
        while (cursor < raceTrackLength - 20)
        {
            Instantiate(ringPrefab, new Vector3(Random.Range(-levelManager.RaceTrackWidth / 2 + 1, levelManager.RaceTrackWidth / 2 - 1), Mathf.Clamp(lastHeight + Random.Range(-5f, 5f), 5, 15), cursor), ringPrefab.transform.rotation, ringParent);
            cursor += Random.Range(40f, 60f);
        }
    }

    private void GenerateBuildingGroups()
    {
        float cursor = 15;
        while (cursor < raceTrackLength + 1000)
        {
            Instantiate(buildingGroupPrefab, new Vector3(-50, 0, cursor), rampPrefab.transform.rotation, buildingGroupParent);
            Instantiate(buildingGroupPrefab, new Vector3(70, 0, cursor), rampPrefab.transform.rotation, buildingGroupParent);
            cursor += Random.Range(100f, 120f);
        }
    }

    private void GenerateGems()
    {
        float cursor = 15;
        while (cursor < raceTrackLength - 20)
        {
            Instantiate(gemPrefab, new Vector3(Random.Range(-levelManager.RaceTrackWidth / 2 + 1, -2), 0, cursor), gemPrefab.transform.rotation, gemParent);
            cursor += cursor < 150 ? Random.Range(13f, 16f) : Random.Range(50f, 60f);
        }
    }
}
