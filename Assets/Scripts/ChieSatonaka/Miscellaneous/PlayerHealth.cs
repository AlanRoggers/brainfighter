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
        Health = 100;
        UpdateDisplay();
    }
    void Update()
    {
        if (Health == 0)
        {
            components.msng.Dead = true;
        }
    }
    public void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;

        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        string healthString = gameObject.layer == 6 ? $"P1: {Health}" : $"P2: {Health}";
        displayHealth.text = healthString;
    }
}
