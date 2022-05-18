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
        // Temp
        TableLoader.Instance.LoadTableData();

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
        int[,] range = CustomUtils.ConvertRangeToArray(data.range);
        _wizardController.Move(range);
    }

    private void Attack()
    {

    }
}
