using System.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    IEnumerator Clear_Attack(int attack)
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        components.msng.AttackChain[attack] = false;
    }
    #region Punchs
    public void LowPunch()
    {
        components.msng.AttackChain[0] = true;
        StartCoroutine(Clear_Attack(0));
        // Fisicas del primer golpe
    }
    public void MiddlePunch()
    {
        components.msng.AttackChain[1] = true;
        StartCoroutine(Clear_Attack(1));
        // Fisicas del segundo golpe
    }
    public void HardPunch()
    {
        components.msng.AttackChain[2] = true;
        StartCoroutine(Clear_Attack(2));
    }
    #endregion
}