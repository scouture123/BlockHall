using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class IA : MonoBehaviour
{
    static public Piece tempPiece;

    static public Mouvement GenererMouvement()
    {
        return new Mouvement();
    }

    static public Mouvement GenererMouvement(Piece[,] Pieces, Jeton[,] Jetons)
    {
        return FindAllPossibleMoves(Pieces, Jetons);
    }

    static public Mouvement FindAllPossibleMoves(Piece[,] Pieces, Jeton[,] Jetons)
    {
        //Piece selectedPiece = new Piece();
        List<Mouvement> listeMouvements = new List<Mouvement>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[i, j] != null && !Pieces[i, j].getIsWhite())
                {
                    FindAllValidMoves(Pieces, Jetons, i, j, listeMouvements);
                }
            }
        }

        System.Random rnd = new System.Random();
        int r = rnd.Next(listeMouvements.Count);

        return listeMouvements[r];
    }

    static void FindAllValidMoves(Piece[,] Pieces, Jeton[,] Jetons, int x1, int y1, List<Mouvement> listeMouvements)
    {
        Mouvement mouvement = new Mouvement();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ValidMove(Pieces, Jetons, x1, y1, i, j))
                {
                    mouvement.Departx = x1;
                    mouvement.Departy = y1;
                    mouvement.Finx = i;
                    mouvement.Finy = j;
                    TempBoardErase(Pieces, mouvement.Departx, mouvement.Departy);
                    for (int s = 0; s < 8; s++)
                    {
                        for (int t = 0; t < 8; t++)
                        {
                            if (!(s == mouvement.Finx && t == mouvement.Finy))
                            {
                                if (ValidMove(Pieces, Jetons, mouvement.Finx, mouvement.Finy, s, t))
                                {
                                    mouvement.jetonx = s;
                                    mouvement.jetony = t;
                                    //Debug.Log("mouvement a ajouter:");
                                    //mouvement.afficher();
                                    listeMouvements.Add(new Mouvement(mouvement.Departx, mouvement.Departy, mouvement.Finx, mouvement.Finy, mouvement.jetonx, mouvement.jetony));
                                    
                                        //Debug.Log(listeMouvements.Count);
                                       
                                    
                                }
                            }
                        }
                    }
                    TempBoardAdd(Pieces, mouvement.Departx, mouvement.Departy);
                }
            }
        }
    }

    static public void TempBoardErase(Piece[,] pieces, int x, int y)
    {
        if(pieces[x,y] != null)
        {
            tempPiece = pieces[x, y];
            pieces[x, y] = null;
        }
    }

    static public void TempBoardAdd(Piece[,] pieces, int x, int y)
    {
        if (pieces[x, y] == null)
        {
            pieces[x, y] = tempPiece;
        }
    }

    static public bool ValidMove(Piece[,] boardPiece, Jeton[,] boardJetons, int x1, int y1, int x2, int y2)
    {
        if (boardPiece[x2, y2] != null || boardJetons[x2, y2] != null)
        {
            return false; // est sur une autre piece
        }

        int deltaMoveX = x2 - x1;
        int deltaMoveY = y2 - y1;

        if (Mathf.Abs(deltaMoveY) == Mathf.Abs(deltaMoveX))
        {
            if (deltaMoveX > 0 && deltaMoveY > 0)
            { // mouvement diagonale haut droite
                for (int i = x1 + 1, j = y1 + 1; i < x2 || j < y2; i++, j++)
                {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null)
                    {
                        //Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX < 0 && deltaMoveY > 0)
            { // mouvement diagonale haut gauche
                for (int i = x1 - 1, j = y1 + 1; i > x2 || j < y2; i--, j++)
                {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null)
                    {
                        //Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX > 0 && deltaMoveY < 0)
            { // mouvement diagonale bas droite
                for (int i = x1 + 1, j = y1 - 1; i < x2 || j > y2; i++, j--)
                {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null)
                    {
                        //Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            if (deltaMoveX < 0 && deltaMoveY < 0)
            { // mouvement diagonale bas gauche
                for (int i = x1 - 1, j = y1 - 1; i > x2 || j > y2; i--, j--)
                {
                    if (boardJetons[i, j] != null || boardPiece[i, j] != null)
                    {
                        //Debug.Log("board[" + i + "," + j + "] ");
                        return false;
                    }
                }
            }
            return true;
        }
        if (deltaMoveY == 0 && deltaMoveX > 0)
        {    // mouvement droite
            for (int i = x1 + 1; i < x2; i++)
            {
                if (boardJetons[i, y2] != null || boardPiece[i, y2] != null)
                {
                    //Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveY == 0 && deltaMoveX < 0)
        {    // mouvement gauche
            for (int i = x1 - 1; i > x2; i--)
            {
                if (boardJetons[i, y2] != null || boardPiece[i, y2] != null)
                {
                    //Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveX == 0 && deltaMoveY > 0)
        {    // mouvement haut
            for (int i = y1 + 1; i < y2; i++)
            {
                if (boardJetons[x2, i] != null || boardPiece[x2, i] != null)
                {
                    //Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }
        if (deltaMoveX == 0 && deltaMoveY < 0)
        {    // mouvement bas
            for (int i = y1 - 1; i > y2; i--)
            {
                if (boardJetons[x2, i] != null || boardPiece[x2, i] != null)
                {
                    //Debug.Log("board[" + i + "," + y2 + "] ");
                    return false;
                }
            }
            return true;
        }

        return false;
    }
}