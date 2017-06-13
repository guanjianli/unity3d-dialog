using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitBtn : MonoBehaviour, IPointerClickHandler {


    private Runner r;

    public void OnPointerClick(PointerEventData eventData)
    {
        r.ExitMe();
    }

    // Use this for initialization
    void Start () {
        r = GameObject.Find("GameRunner").gameObject.GetComponent<Runner>() as Runner;
    }

	// Update is called once per frame
	void Update () {
	    
	}

}
