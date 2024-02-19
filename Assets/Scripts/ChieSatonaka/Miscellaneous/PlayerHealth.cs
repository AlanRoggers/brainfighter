using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text displayHealth;
    public int Health { get; private set; }
    private Components components;
    private int hitTolerance;
    private Coroutine hit_tolerance_reset;
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
        hitTolerance = 2;
        hit_tolerance_reset = null;
    }
    public void BlockedAttack()
    {
        if (hitTolerance > 0)
        {
            if (hit_tolerance_reset != null)
                StopCoroutine(hit_tolerance_reset);
            components.Academy.ManageEvents(AgentEvents.AttackBlocked, gameObject.layer == 6);
            hitTolerance--;
            hit_tolerance_reset = StartCoroutine(HIT_TOLERANCE_RESET());
        }
        else
        {
            if (hit_tolerance_reset != null)
            {
                StopCoroutine(hit_tolerance_reset);
                hit_tolerance_reset = null;
            }
            components.msng.Stuned = true;
            components.msng.IsBlocking = false;
            components.phys.velocity = Vector2.zero;
            StartCoroutine(RECOVER_OF_STUNT());
        }
    }
    private void UpdateDisplay()
    {
        string healthString = gameObject.layer == 6 ? $"P1: {Health}" : $"P2: {Health}";
        displayHealth.text = healthString;
    }
    IEnumerator RECOVER_OF_STUNT()
    {
        yield return new WaitForSecondsRealtime(1.6f);
        components.msng.Stuned = false;
        hitTolerance = 2;
    }
    IEnumerator HIT_TOLERANCE_RESET()
    {
        yield return new WaitForSeconds(2.5f);
        hitTolerance = 4;
        hit_tolerance_reset = null;
    }
}
