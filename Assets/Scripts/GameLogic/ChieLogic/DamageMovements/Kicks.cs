using System.Collections;
using UnityEngine;

public class Kicks : MonoBehaviour
{
    public bool[] Combo;
    public bool CanComboViewer;
    private float lastTime;
    private float coolDown;
    private CircleCollider2D hitbox;
    private Components components;
    private LayerMask enemyLayer;
    [SerializeField]
    private Vector2[] kickForces;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        Combo = new bool[3];
        coolDown = 0.5f;
        enemyLayer = LayerMask.GetMask("Enemy");
        hitbox = transform.Find("hitbox").GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        CanComboViewer = components.canCombo;
        if (Input.GetKeyDown(KeyCode.L) && !components.isJumping && !components.isKicking)
        {
            // components.canCombo = false; // Estoy teniendo un bug pequeño, es culpa de canCombo, tratar de arreglarla de otra forma
            components.isKicking = true;
            lastTime = Time.time;
            // print("Puedes pegar");
        }

        if (Input.GetKeyDown(KeyCode.K) && components.isKicking && Combo[0] && ComboTime() && !Combo[1])
        {
            lastTime = Time.time;
            components.canCombo = true;
            // print("MiddleKick");
        }

        if (Input.GetKeyDown(KeyCode.I) && components.isKicking && Combo[1] && ComboTime() && !Combo[2])
        {
            lastTime = Time.time;
            components.canCombo = true;
            // print("HardKick");
        }
    }
    public void ChangeForceOrientation()
    {
        for (int i = 0; i < kickForces.Length; i++)
        {
            kickForces[i].x *= -1;
        }
    }
    public void KickPhys(AnimationStates kick)
    {
        switch (kick)
        {
            case AnimationStates.Kickl:
                components.phys.velocity = Vector2.zero;
                components.phys.AddForce(kickForces[0], ForceMode2D.Impulse);
                break;
            case AnimationStates.Kickm:
                components.phys.AddForce(kickForces[1], ForceMode2D.Impulse);
                break;
            case AnimationStates.Kickh:
                components.phys.AddForce(kickForces[2], ForceMode2D.Impulse);
                break;
        }
    }
    public void RestartCombo(int index)
    {
        components.canCombo = false;
        if (index >= 0 && index < Combo.Length)
            Combo[index] = false;

        if (index + 1 < Combo.Length)
            if (Combo[index + 1])
                return;

        components.isKicking = false;

        // StartCoroutine(KicksCoolDown());
    }
    public IEnumerator CHECK_DAMAGE(AnimationStates attack)
    {
        yield return null; //Esperar a el siguiente frame para la nueva animación

        int damage = attack == AnimationStates.Kickl ? 10 :
                        attack == AnimationStates.Kickm ? 15 :
                        attack == AnimationStates.Kickh ? 20 : 0;
        Vector2 inertia = attack == AnimationStates.Kickl ? new Vector2(-kickForces[0].x, 0) :
                            attack == AnimationStates.Kickm ? new Vector2(-kickForces[1].x, 0) :
                            attack == AnimationStates.Kickh ? new Vector2(-kickForces[2].x, kickForces[2].y) : Vector2.zero;

        while (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            print("Comprobar daño");
            GameObject enemy = ColliderChecker();
            if (enemy != null)
            {
                DoDamage(enemy, damage);
                enemy.GetComponent<Rigidbody2D>().AddForce(inertia, ForceMode2D.Impulse);
                components.states.check_damage = null;
                break;
            }
            yield return null;
        }

    }
    private void DoDamage(GameObject enemy, int damage)
    {
        enemy.GetComponent<Health>().ReduceHealth(damage);
    }
    private bool ComboTime()
    {
        return Time.time - lastTime >= coolDown &&
                components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }
    private GameObject ColliderChecker()
    {
        if (Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, enemyLayer) != null)
            return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, enemyLayer).gameObject;
        else
            return null;
    }
}