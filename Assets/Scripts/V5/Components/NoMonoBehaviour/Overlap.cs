using UnityEngine;

public class Overlap
{
    private readonly float fixedPos = 2.205f;
    private readonly Vector2 feetsSize = new(1.1f, 0.01f);
    private readonly Vector2 enemyDetectorPos = new(0.3f, 2.5f);
    private readonly Vector2 enemyDetectorSize = new(1, 3);
    public Collider2D AttackHit(LayerMask contactLayer, CircleCollider2D hitbox) =>
        Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);
    public bool GroundDetection(Collider2D collider, LayerMask ground) =>
        Physics2D.OverlapBox(new Vector2(collider.bounds.center.x, collider.bounds.center.y - fixedPos), new Vector2(collider.bounds.size.x, 0.03f), 0f, ground) != null;
    public bool EnemyOverlapping(LayerMask enemy) =>
        Physics2D.OverlapBox(enemyDetectorPos, enemyDetectorSize, 0f, enemy) != null;
    public void DrawGroundDetection(Collider2D collider, LayerMask ground)
    {
        if (GroundDetection(collider, ground))
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector2(collider.bounds.center.x, collider.bounds.center.y - fixedPos), new Vector2(collider.bounds.size.x, 0.03f));
    }
}
