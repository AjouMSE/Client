/*
Thank you for purchasing this asset! If you have any questions, or need any help, 
please don't hesitate to contact me:  mr.patrick.ball@gmail.com
Happy Creating!

the challenge of this script was to make the 3D scroll object and its UI canvas sibling work in tandem to create the illusion of a scrolling scroll.

*/

using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using UI.Game;
using UnityEngine.UI;
using UnityEngine;

public class ScrollScript3DTest : MonoBehaviour
{
    #region Private variables

    [Header("Menu Canvas")] 
    [SerializeField] private Canvas menuCanvas;
    
    [Header("HUD GameCard Selection UI Controller")] 
    [SerializeField] private HUDGameCardSelectionUIController controller;

    [Header("Backdrops, SectionButtons")]
    [SerializeField] private GameObject backdrops;
    [SerializeField] private GameObject cardTemplate;
    [SerializeField] private GameObject[] sectionButtons;

    [Header("Options")] 
    [SerializeField] private int backdropLength = 6;
    [SerializeField] private int menuPositionInt = 0;
    [SerializeField] private float menuSlideSpeed = 0.1f;
    [SerializeField] private float menuFadeInSpeed = 0.2f;
    [SerializeField] private bool playSound = true;

    private Animator _animator;
    private AudioSource _audioSource;
    private Vector3 _currentSliderPos;
    private bool _isScrollOpen = false;

    // About backdrops
    private RectTransform _backdropTrans;
    private float _menuPosY;
    private float _menuPosZ;

    // About section buttons
    private RectTransform[] _buttonRectTrans;
    private Image[] _buttonImages;
    private Color _baseColor, _selectedColor;

    private int _sectionOpen = 0b1000;

    enum SectionType
    {
        All = 8,
        Move = 4,
        Attack = 2,
        Special = 1,
    }
    
    #endregion


    #region Public variables

    public List<CardInScroll> cards;
    public Dictionary<int, CardInScroll> cardDict;
    public Dictionary<int, Image> cardImageDict;
    public Dictionary<int, Button> buttonDict;

    #endregion


    #region Unity event methods

    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        _currentSliderPos = new Vector3(8.75f + (-3.5f * menuPositionInt), _menuPosY, _menuPosZ);
        _backdropTrans.localPosition =
            Vector3.MoveTowards(_backdropTrans.localPosition, _currentSliderPos, menuSlideSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenScroll();
        }
    }

    #endregion


    #region Callbacks

    public void OnMenuBtnClick(int sectionOpen)
    {
        _sectionOpen = sectionOpen;
        StartCoroutine(SortScroll());
    }

    public void OnMenuMoveLeftBtnClick()
    {
        if (menuPositionInt > 0)
        {
            menuPositionInt = (menuPositionInt - 1);
            _animator.SetTrigger("RollRightTrig");
            PlaySound();
        }
    }

    public void OnMenuMoveRightBtnClick()
    {
        if (menuPositionInt < (backdropLength - 1))
        {
            menuPositionInt = (menuPositionInt + 1);
            _animator.SetTrigger("RollLeftTrig");
            PlaySound();
        }
    }

    public void MoveMenuLeft()
    {
        if (menuPositionInt < (backdropLength - 1))
        {
            menuPositionInt = (menuPositionInt + 1);
            _animator.SetTrigger("RollLeftTrig");
            PlaySound();
        }
    }

    public void MoveMenuRight()
    {
        if (menuPositionInt > 0)
        {
            menuPositionInt = (menuPositionInt - 1);
            _animator.SetTrigger("RollRightTrig");
            PlaySound();
        }
    }

    #endregion


    #region Private methods

    private void Init()
    {
        // Init backdrops, section buttons
        _baseColor = new Color(1, 0.87f, 0.75f);
        _selectedColor = new Color(1, 0.75f, 0.36f);
        _buttonRectTrans = new RectTransform[4];
        _buttonImages = new Image[4];

        _backdropTrans = backdrops.GetComponent<RectTransform>();
        for (int i = 0; i < sectionButtons.Length; i++)
        {
            _buttonRectTrans[i] = sectionButtons[i].GetComponent<RectTransform>();
            _buttonImages[i] = sectionButtons[i].GetComponent<Image>();
        }

        // Init components
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _menuPosY = _backdropTrans.localPosition.y;
        _menuPosZ = _backdropTrans.localPosition.z;
        menuCanvas.worldCamera = GameObject.FindGameObjectWithTag("HUDCamera").GetComponent<Camera>();

        cards = new List<CardInScroll>();
        cardDict = new Dictionary<int, CardInScroll>();
        cardImageDict = new Dictionary<int, Image>();
        buttonDict = new Dictionary<int, Button>();
        
        foreach (int key in TableDatas.Instance.cardDatas.Keys)
        {
            CardData cardData = TableDatas.Instance.cardDatas[key];
            GameObject cardObj = Instantiate(cardTemplate, backdrops.transform);
            
            // set name & image
            cardObj.name = $"{cardObj.name}{cardData.text}";
            Image cardImage = cardObj.GetComponent<Image>();
            cardImage.sprite = CacheSpriteSource.Instance.GetSource(key);
            cardImageDict.Add(key, cardImage);
            buttonDict.Add(key, cardObj.GetComponent<Button>());
            
            // set data & on click event listener
            CardInScroll cardInScroll = cardObj.GetComponent<CardInScroll>();
            cardInScroll.cardData = cardData;
            cardObj.GetComponent<Button>().onClick.AddListener(delegate { controller.OnCardAddBtnClick(key); });
            
            cardDict.Add(key, cardInScroll);
        }

        Sort();
    }

    void EnableMenu()
    {
        menuCanvas.gameObject.SetActive(true);

        //for fading in - because fading in is fancy.
        CanvasRenderer canRen = backdrops.GetComponent<CanvasRenderer>();
        canRen.SetAlpha(0.0f);
        backdrops.GetComponent<Image>().CrossFadeAlpha(1.0f, menuFadeInSpeed, false);
    }

    // Called by the CloseScroll() function above
    void DisableMenu()
    {
        menuCanvas.gameObject.SetActive(false);
    }


    private void PlaySound()
    {
        if (playSound)
        {
            _audioSource.Play();
        }
    }

    private void ChangeActivate()
    {
        switch (_sectionOpen)
        {
            case (int) SectionType.All:
                backdropLength = 6;
                foreach (CardInScroll c in cardDict.Values)
                {
                    c.gameObject.SetActive(true);
                }
                break;
            
            case (int) SectionType.Move:
                backdropLength = 2;
                foreach (CardInScroll c in cardDict.Values)
                {
                    var obj = c.gameObject;
                    obj.SetActive(c.cardData.type == (int)Card.SkillType.Move);
                }
                break;
            
            case (int) SectionType.Attack:
                backdropLength = 3;
                foreach (CardInScroll c in cardDict.Values)
                {
                    var obj = c.gameObject;
                    obj.SetActive(c.cardData.type == (int)Card.SkillType.Attack);
                }
                break;
            
            case (int) SectionType.Special:
                backdropLength = 2;
                foreach (CardInScroll c in cardDict.Values)
                {
                    var obj = c.gameObject;
                    obj.SetActive(c.cardData.type == (int)Card.SkillType.Special);
                }
                break;
        }
    }

    private void Sort()
    {
        int mask = 0b1000;
        for (int i = 0; i < 4; i++)
        {
            Vector3 tmpPos;
            Color tmpColor;
            if ((_sectionOpen & mask) > 0)
            {
                tmpPos = new Vector3(_buttonRectTrans[i].localPosition.x, -0.42f, 0.03f);
                tmpColor = _selectedColor;
            }
            else
            {
                tmpPos = new Vector3(_buttonRectTrans[i].localPosition.x, -0.4f, 0.04f);
                tmpColor = _baseColor;
            }

            _buttonRectTrans[i].localPosition = tmpPos;
            _buttonImages[i].color = tmpColor;
            mask >>= 1;
        }

        ChangeActivate();
        menuPositionInt = 0;
    }
    
    #endregion


    #region Public methods

    public void OpenOrCloseScroll()
    {
        if (!_isScrollOpen)
            OpenScroll();
        else
            CloseScroll();
    }

    public void OpenScroll()
    {
        if (!_isScrollOpen)
        {
            _animator.SetTrigger("Opentrig");
            _isScrollOpen = true;
            PlaySound();
        }
    }

    public void CloseScroll()
    {
        if (_isScrollOpen)
        {
            _animator.SetTrigger("Closetrig");
            _isScrollOpen = false;
            PlaySound();
            DisableMenu();
        }
    }

    #endregion


    #region Coroutines

    IEnumerator SortScroll()
    {
        CloseScroll();
        yield return new WaitForSeconds(0.6f);
        OpenScroll();
        Sort();
    }

    #endregion
}