using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name;
    public int Health { get; private set; }
    protected Dictionary<string, List<Atack>> attacks;

}
