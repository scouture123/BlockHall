using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

    private int positionX_;
    private int positionY_;
    private bool isWhite_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public int getPositionX() {
        return positionX_;
    }
    public int getPositionY() {
        return positionY_;
    }
    public void setPositionX(int positionX) {
        positionX_ = positionX;
    }
    public void setPositionY(int positionY) {
        positionY_ = positionY;
    }
    public bool getIsWhite() {
        return isWhite_;
    }
    public void setIsWhite(bool isWhite) {
        isWhite_ = isWhite;
    }
}
