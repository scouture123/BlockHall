using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Checkboard : MonoBehaviour {

    public static int skybox = 0;

    public Material skyboxMontagne;
    public Material skyboxPlage;
    public Material skyboxEspace;
    public GameObject gameController;
    public MainMenu scriptMainMenu;
    public GUITexture overlay;
    public float fadeTime;
    public Canvas canevaVictoire;
    public Canvas canevasMenuPause;
    public Text texteVictoire;


    public Piece[,] pieces = new Piece[8, 8];
    public Jeton[,] jetons = new Jeton[8, 8];
    public Case[,] cases = new Case[8, 8];
    public GameObject lich_King_BlackPrefab;
    public GameObject lich_King_WhitePrefab;
    public GameObject whiteJetonPrefab;

    private List<Piece> whitePieceList = new List<Piece>();
    private List<Piece> blackPieceList = new List<Piece>();
    private Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private int etat;
    private bool victoire = false;

    private bool isWhiteTurn_ = true;
    private bool isBlackTurn_ = false;
    private Vector3 positionWhiteTurn_ = new Vector3(0f, 8f, -8f);
    private Vector3 positionBlackTurn_ = new Vector3(0f, 8f, 8f);
    private Vector3 angleWhiteTurn_ = new Vector3(45f, 0f, 0f);
    private Vector3 angleBlackTurn_ = new Vector3(45f, 180f, 0f);

    private Piece selectedPiece;
    private Jeton selectedJeton;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 stopDrag;

    // mouvement piece
    private Vector3 startPos;
    private Vector3 endPos;
    private float lerptime = 1.4f;
    private float currentLerpTime = 0;
    private bool enMouvement = false;


    // Use this for initialization
    void Start () {
        //Camera.main.transform.position = (Vector3.up * 8) + (Vector3.back * 8);
        //Camera.main.transform.rotation = Quaternion.Euler(45.0f, 0.0f, 0.0f); ;
        GenerateBoard();
        canevaVictoire.enabled = false;
        canevasMenuPause.enabled = false;
        etat = 0;
        
        //isWhiteTurn = true;
    }
	
	// Update is called once per frame
	private void Update () {

        if (Input.GetKey("escape")) {
            Application.Quit();
        }
        if (Input.GetKey("m")) {
            SceneManager.LoadScene("MainMenu");
        }

        updateMouseOver();
        
        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if(selectedPiece != null && (etat == 0 || etat == 2) ) {
            updatePieceDrag(selectedPiece);
        }
        if (selectedJeton != null && (etat == 1 || etat == 3)) {
            updateJetonDrag(selectedJeton);
        }
        if (Input.GetMouseButtonDown(0) && (etat == 0 || etat == 2)) {
            selectPiece(x, y);
        }
        if (Input.GetMouseButtonDown(0) && (etat == 1 || etat == 3)) {
            TryMoveJeton((int)startDrag.x, (int)startDrag.y, x, y);
        }
        if (Input.GetMouseButtonUp(0) && (etat == 0 || etat == 2)) {
            TryMovePiece((int)startDrag.x, (int)startDrag.y, x, y);
        }
        if (isWhiteTurn_ && !victoire) {
            Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, angleWhiteTurn_, Time.deltaTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, positionWhiteTurn_, Time.deltaTime);
        }
        if (isBlackTurn_ && !victoire) {
            Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, angleBlackTurn_, Time.deltaTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, positionBlackTurn_, Time.deltaTime);
        }

        if (enMouvement == true) {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerptime) {
                
                if (selectedPiece.getIsWhite()) {
                    selectedPiece.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f); ;

                }
                else {
                    selectedPiece.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f); ;

                }
                selectedPiece.transform.position = endPos;
                selectedPiece = null;
                enMouvement = false;
                currentLerpTime = 0;
                selectedJeton.GetComponent<MeshRenderer>().enabled = true;
            }
            else {
                float perc = currentLerpTime / lerptime;
                selectedPiece.transform.position = Vector3.Lerp(startPos, endPos, perc);
            }
        }
    }

    private void updatePieceDrag(Piece p) {
        if (!Camera.main) {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board"))) {
            p.transform.position = hit.point + Vector3.up;
        }
    }

    private void updateJetonDrag(Jeton j) {
        if (!Camera.main) {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board"))) {
            j.transform.position = hit.point + Vector3.up;
        }
    }

    private void updateMouseOver() {

        if (!Camera.main) {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board"))) {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }

    }

    private void selectPiece(int x, int y) {
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
            return;
        Piece p = pieces[x, y];
        if (p != null) {
            if (etat == 0 && p.name == "lich_King_White(Clone)") {
                selectedPiece = p;
                startDrag = mouseOver;
            }
            else if (etat == 2 && p.name == "lich_King_Black(Clone)") {
                selectedPiece = p;
                startDrag = mouseOver;
            }
        }
    }

    private void selectJeton(int x, int y) {
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
            return;

        Jeton j = jetons[x, y];
        if (j != null) {
            selectedJeton = j;
            startDrag = mouseOver;
        }
    }

    private void TryMoveJeton(int x1, int y1, int x2, int y2) {

        startDrag = new Vector2(x1, y1);
        stopDrag = new Vector2(x2, y2);
        
        if (x2 < 0 || x2 >= jetons.Length || y2 < 0 || y2 >= jetons.Length) {
            if (selectedJeton != null) {
                MoveJeton(selectedJeton, x1, y1);
            }
            startDrag = Vector2.zero;
            selectedJeton = null;
            return;
        }
        
        if (selectedJeton != null) {
            // le jeton n'a pas bouger

            if (stopDrag == startDrag) {
                return;
            }
            if (selectedJeton.ValidMove(pieces, jetons, x1, y1, x2, y2)) {
                jetons[x2, y2] = selectedJeton;
                jetons[x1, y1] = null;
                MoveJeton(selectedJeton, x2, y2);
                EndTurn();
            }
        }
    }
    
    private void TryMovePiece(int x1, int y1, int x2, int y2) {

        startDrag = new Vector2(x1, y1);
        stopDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

        if (x2 < 0 || x2 >= pieces.Length || y2 < 0 || y2 >= pieces.Length) {
            if(selectedPiece != null) {
                MovePiece(selectedPiece, x1, y1, x2, y2, false);
            }
            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }

        if (selectedPiece != null) {
            // la piece n'a pas bouger
            if (stopDrag == startDrag) {
                MovePiece(selectedPiece, x1, y1, x2, y2, true);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }
            if (selectedPiece.ValidMove(pieces, jetons, x1, y1, x2, y2)) {
                pieces[x2, y2] = selectedPiece;
                selectedPiece.setPositionX(x2);
                selectedPiece.setPositionY(y2);
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x1, y1, x2, y2, false);


                GenerateJeton(x2, y2);
                Jeton j = jetons[x2, y2];
                selectedJeton = j;
                j.GetComponent<MeshRenderer>().enabled = false;
                startDrag = mouseOver;
                EndTurn();
            }
            else {
                MovePiece(selectedPiece, x1, y1, x2, y2, true);
                startDrag = Vector2.zero;
                selectedPiece = null;
            }
        }

    }

    private void EndTurn() {
        
        if(etat == 0 || etat == 2) {
            etat++;
            
        }
        else if(etat == 1) {
            etat++;
            selectedJeton = null;
            StartCoroutine(ChangeCameraTurn());
            
        }
        else {
            etat = 0;
            selectedJeton = null;
            StartCoroutine(ChangeCameraTurn());
        }
        miseAJourPlateau();
        checkVictory();
    }

    private void checkVictory() {
        int nombreDePieceBloque = 0;
        for (int i = 0; i < whitePieceList.Count; i++) {
            Piece pieceAVerifier = whitePieceList[i];
            if (scanAutour(pieceAVerifier)) {
                nombreDePieceBloque++;
            }
            if(nombreDePieceBloque >= 4) {
                victoire = true;
                StartCoroutine(Victoire());
                texteVictoire.text = "le joueur noir a gagné!!!";
                canevaVictoire.enabled = true;
                //Debug.Log("le joueur noir a gagné!!!");
            }
        }
        nombreDePieceBloque = 0;
        for (int i = 0; i < blackPieceList.Count; i++) {
            Piece pieceAVerifier = blackPieceList[i];
            if (scanAutour(pieceAVerifier)) {
                nombreDePieceBloque++;
            }
            if (nombreDePieceBloque >= 4) {
                victoire = true;
                StartCoroutine(Victoire());
                texteVictoire.text = "le joueur blanc a gagné!!!";
                canevaVictoire.enabled = true;
                //Debug.Log("le joueur blanc a gagné!!!");
            }
        }
    }

    private bool scanAutour(Piece pieceAVerifier) {
        List<Case> voisins = cases[pieceAVerifier.getPositionX(), pieceAVerifier.getPositionY()].getVoisins();
        for (int i = 0; i < voisins.Count; i++) {
            if (voisins[i].getOccuper() == false) {
                return false;
            }
        }
        return true;
    }

    private void GenerateBoard() {
        // generate case
        for (int i = 0; i < cases.GetLength(0); i++) {
            for (int j = 0; j < cases.GetLength(1); j++) {
                cases[i, j] = new Case();
                cases[i, j].setPositionX(i);
                cases[i, j].setPositionY(j);
                cases[i, j].setOccuper(false);
            }
        }

        genererVoisinsCases();

        //generer skybox
        genererSkybox();

        //generate white team
        GeneratePiece(0, 2);
        GeneratePiece(2, 0);
        GeneratePiece(5, 0);
        GeneratePiece(7, 2);

        //generate black team
        GeneratePiece(0, 5);
        GeneratePiece(2, 7);
        GeneratePiece(5, 7);
        GeneratePiece(7, 5);

        miseAJourPlateau();
    }

    private void GeneratePiece(int x, int y) {
        bool isPieceWhite = (y > 3) ? false : true;
        GameObject gameObject = Instantiate((isPieceWhite) ? lich_King_WhitePrefab : lich_King_BlackPrefab) as GameObject;
        gameObject.transform.SetParent(transform);
        Piece p = gameObject.GetComponent<Piece>();
        if (isPieceWhite) {
            whitePieceList.Add(p);
            p.setIsWhite(true);
        }
        else {
            blackPieceList.Add(p);
            p.setIsWhite(false);
        }
        p.setPositionX(x);
        p.setPositionY(y);
        pieces[x, y] = p;
        firstMovePiece(p, x, y);
    }

    private void GenerateJeton(int x, int y) {
        GameObject gameObject = Instantiate(whiteJetonPrefab) as GameObject;
        gameObject.transform.SetParent(transform);
        Jeton j = gameObject.GetComponent<Jeton>();
        jetons[x, y] = j;
        MoveJeton(j, x, y);
    }

    private void MovePiece(Piece p, int x1, int y1, int x2, int y2, bool surPlace) {
        if (!surPlace) {
            selectedPiece = p;
            p.GetComponent<Animation>().Play();
            startPos = (Vector3.right * x1) + (Vector3.forward * y1) + boardOffset + pieceOffset;
            endPos = (Vector3.right * x2) + (Vector3.forward * y2) + boardOffset + pieceOffset;
            ajustementAngle(p, x1, y1, x2, y2);
            enMouvement = true;
            //si la piece n'a pas besoin d'animation
            //p.transform.position = (Vector3.right * x2) + (Vector3.forward * y2) + boardOffset + pieceOffset;            
        }
        else {
            p.transform.position = (Vector3.right * x1) + (Vector3.forward * y1) + boardOffset + pieceOffset;
        }
    }

    private void MoveJeton(Jeton j, int x, int y) {
        j.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

    private void firstMovePiece(Piece p, int x, int y) {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        bool isPieceWhite = (y > 3) ? false : true;
        if (isPieceWhite) {
            p.transform.Rotate(0, 90, 0);
        }
        else {
            p.transform.Rotate(0, 270, 0);
        }
    }

    private void miseAJourPlateau() {
        for (int i = 0; i < cases.GetLength(0); i++) {
            for (int j = 0; j < cases.GetLength(1); j++) {
                if (pieces[i, j] != null || jetons[i, j] != null) {
                    cases[i, j].setOccuper(true);
                }
                else {
                    cases[i, j].setOccuper(false);
                }
            }
        }
    }

    private void genererVoisinsCases() {
        for (int i = 0; i < cases.GetLength(0) - 1; i++) {
            for (int j = 0; j < cases.GetLength(1) - 1; j++) {
                cases[i, j].ajouterVoisin(cases[i, j + 1]);
                if (i == 0 || i == cases.GetLength(0) - 1) {
                    cases[i, j + 1].ajouterVoisin(cases[i, j]);
                }
            }
        }
        for (int i = 1; i < cases.GetLength(0); i++) {
            for (int j = 1; j < cases.GetLength(1); j++) {
                cases[i, j].ajouterVoisin(cases[i, j - 1]);
                if(i == 0 || i == cases.GetLength(0) - 1) {
                    cases[i, j - 1].ajouterVoisin(cases[i, j]);
                }
            }
        }
        for (int i = 0; i < cases.GetLength(0) - 1; i++) {
            for (int j = 0; j < cases.GetLength(1) - 1; j++) {
                cases[i, j].ajouterVoisin(cases[i + 1, j]);
                if (j == 0 || j == cases.GetLength(1) - 1) {
                    cases[i + 1, j].ajouterVoisin(cases[i, j]);
                }
            }
        }
        for (int i = 1; i < cases.GetLength(0); i++) {
            for (int j = 1; j < cases.GetLength(1); j++) {
                cases[i, j].ajouterVoisin(cases[i - 1, j]);
                if (j == 0 || j == cases.GetLength(1) - 1) {
                    cases[i - 1, j].ajouterVoisin(cases[i, j]);
                }
            }
        }
        for (int i = 0; i < cases.GetLength(0) - 1; i++) {
            for (int j = 0; j < cases.GetLength(1) - 1; j++) {
                cases[i, j].ajouterVoisin(cases[i + 1, j + 1]);
            }
        }
        for (int i = 1; i < cases.GetLength(0); i++) {
            for (int j = 1; j < cases.GetLength(1); j++) {
                cases[i, j].ajouterVoisin(cases[i - 1, j - 1]);
            }
        }
        for (int i = cases.GetLength(0) - 1; i > 0; i--) {
            for (int j = 0; j < cases.GetLength(1) - 1; j++) {
                cases[i, j].ajouterVoisin(cases[i - 1, j + 1]);
            }
        }
        for (int i = cases.GetLength(0) - 2; i > -1; i--) {
            for (int j = 1; j < cases.GetLength(1); j++) {
                cases[i, j].ajouterVoisin(cases[i + 1, j - 1]);
            }
        }
    }

    private void ajustementAngle(Piece p, int x1, int y1, int x2, int y2) {
        int deltaX = x2 - x1;
        int deltaY = y2 - y1;
        if (deltaX > 0 && deltaY > 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f); ;
        }
        if (deltaX > 0 && deltaY == 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f); ;
        }
        if (deltaX > 0 && deltaY < 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 225.0f, 0.0f); ;
        }
        if (deltaX == 0 && deltaY < 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f); ;
        }
        if (deltaX < 0 && deltaY < 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 315.0f, 0.0f); ;
        }
        if (deltaX < 0 && deltaY == 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); ;
        }
        if (deltaX < 0 && deltaY > 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f); ;
        }
        if (deltaX == 0 && deltaY > 0) {
            p.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f); ;
        }
    }

    private void genererSkybox() {
        scriptMainMenu = (MainMenu)gameController.GetComponent(typeof(MainMenu));
        if (scriptMainMenu.getSkybox() == 1) {
            scriptMainMenu.setSkyboxEspace();
        }
        else if (scriptMainMenu.getSkybox() == 2) {
            scriptMainMenu.setSkyboxMontagne();
        }
        else if (scriptMainMenu.getSkybox() == 3) {
            scriptMainMenu.setSkyboxPlage();
        }
    }

    private IEnumerator FadeToClear() {

        overlay.gameObject.SetActive(true);
        overlay.color = Color.black;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;

        while (progress < 1.0f) {
            overlay.color = Color.Lerp(Color.black, Color.clear, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        overlay.color = Color.clear;
        overlay.gameObject.SetActive(false);

    }

    private IEnumerator FadeToBlack(Action levelLoad) {

        overlay.gameObject.SetActive(true);
        overlay.color = Color.clear;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;

        while (progress < 1.0f) {
            overlay.color = Color.Lerp(Color.clear, Color.black, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        overlay.color = Color.black;

        levelLoad();

    }

    private IEnumerator ChangeCameraTurn() {
        yield return new WaitForSeconds(0.5f);
        isWhiteTurn_ = !isWhiteTurn_;
        isBlackTurn_ = !isBlackTurn_;
    }

    private IEnumerator Victoire() {
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeToBlack(() => SceneManager.LoadScene("MainMenu")));
    }
}
