using UnityEngine;

public class Components : MonoBehaviour
{
    public Animator anim;
    public ClassMessenger msng;
    public CollisionDetector coll;
    public Motion motion;
    public Attacks attacks;
    public PlayerHealth Health;
    public PPOAgent BrainAgent;
    public Rigidbody2D phys;
    public StateController states;
    public UserInput Input;
    void Awake()
    {
        phys = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CollisionDetector>();
        states = GetComponent<StateController>();
        msng = GetComponent<ClassMessenger>();
        motion = GetComponent<Motion>();
        attacks = GetComponent<Attacks>();
        Health = GetComponent<PlayerHealth>();
        Input = GetComponent<UserInput>();
        BrainAgent = GetComponent<PPOAgent>();
    }
}