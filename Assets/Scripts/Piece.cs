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

    public bool ValidMove(Piece[,] boardPiece, Jeton[,] boardJetons, int x1, int y1, int x2, int y2) {
        if (boardPiece[x2,y2]  != null || boardJetons[x2,y2] != null) {
            return false; // est sur une autre piece
        }

        int deltaMoveX = x2 - x1;
        int deltaMoveY = y2 - y1;

        if (Mathf.Abs(deltaMoveY) == Mathf.Abs(deltaMoveX)) {
            if (deltaMoveX > 0 && deltaMoveY > 0) { // mouvement diagonale haut droite
                for (int i = x1 + 1, j = y1 + 1; i < x2 || j < y2; i++, j++) {
                    if (boardJetons[i,j] != null || boardPiece[i, j] != null) {
                        Debug.Log("board[" + i + "," + j +"] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX < 0 && deltaMoveY > 0) { // mouvement diagonale haut gauche
                for (int i = x1 - 1, j = y1 + 1; i > x2 || j < y2; i--, j++) {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null) {
                        Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX > 0 && deltaMoveY < 0) { // mouvement diagonale bas droite
                for (int i = x1 + 1, j = y1 - 1; i < x2 || j > y2; i++, j--) {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null) {
                        Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX < 0 && deltaMoveY < 0) { // mouvement diagonale bas gauche
                for (int i = x1 - 1, j = y1 - 1; i > x2 || j > y2; i--, j--) {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null) {
                        Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            return true;
        }
        if (deltaMoveY == 0 && deltaMoveX > 0) {    // mouvement droite
            for (int i = x1 + 1; i < x2; i++) {
                if (boardJetons[i, y2] != null || boardPiece[i, y2] != null) {
                    Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveY == 0 && deltaMoveX < 0) {    // mouvement gauche
            for (int i = x1 - 1; i > x2; i--) {
                if (boardJetons[i, y2] != null || boardPiece[i, y2] != null) {
                    Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveX == 0 && deltaMoveY > 0) {    // mouvement haut
            for (int i = y1 + 1; i < y2; i++) {
                if (boardJetons[x2, i] != null || boardPiece[x2, i] != null) {
                    Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveX == 0 && deltaMoveY < 0) {    // mouvement bas
            for (int i = y1 - 1; i > y2; i--) {
                if (boardJetons[x2, i] != null || boardPiece[x2, i] != null) {
                    Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }

        return false;
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
