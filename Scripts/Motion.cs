using UnityEngine;
using live2d;
using System;

[ExecuteInEditMode]
public class Motion: MonoBehaviour 
{
	private Live2DModelUnity live2DModel;
	private Live2DMotion motion;
	private MotionQueueManager motionMgr;


    private Matrix4x4 live2DCanvasPos;

	public TextAsset mocFile ;
	public Texture2D[] textureFiles ;
	public TextAsset [] motionFiles;


	void Start () 
	{
		//Live2D.init();

		live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

		for (int i = 0; i < textureFiles.Length; i++)
		{
			live2DModel.setTexture(i, textureFiles[i]);
		}

		float modelWidth = live2DModel.getCanvasWidth();
		live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
        if (motionMgr == null) motionMgr = new MotionQueueManager();
    }


	void Update()
	{
		if (live2DModel == null) return;
		live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

		if (!Application.isPlaying)
		{
			live2DModel.update();
			return;
		}
        if (motionMgr.isFinished()) {
            SetMotion(Live2DMotion.loadMotion(motionFiles[0].bytes));
        }

        //double t = (UtSystem.getUserTimeMSec() / 1000.0) * 2 * Math.PI;
        //live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", (float)(2 * Math.Sin(t / 0.5)));

        //Debug.Log(live2DModel.getParamFloat("PARAM_MOUTH_OPEN_Y"));
        motionMgr.updateParam(live2DModel);

        //live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", (float)(2 * Math.Sin(t / 0.5)));
        //Debug.Log(live2DModel.getParamFloat("PARAM_MOUTH_OPEN_Y"));
        /*先播放mtn文件，再setParamFloat，不然会被mtn文件覆盖*/
        if (!GameParam.isTalkEnd)
        {
            //Debug.Log(GameParam.amplitude);
            live2DModel.setParamFloat("PARAM_MOUTH_FORM", (float)1f);
            live2DModel.setParamFloat("PARAM_MOUTH_SIZE", (float)0.8f);
            live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", (float)GameParam.amplitude);
        }
        live2DModel.update();
    }

    private int [] motionList = { 2, 3, 0 ,0 ,7,0,1};
    private int currentMotionIndex = 0;

    public void ChangeMotion() {
        if(motionMgr == null) motionMgr = new MotionQueueManager();
        motionMgr.stopAllMotions();
        //Debug.Log("--表情--->" + motionList[currentMotionIndex]);
        motion = Live2DMotion.loadMotion(motionFiles[motionList[currentMotionIndex]].bytes);
        SetMotion(motion);
        currentMotionIndex++;
    }

    //默认表情
    private void SetMotion(Live2DMotion motion) {
        motion.setFadeIn(0);
        motion.setFadeOut(0);
        motionMgr.startMotion(motion);
    }

	void OnRenderObject()
	{
		if (live2DModel == null) return;
		if ((Camera.current.cullingMask & gameObject.layer) > 0)
		{
			if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW) live2DModel.draw();
		}
	}

    private void OnDestroy()
    {
        //Live2D.dispose();
		if(live2DModel != null) live2DModel.releaseModel();
    }
}