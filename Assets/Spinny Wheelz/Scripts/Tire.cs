using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Tire : MonoBehaviour
{
    [HideInInspector] public Competitor Competitor = null;
    [HideInInspector] public bool Initial = false;
    private bool departed = false;
    private static float maxDurability = 1f;
    private float durability = 0;
    private Rigidbody rb = null;
    private static RaceLevel levelManager = null;
    [SerializeField] private GameObject departedColliderObject = null;
    [SerializeField] private Renderer rend = null;

    public float Durability { get => durability; }

    private void Awake()
    {
        if (!levelManager)
            levelManager = (RaceLevel)LevelManager.Instance;
        Repair();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!Initial)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one, Quaternion.identity, levelManager.RoadLayerMask);
            if (colliders.Length > 0)
            {
                durability -= Time.deltaTime;
            }
            rend.material.color = levelManager.TireGradient.Evaluate(durability / maxDurability);
            if (!departed && durability <= 0)
            {
                Depart();
                return;
            }
        }
    }

    public void Repair()
    {
        durability = maxDurability;
    }

    private void Depart()
    {
        departed = true;
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(new Vector3(Random.Range(-3f, 3f), Random.Range(5f, 10f), Random.Range(5f, 10f)), ForceMode.Impulse);
        departedColliderObject.SetActive(true);
        Competitor.RemoveTire(this);
        LeanTween.delayedCall(4, () => { transform.LeanScale(Vector3.zero, 0.3f).setOnComplete(() => { Destroy(gameObject); }); });
    }

}
