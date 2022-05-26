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
    private List<CardInScroll> _cards;
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
        TableLoader.Instance.LoadTableData();
        StartCoroutine(CacheSpriteSource.Instance.Init());

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

        _cards = new List<CardInScroll>();
        foreach (int key in TableDatas.Instance.cardDatas.Keys)
        {
            CardData cardData = TableDatas.Instance.cardDatas[key];
            GameObject cardObj = Instantiate(cardTemplate, backdrops.transform);
            
            // set name & image
            cardObj.name = $"{cardObj.name}{cardData.text}";
            cardObj.GetComponent<Image>().sprite = CacheSpriteSource.Instance.GetSource(key);
            
            // set data & on click event listener
            CardInScroll cardInScroll = cardObj.GetComponent<CardInScroll>();
            cardInScroll.cardData = cardData;
            cardObj.GetComponent<Button>().onClick.AddListener(delegate { controller.OnCardAddBtnClick(key); });
            
            _cards.Add(cardInScroll);
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
                for (int i = 0; i < _cards.Count; i++)
                {
                    _cards[i].gameObject.SetActive(true);
                }
                break;
            
            case (int) SectionType.Move:
                backdropLength = 2;
                for (int i = 0; i < _cards.Count; i++)
                {
                    GameObject obj = _cards[i].gameObject;
                    if (_cards[i].cardData.type == (int) Card.SkillType.Move)
                        obj.SetActive(true);
                    else
                        obj.SetActive(false);
                }
                break;
            
            case (int) SectionType.Attack:
                backdropLength = 3;
                for (int i = 0; i < _cards.Count; i++)
                {
                    GameObject obj = _cards[i].gameObject;
                    if (_cards[i].cardData.type == (int) Card.SkillType.Attack)
                        obj.SetActive(true);
                    else
                        obj.SetActive(false);
                }
                break;
            
            case (int) SectionType.Special:
                backdropLength = 2;
                for (int i = 0; i < _cards.Count; i++)
                {
                    GameObject obj = _cards[i].gameObject;
                    if (_cards[i].cardData.type == (int) Card.SkillType.Special)
                        obj.SetActive(true);
                    else
                        obj.SetActive(false);
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
                tmpPos = new Vector3(_buttonRectTrans[i].localPosition.x, -0.44f, 0.03f);
                tmpColor = _selectedColor;
            }
            else
            {
                tmpPos = new Vector3(_buttonRectTrans[i].localPosition.x, -0.43f, 0.04f);
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
        yield return new WaitForSeconds(0.8f);
        OpenScroll();
        Sort();
    }

    #endregion
}