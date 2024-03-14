using UnityEngine;

public class Overlap
{
    private readonly float fixedPos = 2.205f;
    private readonly Vector2 enemyDetectorPos = new(0.3f, 0f);
    private readonly Vector2 enemyDetectorSize = new(1, 3);
    public Collider2D AttackHit(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        Collider2D a = Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);
        if (a != null)
            Debug.Log($"Collisiono con: {a.name}");
        return a;
    }
    public bool GroundDetection(Collider2D collider, LayerMask ground) =>
        Physics2D.OverlapBox(new Vector2(collider.bounds.center.x, collider.bounds.center.y - fixedPos), new Vector2(collider.bounds.size.x, 0.03f), 0f, ground) != null;
    public bool EnemyOverlapping(Collider2D collider, LayerMask contact) =>
        Physics2D.OverlapBox(
            new Vector2(collider.bounds.center.x + (collider.transform.localScale.x > 0 ? 0.3f : -0.3f), collider.bounds.center.y),
            enemyDetectorSize,
            0,
            contact) != null;
    public void DrawGroundDetection(Collider2D collider, LayerMask ground)
    {
        if (GroundDetection(collider, ground))
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector2(collider.bounds.center.x, collider.bounds.center.y - fixedPos), new Vector2(collider.bounds.size.x, 0.03f));
    }
    public void DrawEnemyOverlapping(Collider2D collider, LayerMask layer)
    {
        if (EnemyOverlapping(collider, layer))
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector2(collider.bounds.center.x + (collider.transform.localScale.x > 0 ? 1 : -1), collider.bounds.center.y), enemyDetectorSize);
    }
    public void DrawHitBox(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        Collider2D x = AttackHit(contactLayer, hitbox);
        // Debug.Log($"Collider de Gizmos: {x}");
        if (x)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitbox.bounds.center, hitbox.radius);
    }
}
