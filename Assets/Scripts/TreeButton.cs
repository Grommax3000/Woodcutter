using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeButton : MonoBehaviour
{
    private TreeData data;
    public Image icon;

    public void SetData(TreeData dataArg)
    {
        data = dataArg;
        icon.sprite = data.icon;
    }

    public void ClickButton()
    {
        GameManager.instance.StartPlanting(data);
    }
}
