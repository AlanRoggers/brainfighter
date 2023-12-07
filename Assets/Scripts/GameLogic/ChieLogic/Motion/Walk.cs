using UnityEngine;
public class Walk : MonoBehaviour
{
    private int maxSpeed;
    private float walkForce;
    private Components components;
    void Awake()
    {
        Application.targetFrameRate = 60;
        components = GetComponent<Components>();
    }
    void Start()
    {
        maxSpeed = 5;
        walkForce = 100;
    }
    void Update()
    {
        Gameplay();
    }
    public void WalkLogic(int direction)
    {
        if (direction != 0)
        {
            if (!components.isKicking)
            {
                components.isWalking = true;
                if (Mathf.Abs(components.phys.velocity.x) < maxSpeed)
                {
                    float speed = components.phys.velocity.x + (1 * walkForce * direction * Time.deltaTime);
                    components.phys.velocity = new Vector2(speed, components.phys.velocity.y);
                }
                else components.phys.velocity = new Vector2(maxSpeed * direction, components.phys.velocity.y);
            }
            else
            {
                components.isWalking = false;
            }
        }
        else
        {
            components.isWalking = false;
            if (!components.isKicking)
                components.phys.velocity = new Vector2(0, components.phys.velocity.y);
            // print("Se esta cancelando el movimiento");
        }

    }
    private void Gameplay()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        int direction = !right && left ? -1 : right && !left ? 1 : 0;
        WalkLogic(direction);
    }
}
