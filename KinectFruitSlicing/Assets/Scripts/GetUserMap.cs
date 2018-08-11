using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUserMap : MonoBehaviour {

    public RawImage kinectImg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool isInit = KinectManager.Instance.IsInitialized();
        if(isInit)
        {
            //得到深度帧(索引完成以后的)，如果在KinectManager中开启彩色帧，则抠图为彩色
            Texture2D kinectPic = KinectManager.Instance.GetUsersLblTex();
            kinectImg.texture = kinectPic;
        }
	}
}
