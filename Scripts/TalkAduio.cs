using System.Collections;
using UnityEngine;

public class TalkAduio: MonoBehaviour
{

    //音乐文件
    AudioSource source;
    //音量
    public float musicVolume = 0.95F;

    private string[] playList = { "10001", "", "10002", "20001", "10003", "20002", "10004", "", "10005",  "20003", "10006", "20004", "10007", "20005"}; 

	private int currentPro = 0;

    private Coroutine currentCoro;
    void Start()
    {
        source = (AudioSource)this.gameObject.GetComponent("AudioSource");
    
        source.minDistance = 1.0f;
        source.maxDistance = 50;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.transform.position = transform.position;
       
        PlayNext();
    }

	public void PlayNext (){
        GameParam.isTalkEnd = false;
        if(currentCoro != null)StopCoroutine(currentCoro);
        if (playList[currentPro] != "")
        {
            string id = "audios/" + playList[currentPro];
            Debug.Log("play:"+id);
            Play(id, OnComp);
        }else {
            Debug.Log("has no words say:");
            source.Stop();
            GameParam.isTalkEnd = true;
        }
		currentPro++;
	}

    public void OnComp() {
        Debug.Log("music end");
    }


    public float updateStep = 0.1f;
    public int sampleDataLength = 512;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;

    // Use this for initialization
    void Awake()
    {
        clipSampleData = new float[sampleDataLength];

    }
    public void Update()
    {
        try {
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep && source.isPlaying)
            {
                currentUpdateTime = 0f;
                if (source.clip.samples - source.timeSamples >= sampleDataLength) {
                    source.clip.GetData(clipSampleData, source.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
                    clipLoudness = 0f;
                    foreach (var sample in clipSampleData)
                    {
                        clipLoudness += Mathf.Abs(sample);
                    }
                    clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
                    //Debug.Log(clipLoudness * 5);
                    GameParam.amplitude = clipLoudness * 9;
                }
            }
        }
        catch(System.Exception e) {
            Debug.Log(e);
        }

    }

    public delegate void AudioCallBack();
    private void Play(string str, AudioCallBack callback)
    {
        AudioClip clip = (AudioClip) Resources.Load(str, typeof(AudioClip));//调用Resources方法加载AudioClip资源
		if (clip == null) {
			Debug.Log("path is no exist");
			return;
		}
        source.clip = clip;
		source.Play();

        currentCoro = StartCoroutine(DelayedCallback(clip.length, callback));
    }

    private IEnumerator DelayedCallback(float time, AudioCallBack callback)
    {
        yield return new WaitForSeconds(time);
        GameParam.isTalkEnd = true;
        if(callback != null) callback();
    }
}
