using System.Collections.Generic;
using UnityEngine;

public class Attack : Command
{
    public readonly bool HitFreeze;
    public readonly int Damage;
    public readonly int ResistanceGained;
    public readonly float HitFreezeTimer;
    public readonly float HitStun;
    public readonly float CoolDown;
    public readonly Vector2 Inertia;
    public readonly Vector2 Force;
    public readonly int TimesDamageApplied;
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
    public Attack(bool hitF, int dmg, int timeDmg, int resGained, float hitS, float cd, float hitFT, Vector2 inertia, Vector2 force, List<AnimationStates> actionStates) : base(actionStates)
    {
        HitFreeze = hitF;
        Damage = dmg;
        HitStun = hitS;
        CoolDown = cd;
        Inertia = inertia;
        Force = force;
        TimesDamageApplied = timeDmg;
        HitFreezeTimer = hitFT;
        ResistanceGained = resGained;
    }
}
