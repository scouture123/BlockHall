using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Case {

    private int positionX_;
    private int positionY_;
    private bool occuper_ = false;
    private string name_ = "";
    private List<Case> voisins_ = new List<Case> { };

    public int getPositionX() {
        return positionX_;
    }

    public void setPositionX(int positionX) {
        positionX_ = positionX;
    }

    public int getPositionY() {
        return positionY_;
    }

    public void setPositionY(int positionY) {
        positionY_ = positionY;
    }

    public bool getOccuper() {
        return occuper_;
    }

    public void setOccuper(bool occuper) {
        occuper_ = occuper;
    }

    public List<Case> getVoisins() {
        return voisins_;
    }

    public void setVoisins(List<Case> voisins) {
        voisins_ = voisins;
    }

    public void ajouterVoisin(Case voisin) {
        voisins_.Add(voisin);
    }

    public int getLengthVoisins() {
        return voisins_.Count;
    }

    public void setName(string name) {
        name_ = name;
    }

    public string getName() {
        return name_;
    }

}
