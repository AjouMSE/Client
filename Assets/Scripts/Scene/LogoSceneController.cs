using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// LogoSceneController.cs
/// Author: Lee Hong Jun (Arcane22, hong3883@naver.com)
/// Version: 1.0
/// Last Modified: 2022. 04. 04
/// Description: LogoScene's fade effect controller
/// </summary>
public class LogoSceneController : MonoBehaviour
{
    #region Private constants, instance variables

    private enum FadeTypes
    {
        FadeIn = 0,
        FadeOut = 1,
    }

    private const float MinAlphaValue = 0f, MaxAlphaValue = 1f;
    private const float MaxFadeTime = 2f;
    private const int MaxFrameRate = 60;
    
    private CanvasGroup _canvasGroup;
    
    #endregion
    
    #region Init, Unity event methods
    void Init()
    {
        // Get CanvasGroup component
        _canvasGroup = GetComponent<CanvasGroup>();
        if(_canvasGroup == null)
            Debug.LogError("Cannot find component: " + typeof(CanvasGroup));
        
        // Set canvas group's alpha value to 0
        _canvasGroup.alpha = MinAlphaValue;
        
        // Set Screen orientation : landscape
        Screen.orientation = ScreenOrientation.Landscape;
        
        // Set maximum frame rate : 60
        Application.targetFrameRate = MaxFrameRate;
        
        // Start fade effect
        StartCoroutine(FadeEffect((int) FadeTypes.FadeIn));
    }
    
    void Awake()
    {
        Init();
    }

    #endregion
    
    
    
    #region Fade Effect coroutine
    IEnumerator FadeEffect(int fadeType)
    {
        float gap =  Time.deltaTime / MaxFadeTime;
        
        switch (fadeType)
        {
            case (int) FadeTypes.FadeIn:
                while (_canvasGroup.alpha < MaxAlphaValue)
                {
                    _canvasGroup.alpha += gap;
                    Debug.Log(_canvasGroup.alpha);
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                StartCoroutine(FadeEffect((int) FadeTypes.FadeOut));
                break;
            
            case (int) FadeTypes.FadeOut:
                while (_canvasGroup.alpha > MinAlphaValue)
                {
                    _canvasGroup.alpha -= gap;
                    Debug.Log(_canvasGroup.alpha);
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                SceneManager.LoadScene("LoginScene");
                break;
            
            default:
                Debug.LogError("UndefinedFadeTypeError");
                break;
        }
    }
    #endregion
}
