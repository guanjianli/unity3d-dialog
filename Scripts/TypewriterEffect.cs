using System;  
using UnityEngine;  
using UnityEngine.UI;  
using UnityEngine.Events;
using System.Collections;

/// <summary>  
/// 此脚本是能够将文本字符串随着时间打字或褪色显示。  
/// </summary>  

[RequireComponent(typeof(Text))]  
//[AddComponentMenu("Typewriter Effect")]  
public class TypewriterEffect : MonoBehaviour, ICustomMessageTarget
{  
	public UnityEvent myEvent;  
	public int charsPerSecond = 6;

	private bool isActive = false;

    private int currentCharsPerSecond;//新速度
    private float timer;  
	private string words;  
	private Text mText;
    private int whichWords = 0;

    private GameObject qt;
    private GameObject lj;
    private GameObject pageBtn;
    private GameObject nameLj;
    private GameObject nameQt;
    private Coroutine currentCoro;

    private string[] dialogs = {
        "那帮人整日净劝我出仕，可是我根本就志不在此，身负所谓的才干就必须选择这条道路吗？",
        "……",
        "你在看什么？",
        "我在看大哥哥的扇子，上面的图案好漂亮，唔……\n ‘且将新火试新茶，诗酒趁年华’，这写的是什么意思啊？",
        "嗯…… \n‘且将新火试新茶，诗酒趁年华’？",
        "咦？",
        "‘且将新火试新茶，诗酒趁年华’，这道出的不正是那超然物外之理吗？",
        "……",
        "‘夫求祸而辞福，岂人之情也哉？物有以盖之矣’，我因耽于外物而愀然不乐，却不知解惑之语近在眼前，哈……哈哈！",
        "诶，发生了什么好笑的事情吗？",
        "既然如此，区区在位者说什么又与我何干呢——啊，抱歉，方才略有心事，你……叫什么名字？",
        "我吗？我、我叫青团！",
        "青团……就是那个曾无意间救过李秀成一命的青团啊。青团，给。",
        "哇，好漂亮的山茶花啊，虽然不知道为什么会被送花，不过大哥哥你终于笑起来真是太好了！"
    };

	void Start()  
	{  
		if (myEvent == null)  
			myEvent = new UnityEvent();
        ReloadText();
        qt = GameObject.Find("Qingtuan-Live2d");
        lj = GameObject.Find("Longjingxiaren");
        pageBtn = GameObject.Find("PageNext");
        nameLj = GameObject.Find("Name_LongJing");
        nameQt = GameObject.Find("Name_QingTuan");
        nameLj.SetActive(true);
        nameQt.SetActive(false);
        qt.SetActive(false);
        lj.SetActive(true);
        currentCharsPerSecond = Mathf.Max(1, charsPerSecond);  		
	}  

	void ReloadText()  
	{
        GameParam.isWordsEnd = false;
        isAutoPaging = false;
        words = dialogs[whichWords];
        GetComponent<Text>().text = string.Empty;
        timer = 0;
        isActive = true;
        mText = GetComponent<Text>();
	}  


	void OnFinish()  
	{  
		isActive = false;  
		timer = 0;
        GameParam.isWordsEnd = true;
		GetComponent<Text>().text = dialogs[whichWords];
        try  
		{  
			myEvent.Invoke();  
		}  
		catch (Exception)  
		{  
			Debug.Log("bug");  
		}  
	}

    public void NextWord()
    {
        if (currentCoro != null) {
            StopCoroutine(currentCoro);
        }
        
        if (isActive == true)
        {
            currentCharsPerSecond = 50;
        }
        else {
            whichWords++;

            currentCharsPerSecond = charsPerSecond;
            if (whichWords < dialogs.Length)
            {
                if (whichWords % 2 == 0)
                {
                    qt.SetActive(false);
                    nameQt.SetActive(false);
                    lj.SetActive(true);
                    nameLj.SetActive(true);
                }
                else
                {
                    lj.SetActive(false);
                    nameLj.SetActive(false);
                    qt.SetActive(true);
                    nameQt.SetActive(true);
                    //更像表情
                    Motion mo = qt.gameObject.GetComponent<Motion>() as Motion;
                    mo.ChangeMotion();
                    //更换声音

                }
                TalkAduio ta = GameObject.Find("TalkAduio").gameObject.GetComponent<TalkAduio>() as TalkAduio;
                ta.PlayNext();
                ReloadText();
            }
            else {
                Runner r = GameObject.Find("GameRunner").gameObject.GetComponent<Runner>() as Runner;
                r.ExitMe();
            }
        }
    }

    private bool isAutoPaging;
    void Update()  
	{
        if (isActive)
        {
            try
            {
                mText.text = words.Substring(0, (int)(currentCharsPerSecond * timer));
                timer += Time.deltaTime;
                pageBtn.SetActive(false);
            }
            catch (Exception)
            {
                pageBtn.SetActive(true);
                OnFinish();
            }
        }

        //如果已经播放完声音，且文字已演示完，自动播放一下页
        if (GameParam.isWordsEnd && GameParam.isTalkEnd && !isAutoPaging && GameParam.isAutoTalk) {
            isAutoPaging = true;
            currentCoro = StartCoroutine(DelayedNext(0.6f));
        }
    }

    private IEnumerator DelayedNext(float time)
    {
        yield return new WaitForSeconds(time);
        if (GameParam.isAutoTalk) {
            NextWord();
        }
        isAutoPaging = false;
    }

    public void Message1()
    {
        this.charsPerSecond = 50;
        Debug.Log("Message1 received.");
    }
    public void Message2()
    {
        Debug.Log("Message2 received.");
    }

}