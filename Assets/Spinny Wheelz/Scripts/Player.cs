using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Player : Competitor
{
    private Swerve1D swerve = null;
    private float swerveStart = 0;
    private bool finished = false;
    [SerializeField] private GameObject speedEffect = null;

    private new void Awake()
    {
        base.Awake();
        swerve = InputManager.CreateSwerve1D(Vector2.right, Screen.width * 0.4f);
        swerve.OnStart = () => { swerveStart = transform.position.x; };
    }
    protected new void Update()
    {
        base.Update();
        if (GameManager.Instance.State == GameManager.GameState.STARTED)
        {
            CheckInput();
            if (transform.position.z >= levelManager.LevelGenerator.RaceTrackLength && !finished)
            {
                finished = true;
                levelManager.FinishLevel(true);
            }
        }
        if (rb.velocity.magnitude >= 60 && !speedEffect.activeSelf)
            speedEffect.SetActive(true);
        else if (rb.velocity.magnitude < 60 && speedEffect.activeSelf)
            speedEffect.SetActive(false);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.Instance.State == GameManager.GameState.STARTED)
            CheckFixedInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            AddTire();
        else if (Input.GetKeyDown(KeyCode.E))
            RemoveTire();
        else if (Input.GetKeyDown(KeyCode.R))
            Boost();

    }
    private void CheckFixedInput()
    {
        if (swerve.Active)
        {
            Move(swerve.Rate, swerveStart);
        }

    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "CollectibleGem")
        {
            levelManager.CollectedGemCount++;
            print(levelManager.CollectedGemCount);
            other.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(() => { Destroy(other.gameObject); });
        }
    }
}
