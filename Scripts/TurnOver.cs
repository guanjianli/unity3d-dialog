using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TurnOver : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameParam.isAutoTalk) return; 
        TypewriterEffect tx = this.transform.Find("Text").gameObject.GetComponent<TypewriterEffect>() as TypewriterEffect;
        tx.NextWord();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
