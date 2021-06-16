using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames;
using System.Linq;
using TMPro;

public class RaceLevel : LevelManager
{
    [SerializeField] private GameObject[] buildingPrefabs = null;
    [SerializeField] private float raceTrackWidth = 15;
    [SerializeField] private LayerMask roadLayerMask = 0;
    [SerializeField] private Gradient tireGradient = null;
    [SerializeField] private Color[] botColors = null;
    [SerializeField] private Slider progressSlider = null;
    [SerializeField] private GameObject progressBar = null;
    [SerializeField] private TextMeshProUGUI rankText = null;
    [SerializeField] private Text gemText = null;
    private LevelGenerator levelGenerator = null;
    private GameObject[] collectibleTires = null;
    private GameObject[] ramps = null;
    private Competitor[] competitors = null;
    private Player player = null;
    private int collectedGemCount = 0;
    private CameraFollow cameraFollow = null;
    private Countdown countdown = null;

    public float RaceTrackWidth { get => raceTrackWidth; }
    public LayerMask RoadLayerMask { get => roadLayerMask; }
    public Gradient TireGradient { get => tireGradient; }
    public GameObject[] Ramps { get => ramps; }
    public Color[] BotColors { get => botColors; }
    public Player Player { get => player; }
    public GameObject[] CollectibleTires { get => collectibleTires; }
    public Competitor[] Competitors { get => competitors; }
    public LevelGenerator LevelGenerator { get => levelGenerator; }
    public GameObject[] BuildingPrefabs { get => buildingPrefabs; }
    public int CollectedGemCount { get => collectedGemCount; set => collectedGemCount = value; }

    private new void Awake()
    {
        base.Awake();

        levelGenerator = FindObjectOfType<LevelGenerator>();
        player = FindObjectOfType<Player>();
        competitors = FindObjectsOfType<Competitor>();
        Bot.IndexCount = 0;
        gemText.text = GameManager.GEM.ToString();
        cameraFollow = FindObjectOfType<CameraFollow>();
        countdown = FindObjectOfType<Countdown>();
    }
    private void Start()
    {
        //collectibleTires = GameObject.FindGameObjectsWithTag("CollectibleTire");
        ramps = GameObject.FindGameObjectsWithTag("Ramp");

    }
    public override void FinishLevel(bool success)
    {
        GameManager.Instance.State = GameManager.GameState.FINISHED;

        StartCoroutine(Finish());
    }

    public override void StartLevel()
    {
        cameraFollow.TakeRotation();
        cameraFollow.Target = player.transform;

        countdown.StartCountdown();
        LeanTween.delayedCall(3, () =>
        {
            foreach (Competitor competitor in competitors)
            {
                competitor.Rb.isKinematic = false;
            }
        });

    }

    private void Update()
    {
        progressSlider.value = player.transform.position.z / levelGenerator.RaceTrackLength;
        if (GameManager.Instance.State != GameManager.GameState.NOT_STARTED || GameManager.Instance.State != GameManager.GameState.PAUSED)
        {
            List<Competitor> competitors = this.competitors.OrderBy(o => o.transform.position.z).ToList();
            for (int i = 0; i < competitors.Count; i++)
            {
                competitors[i].Rank = competitors.Count - i;
            }
        }
        rankText.text = player.Rank > 0 ? player.Rank.ToString() + (player.Rank == 1 ? "st" : player.Rank == 2 ? "nd" : player.Rank == 3 ? "rd" : "th") : "";
    }

    private IEnumerator Finish()
    {
        yield return new WaitUntil(() => player.Rb.velocity.magnitude <= 0.5f);
        progressBar.SetActive(false);
        bool success = player.Rank <= 3;
        if (success)
        {
            GameManager.GEM += (int)(collectedGemCount * (((player.transform.position.z - levelGenerator.RaceTrackLength) / 12.6f) * 0.3f + 1));
        }
        GameManager.Instance.FinishLevel(success);
    }
}
