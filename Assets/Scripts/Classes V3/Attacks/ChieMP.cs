using UnityEngine;

public class ChieMP : Attack
{
    public ChieMP()
    {
        Inertia = Vector2.zero;
        Force = new Vector2(0, 12);
        HitFreeze = true;
        Damage = 5;
        TimesDamageApplied = 1; // Veces que se puede aplicar el daño
        HitStun = 0.2f;// Cuanto se stunea el enemigo
        CoolDown = 0.3f; // Tiempo de enfriamiento para los golpes
        initState = AnimationStates.MiddlePunch;
        endState = AnimationStates.ChainMiddlePunch;
    }
}
