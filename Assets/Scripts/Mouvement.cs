using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Mouvement
{
    public int Departx { get; set; }
    public int Departy { get; set; }
    public int Finx { get; set; }
    public int Finy { get; set; }
    public int jetonx { get; set; }
    public int jetony { get; set; }

    public Mouvement()
    {
        Departx = 0;
        Departy = 0;
        Finx = 0;
        Finy = 0;
        jetonx = 0;
        jetony = 0;
    }

    public Mouvement(int x1, int y1, int x2, int y2)
    {
        Departx = x1;
        Departy = y1;
        Finx = x2;
        Finy = y2;
    }

    public Mouvement(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        Departx = x1;
        Departy = y1;
        Finx = x2;
        Finy = y2;
        jetonx = x3;
        jetony = y3;
    }

    public void afficher()
    {
        Debug.Log("Mouvement: (" + Departx + ", " + Departy + ", " + Finx + ", " + Finy + ")" + " jeton: (" + jetonx + ", " + jetony + ")");
    }
}

