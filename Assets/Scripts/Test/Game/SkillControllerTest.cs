using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SkillControllerTest : MonoBehaviour
{
    private WizardController _wizardController;

    public void Start()
    {
        _wizardController = GetComponent<WizardController>();
    }

    public void Update()
    {

    }

    public void Play(int code)
    {
        CardData data = TableDatas.Instance.GetCardData(code);

        switch (data.type)
        {
            case (int)Consts.SkillType.Move:
                Move(data);
                break;
        }

    }

    private void Move(CardData data)
    {
        _wizardController.Move(data.range);
    }

    private void Attack()
    {

    }
}
