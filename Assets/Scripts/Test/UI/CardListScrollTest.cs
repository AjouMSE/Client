using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardListScrollTest : MonoBehaviour
{
    [SerializeField] private ScrollScript3DTest scroll3D;
    [SerializeField] private GameObject[] buttons;

    private RectTransform[] rectTransforms;
    private Image[] images;

    private Color baseColor, selectedColor;

    private int isOpen = 0b1000;

    public void OnSkillCardClick()
    {
        Debug.Log("Skill card click!");
    }

    public void OnCategoryBtnClick(int idx)
    {
        isOpen = idx;
        StartCoroutine(OpenAndClose());
    }

    private void Start()
    {
        baseColor = new Color(1, 0.87f, 0.75f);
        selectedColor = new Color(1, 0.75f, 0.36f);
        rectTransforms = new RectTransform[4];
        images = new Image[4];

        for (int i = 0; i < buttons.Length; i++)
        {
            rectTransforms[i] = buttons[i].GetComponent<RectTransform>();
            images[i] = buttons[i].GetComponent<Image>();
        }
        
        SortScroll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            scroll3D.OpenOrCloseScroll();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(OpenAndClose());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Move Right");
            scroll3D.MoveMenuRight();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Move Right");
            scroll3D.MoveMenuLeft();
        }
    }

    private void SortScroll()
    {
        int mask = 8;
        for (int i = 0; i < 4; i++)
        {
            Vector3 tmpPos;
            Color tmpColor;
            if ((isOpen & mask) > 0)
            {
                tmpPos = new Vector3(rectTransforms[i].localPosition.x, -0.44f, 0.03f);
                tmpColor = selectedColor;
            }
            else
            {
                tmpPos = new Vector3(rectTransforms[i].localPosition.x, -0.4f, 0.04f);
                tmpColor = baseColor;
            }
            rectTransforms[i].localPosition = tmpPos;
            images[i].color = tmpColor;
            mask >>= 1;
        }
    }
    
    IEnumerator OpenAndClose()
    {
        scroll3D.CloseScroll();
        yield return new WaitForSeconds(0.8f);
        scroll3D.OpenScroll();
        SortScroll();
    }
}