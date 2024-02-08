using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text displayHealth;
    public int Health { get; private set; }
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        NewLife();
        UpdateDisplay();
    }
    void Update()
    {
        if (IsDead())
        {
            components.msng.Dead = true;
        }
    }
    void FixedUpdate()
    {
        if (IsDead())
        {
            components.BrainAgent.AddReward(-100f);
            components.BrainAgent.EndEpisode();
        }
    }
    public void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
        {
            Health -= damage;
            components.BrainAgent.AddReward(-0.1f * damage);
        }
        else
        {
            Health = 0;
        }
        UpdateDisplay();
    }
    public bool IsDead() => Health == 0;
    public void NewLife() => Health = 100;
    private void UpdateDisplay()
    {
        string healthString = gameObject.layer == 6 ? $"P1: {Health}" : $"P2: {Health}";
        displayHealth.text = healthString;
    }
}
