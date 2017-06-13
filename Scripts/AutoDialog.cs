using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class AutoDialog : MonoBehaviour, IPointerClickHandler {

    Image img;
 
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        GameParam.isAutoTalk = !GameParam.isAutoTalk;
    }

    // Use this for initialization
    void Start()
    {
        img = (Image)this.gameObject.GetComponent("Image");
        SetImg();
    }

    // Update is called once per frame
    private bool nowAutoStatus;

    void Update()
    {
        if (nowAutoStatus != GameParam.isAutoTalk)
        {
            SetImg();
            nowAutoStatus = GameParam.isAutoTalk;
        }
    }

    void SetImg()
    {
        if (GameParam.isAutoTalk)
        {
            img.sprite = (Sprite)Resources.Load("zidong", typeof(Sprite));
        }
        else
        {
            img.sprite = (Sprite)Resources.Load("shoudong", typeof(Sprite));
        }
    }
}
