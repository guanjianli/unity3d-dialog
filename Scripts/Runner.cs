using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using live2d;

public class Runner : MonoBehaviour, IPointerClickHandler {

    private GameObject prefab;

    // Use this for initialization
    void Start () {
        Live2D.init();
        ChangMe("prefab/Primary");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ExitMe()
    {
        Destroy(prefab);
        ChangMe("prefab/Primary");
    }

    public void ChangMe(string p) {
        prefab = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(p));
        //UnityEngine.Object.Destroy(prefab);
        prefab.transform.SetParent(gameObject.transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        var n = eventData.pointerCurrentRaycast.gameObject.name;
        Debug.Log("--->" + n);
        switch (n) {
            case "JuQing":
                Destroy(prefab);
                ChangMe("prefab/Story");
                break;
        }
    }
}
