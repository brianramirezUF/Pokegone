using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    public string name;
    public int damage;
    public int pp;
    public int maxpp;
    public string type;

    public Moves(string name, int damage, int pp, int maxpp, string type)
    {
        name = this.name;
        damage = this.damage;
        pp = this.pp;
        maxpp = this.maxpp;
        type = this.type;
    }

}
