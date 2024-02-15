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
        if (displayHealth != null)
            UpdateDisplay();
    }
    void Update()
    {
        if (Health <= 0)
        {
            components.msng.Dead = true;
            components.Academy.ManageEvents(AgentEvents.Loss, gameObject.layer == 6);
        }
    }
    void LateUpdate()
    {
        if (displayHealth != null)
            UpdateDisplay();
    }
    public void ReduceHealth(int damage)
    {
        if (damage == 1)
            components.Academy.ManageEvents(AgentEvents.AttackBlocked, gameObject.layer == 6);

        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;

        components.Academy.ManageEvents(AgentEvents.ReceivedDamage, gameObject.layer == 6, damage);
    }
    public void NewLife()
    {
        Health = 100;
    }
    private void UpdateDisplay()
    {
        string healthString = gameObject.layer == 6 ? $"P1: {Health}" : $"P2: {Health}";
        displayHealth.text = healthString;
    }
}
