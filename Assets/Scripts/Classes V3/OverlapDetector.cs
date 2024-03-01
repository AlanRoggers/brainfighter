using UnityEngine;

public class OverlapDetector
{
    public Collider2D AttackHit(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);
    }
}
