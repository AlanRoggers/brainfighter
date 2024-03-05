using UnityEngine;

public class OverlapDetector
{
    public bool AttackHit(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        // Debug.Log("[Checando Hit]");
        return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer) != null;
    }
    public bool GroundDetection(Transform transform, Vector2 position, Vector2 size, LayerMask ground)
    {
        return Physics2D.OverlapBox((Vector2)transform.position + position, size, 0f, ground) != null;
    }
    public bool EnemyOverlapping(Vector2 position, Vector2 size, LayerMask enemy)
    {
        return Physics2D.OverlapBox(position, size, 0f, enemy) != null;
    }
}
