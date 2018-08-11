using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour {

    public Sprite[] fruitSprites;
    public GameObject particleObj;
    public int type;  //标记水果类型
    private Image imag;//根据水果类型读入sprite

    private int minY = -250;
    void Awake()
    {
        imag = GetComponent<Image>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RectTransform rtf = this.gameObject.transform as RectTransform;
        float newFruitY = rtf.anchoredPosition.y;
        if (newFruitY < minY)
        {
            Destroy(this.gameObject);
        }
	}
    public void SetType(int type)
    {
        this.type = type;
        imag.sprite = fruitSprites[type];
        imag.SetNativeSize();
        //如果水果类型时炸弹，需要启动粒子系统
        if(type == Contant.Type_Boom)
        {
            particleObj.SetActive(true);
        }
        else
        {
            particleObj.SetActive(false);
        }
    }
}
