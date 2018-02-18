using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class CheckboardReseau : Checkboard
{

	// Use this for initialization
	override protected void Start () {
        GenerateBoard();
        canevaVictoire.enabled = false;
        canevasMenuPause.enabled = false;
        etat = 0;
    }
	
	// Update is called once per frame
	override protected void Update () {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKey("m"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown("n"))
        {
            PauseGame();
        }

        updateMouseOver();

        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if (selectedPiece != null && (etat == 0))
        {
            updatePieceDrag(selectedPiece);
        }
        if (selectedJeton != null && (etat == 1))
        {
            updateJetonDrag(selectedJeton);
        }
        if (Input.GetMouseButtonDown(0) && !pause && (etat == 0))
        {
            selectPiece(x, y);
        }
        if (Input.GetMouseButtonDown(0) && !pause && (etat == 1))
        {
            TryMoveJeton((int)startDrag.x, (int)startDrag.y, x, y);
        }
        if (Input.GetMouseButtonUp(0) && !pause && (etat == 0))
        {
            TryMovePiece((int)startDrag.x, (int)startDrag.y, x, y);
        }
        if (isWhiteTurn_ && !victoire)
        {
            Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, angleWhiteTurn_, Time.deltaTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, positionWhiteTurn_, Time.deltaTime);
        }

        if (enMouvement)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerptime)
            {

                if (selectedPiece.getIsWhite())
                {
                    selectedPiece.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f); ;

                }
                else
                {
                    selectedPiece.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f); ;

                }
                selectedPiece.transform.position = endPos;
                selectedPiece = null;
                enMouvement = false;
                currentLerpTime = 0;
                selectedJeton.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                float perc = currentLerpTime / lerptime;
                selectedPiece.transform.position = Vector3.Lerp(startPos, endPos, perc);
            }
        }
    }
}
