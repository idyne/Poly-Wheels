using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

[RequireComponent(typeof(Rigidbody))]
public abstract class Competitor : MonoBehaviour
{
    protected static RaceLevel levelManager = null;
    protected Rigidbody rb = null;
    private List<Tire> tires = new List<Tire>();
    private GameObject currentShape = null;
    private List<Ring> usedRings = new List<Ring>();
    public int Rank = 0;
    public string Name = "Competitor";
    [SerializeField] protected Color color;
    [SerializeField] protected TrailRenderer trail = null;
    [SerializeField] protected float acceleration = 1;
    [SerializeField] private float speedLimit = 1;
    [SerializeField] private GameObject tirePrefab = null;
    [SerializeField] private GameObject[] shapes = null;

    public Rigidbody Rb { get => rb; }

    protected void Awake()
    {
        if (!levelManager)
            levelManager = (RaceLevel)LevelManager.Instance;
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = 5;
    }

    protected void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            AddTire();
        }
    }

    protected void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -levelManager.RaceTrackWidth / 2, levelManager.RaceTrackWidth / 2);
        transform.position = pos;
    }

    protected void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.STARTED)
            MoveForward();
    }

    protected void Move(float rate, float anchor)
    {
        Vector3 pos = transform.position;
        float x = (rate * levelManager.RaceTrackWidth) + anchor;
        pos.x = Mathf.Clamp(x, -levelManager.RaceTrackWidth / 2, levelManager.RaceTrackWidth / 2);
        Collider[] ramps = Physics.OverlapBox(pos, new Vector3(0.2f, 1, 1), Quaternion.identity, levelManager.RoadLayerMask);
        bool canGo = true;
        for (int i = 0; i < ramps.Length; i++)
        {
            if (ramps[i].tag == "Ramp")
            {
                canGo = false;
                break;
            }

        }
        if (canGo && Physics.Raycast(transform.position + Vector3.forward * 1, (pos - transform.position).normalized, out RaycastHit hit, levelManager.RaceTrackWidth, levelManager.RoadLayerMask))
        {
            canGo = false;
        }

        if (canGo)
            transform.position = pos;
    }

    private void MoveForward()
    {
        rb.AddTorque(Vector3.right * acceleration);

        if (rb.velocity.magnitude > speedLimit)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, rb.velocity.normalized * speedLimit, Time.deltaTime * acceleration);
        }
        else
        {
            rb.AddForce(Vector3.forward * acceleration);
        }

    }

    public void AddTire()
    {
        if (tires.Count < 15)
        {
            Tire tire = Instantiate(tirePrefab, transform.position, tirePrefab.transform.rotation, transform).GetComponent<Tire>();
            tire.Competitor = this;
            tires.Add(tire);
            RelocateTires();
            if (tires.Count >= 3)
                SwitchShape();
            if (tires.Count <= 3)
            {
                tire.Initial = true;
            }
            SetSpeedLimit();
        }
        else
        {
            Tire minDurabilityTire = tires[0];
            for (int i = 1; i < tires.Count; i++)
            {
                if (tires[i].Durability < minDurabilityTire.Durability)
                    minDurabilityTire = tires[i];
            }
            minDurabilityTire.Repair();
        }
    }

    public void RemoveTire()
    {
        Tire tire = tires[tires.Count - 1];
        if (RemoveTire(tire))
            Destroy(tire.gameObject);
    }

    public bool RemoveTire(Tire tire)
    {
        if (tires.Count > 3)
        {
            tires.Remove(tire);
            RelocateTires();
            SwitchShape();
            SetSpeedLimit();
            return true;
        }
        return false;
    }

    private void RelocateTires()
    {
        Quaternion rot = transform.rotation;
        transform.rotation = Quaternion.identity;
        for (int i = 0; i < tires.Count; i++)
        {
            tires[i].transform.position = transform.position + Quaternion.Euler(Vector3.right * (360 / tires.Count) * i) * Vector3.forward;
        }
        transform.rotation = rot;
    }

    private void SwitchShape()
    {
        if (currentShape)
            currentShape.SetActive(false);
        currentShape = shapes[tires.Count - 3];
        currentShape.GetComponentInChildren<Renderer>().material.color = color;
        currentShape.SetActive(true);

    }

    protected void Boost()
    {

        Vector3 force = new Vector3(0, rb.velocity.y < 0 ? -rb.velocity.y + 10 : 10, 10);
        LeanTween.value(gameObject, (float value) =>
        {
            speedLimit = value;
        }, speedLimit + force.magnitude, speedLimit, 2);
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void SetSpeedLimit()
    {
        speedLimit = tires.Count * 12f / 3f;
        //speedLimit = 10;
    }



    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CollectibleTire")
            CollectTire(other.transform);
        else
        {
            Ring ring = other.GetComponent<Ring>();
            if (ring)
            {
                Boost();
                ring.PlayEffect();
            }
        }
    }

    private void CollectTire(Transform collectibleTire)
    {
        Destroy(collectibleTire.gameObject);
        AddTire();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.2f, 2, 2));
    }


}
