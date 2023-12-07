using UnityEngine;

public class Components : MonoBehaviour
{
    public bool canCombo;
    public bool isJumping;
    public bool isKicking;
    public bool isWalking;
    public bool onGround;
    public Animator anim;
    public Collisions coll;
    public Kicks kicks;
    public Rigidbody2D phys;
    public StateController states;
    void Awake()
    {
        phys = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collisions>();
        kicks = GetComponent<Kicks>();
        states = GetComponent<StateController>();
    }
}