using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    ScrollScriptLobby scroll = new ScrollScriptLobby();
    public GameObject basicui;
    public GameObject settingobject;
    public GameObject rankingobject;
    public GameObject cardlibraryobject;
    public GameObject soundon;
    public GameObject soundon2;
    [SerializeField] private Camera mainCamera;
    private const int RotCamDirLeft = -1, RotCamDirRight = 1;
    private const int RotAngleMaxLeft = -60, RotAngleMaxRight = 60;
    private float _mainCamRotSpd = 6.0f, _mainCamRotAngle = 0;
    private int _rotCamCurrDir = RotCamDirRight;

    // Start is called before the first frame update



    private void Update()
    {
        RotateMainCamera();
    }


    // Update is called once per frame

    public void GoGameScene()
    {
        SceneManager.LoadScene("LogicTest");
    }
    public void GameOver()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void CardLibraryon()
    {
        basicui.SetActive(false);
        cardlibraryobject.SetActive(true);
    }
    public void CardLibraryoff()
    {
        basicui.SetActive(true);
        cardlibraryobject.SetActive(false);
    }
    public void RankingOn()
    {
        basicui.SetActive(false);
        rankingobject.SetActive(true);
    }
    public void RankingOff()
    {
        basicui.SetActive(true);
        rankingobject.SetActive(false);
    }

    public void SettingOn()
    {

        basicui.SetActive(false);
        settingobject.SetActive(true);
        soundon.SetActive(false);
        soundon2.SetActive(false);
    }
    public void SettingOff()
    {

        settingobject.SetActive(false);
        basicui.SetActive(true);
    }
    private void RotateMainCamera()
    {
        if (_mainCamRotAngle > RotAngleMaxRight)
            _rotCamCurrDir = RotCamDirLeft;
        if (_mainCamRotAngle < RotAngleMaxLeft)
            _rotCamCurrDir = RotCamDirRight;

        float rotValue = _mainCamRotSpd * _rotCamCurrDir * Time.deltaTime;
        _mainCamRotAngle += rotValue;
        mainCamera.transform.Rotate(Vector3.up * rotValue);
    }
}
