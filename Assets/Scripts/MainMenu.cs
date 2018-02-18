using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour {

    static int skybox_ = 1;
    public Material skyboxMontagne;
    public Material skyboxPlage;
    public Material skyboxEspace;

    public Camera mainCamera;

    public GUITexture overlay;
    public float fadeTime;

    private bool MenuPrincipal_ = false;
    private bool Configuration_ = false;
    private bool Skybox_ = false;
    private bool Demarrage_ = false;
    private bool Audio_ = false;
    private bool Graphique_ = false;

    private Vector3 positionNiveau2_ = new Vector3(0f, 850f, 0f);
    private Vector3 positionNiveau1_ = new Vector3(0f, 350f, 0f);
    private Vector3 angle4_ = new Vector3(25f, 270f, 0f);
    private Vector3 angle3_ = new Vector3(25f, 180f, 0f);
    private Vector3 angle2_ = new Vector3(25f, 90f, 0f);
    private Vector3 angle1_ = new Vector3(25f, 0f, 0f);

    void Awake() {
        overlay.pixelInset = new Rect(0, 0, Screen.width, Screen.height);

        StartCoroutine(FadeToClear());
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * 5);
        }

        if (MenuPrincipal_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle1_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau1_, Time.deltaTime);
        }
        if (Configuration_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle2_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau1_, Time.deltaTime);
        }
        if (Skybox_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle2_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau2_, Time.deltaTime);
        }
        if (Demarrage_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle1_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau2_, Time.deltaTime);
        }
        if (Audio_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle3_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau1_, Time.deltaTime);
        }
        if (Graphique_) {
            mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, angle4_, Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionNiveau1_, Time.deltaTime);
        }
    }

    public void StartHotSeat()
    {
        StartCoroutine(FadeToBlack( () => SceneManager.LoadScene("Hotseat")));
    }

    public void StartIA()
    {
        StartCoroutine(FadeToBlack(() => SceneManager.LoadScene("IA")));
    }

    public void StartReseau()
    {
        StartCoroutine(FadeToBlack(() => SceneManager.LoadScene("Reseau")));
    }

    public void Quit() {
        Application.Quit();
    }

    public void Configuration() {
        MenuPrincipal_ = false;
        Configuration_ = true;
        Skybox_ = false;
        Demarrage_ = false;
        Audio_ = false;
        Graphique_ = false;
    }

    public void MenuPrincipal() {
        MenuPrincipal_ = true;
        Configuration_ = false;
        Skybox_ = false;
        Demarrage_ = false;
        Audio_ = false;
        Graphique_ = false;
    }

    public void Skybox() {
        MenuPrincipal_ = false;
        Configuration_ = false;
        Skybox_ = true;
        Demarrage_ = false;
        Audio_ = false;
        Graphique_ = false;
    }

    public void Demarrage() {
        MenuPrincipal_ = false;
        Configuration_ = false;
        Skybox_ = false;
        Demarrage_ = true;
        Audio_ = false;
        Graphique_ = false;
    }

    public void Audio() {
        MenuPrincipal_ = false;
        Configuration_ = false;
        Skybox_ = false;
        Demarrage_ = false;
        Audio_ = true;
        Graphique_ = false;
    }

    public void Graphique() {
        MenuPrincipal_ = false;
        Configuration_ = false;
        Skybox_ = false;
        Demarrage_ = false;
        Audio_ = false;
        Graphique_ = true;
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

    public void setSkyboxMontagne() {
        skybox_ = 2;
        RenderSettings.skybox = skyboxMontagne;
    }

    public void setSkyboxPlage() {
        skybox_ = 3;
        RenderSettings.skybox = skyboxPlage;
    }

    public void setSkyboxEspace() {
        skybox_ = 1;
        RenderSettings.skybox = skyboxEspace;
    }

    public int getSkybox() {
        return skybox_;
    }
}
