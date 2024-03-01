using System.Collections.Generic;
using UnityEngine;

public class AttackV4 : Command
{
    public bool HitFreeze { get; private set; }
    public int Damage { get; private set; }
    public float HitStun { get; private set; }
    public float CoolDown { get; private set; }
    public Vector2 Inertia { get; private set; }
    public Vector2 Force { get; private set; }
    public int TimesDamageApplied;
    /// <summary>
    /// Ataque
    /// </summary>
    /// <param name="hitF">El ataque puede congelar animaciones</param>
    /// <param name="dmg">Daño del ataque</param>
    /// <param name="timeDmg">Veces que el ataque puede aplicar daño</param>
    /// <param name="hitS">Cantidad de tiempo que se deja incapacitado al enemigo</param>
    /// <param name="cd">Enfriamiento que se aplica a todos los ataques ejecutando este ataque</param>
    /// <param name="inertia">Inercia aplicada al personaje que ejecuta este ataque</param>
    /// <param name="force">Fuerza que se le aplica al enemigo</param>
    /// <param name="actionStates">Animaciones del ataque</param>
    public AttackV4(bool hitF, int dmg, int timeDmg, float hitS, float cd, Vector2 inertia, Vector2 force, List<AnimationStates> actionStates) : base(actionStates)
    {
        HitFreeze = hitF;
        Damage = dmg;
        HitStun = hitS;
        CoolDown = cd;
        Inertia = inertia;
        Force = force;
        TimesDamageApplied = timeDmg;
    }
}
