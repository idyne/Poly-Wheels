using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Bot : Competitor
{
    private State state = State.IDLE;
    private float maxCooldown = 0.6f;
    private float minCooldown = 0.5f;
    private float cooldown = 0;
    public static int IndexCount = 0;
    private int index = 0;

    private new void Awake()
    {
        base.Awake();
        minCooldown = Random.Range(0.8f, 1.6f);
        maxCooldown = minCooldown + 0.2f;
        index = IndexCount;
        IndexCount++;
        SetColor();
        acceleration *= Random.Range(0.8f, 1f);
    }

    private new void Update()
    {
        base.Update();
        if (GameManager.Instance.State == GameManager.GameState.STARTED)
        {
            if (cooldown <= 0)
                Decide();
            else
                cooldown -= Time.deltaTime;
        }

    }
    private void Decide()
    {
        if (state == State.IDLE)
        {
            state = State.MOVING;
            float anchor = transform.position.x;
            float target;
            if (transform.position.z < levelManager.LevelGenerator.RaceTrackLength)
            {
                if (rb.velocity.magnitude <= 0.1f)
                {
                    target = Random.Range(-levelManager.RaceTrackWidth / 2, levelManager.RaceTrackWidth / 2);
                }
                else if (levelManager.Ramps.Length > 0)
                {
                    GameObject closestRamp = levelManager.Ramps[0];
                    float closestDistance = Vector3.Distance(levelManager.Ramps[0].transform.position, transform.position);
                    for (int i = 1; i < levelManager.Ramps.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, levelManager.Ramps[i].transform.position);
                        if (levelManager.Ramps[i].transform.position.z > transform.position.z + 3 && distance < closestDistance)
                        {
                            closestRamp = levelManager.Ramps[i];
                            closestDistance = distance;
                        }
                    }
                    target = closestRamp.transform.position.x;
                }
                else
                {
                    target = transform.position.x;
                }
            }
            else
            {
                target = Random.Range(-levelManager.RaceTrackWidth / 2, levelManager.RaceTrackWidth / 2);
            }


            LeanTween.value(gameObject, (float value) =>
            {
                Move(value * (target - anchor) / levelManager.RaceTrackWidth, anchor);
            }, 0, 1, 0.6f).setOnComplete(() =>
            {
                state = State.IDLE;
                ResetCooldown();
            });
        }
    }

    private void ResetCooldown()
    {
        cooldown = Random.Range(minCooldown, maxCooldown);
    }

    private void SetColor()
    {
        color = levelManager.BotColors[index];
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[2];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        for (int i = 0; i < 2; i++)
        {
            colorKey[i].color = levelManager.BotColors[index];
            colorKey[i].time = i;
            alphaKey[i].alpha = 1 - i;
            alphaKey[i].time = i;
        }
        gradient.colorKeys = colorKey;
        gradient.alphaKeys = alphaKey;
        trail.colorGradient = gradient;
    }
    private enum State { MOVING, IDLE }
}
