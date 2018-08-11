using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    //将克隆水果资源赋值
    public Fruit fruitClone;
    //初始化的水果
    private Fruit newFruit;
    //定义跟踪的骨骼
    private KinectInterop.JointType handRightType = KinectInterop.JointType.HandRight;
    private KinectInterop.JointType handLeftType = KinectInterop.JointType.HandLeft;
    //手部跟踪平滑度
    public float smoothFactor = 5f;
    private float distanceToCamera = 10f;
    //左右手拖尾效果
    public GameObject rightTrail;
    public GameObject leftTrail;
    //画板边界定义
    private int maxX = 320;
    private int minX = -320;
    private int minY = -250;
    //定义水果上抛的力
    public int forceX = 500;
    public int forceY = 8000;
    //获取玩家ID
    private long userId;
    //计分器
    private int score = 0;
    public Text scoreText;
    //计时器
    public Text gameTimeText;
    private int gameTime = 0;
    private float floatTime = 0;//用于方法一：用来递增delatime的1秒 
    //游戏结束画面
    public Image gameOverImag;
	// Use this for initialization
	void Start () {
        CreateNewFruit();
	}	
	// Update is called once per frame
	void Update () {
   
        //开始游戏计时
        GameTimer();        
        if(this.gameTime < 30)
        {
            gameTimeText.text = this.gameTime.ToString() + "秒";
            //得到左右手的屏幕位置
            Vector3 handRightPos = GetjointPos((int)handRightType);
            Vector3 handLeftPos = GetjointPos((int)handLeftType);
            //添加左右手的拖尾效果
            TrailingFollow(handRightPos, handLeftPos);

            if (newFruit == null)
            {
                CreateNewFruit();
            }

            //判断是否切中水果，包括炸弹
            bool isHit = IsHit(handRightPos, handLeftPos);
            /**** 如果水果切中就调用DeestroyFruit销毁，如果没有切中就会调用Fruit脚本销毁****/
            //销毁并计分
            DeestroyFruit(isHit);
        }
        else
        {
            gameTimeText.text = "30秒";
            gameOverImag.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
       
	}
    //切中“水果”，销毁水果，生成两半
    private void newFruitInit(Fruit fruit, bool isLeft)
    {
        RectTransform fruitCloneR = fruitClone.transform as RectTransform;
        fruit.transform.SetParent(transform);
        RectTransform rtf = fruit.transform as RectTransform;
        rtf.anchoredPosition3D = new Vector3(0, 0, 0);
        rtf.anchoredPosition = fruitCloneR.anchoredPosition;
        rtf.localScale = new Vector3(1, 1, 1);

        Rigidbody2D rig = fruit.GetComponent<Rigidbody2D>();
        if(isLeft)
        {
            rig.AddForce(new Vector2(-forceX, forceY / 4));
        }
        else
        {
            rig.AddForce(new Vector2(forceX, forceY / 4));
        }
    }
    void CreateLeftRightFruit()
    {
        Fruit leftFruit = Instantiate(fruitClone, fruitClone.transform.position, fruitClone.transform.rotation) as Fruit;
        leftFruit.SetType(newFruit.type + 1); //生成随机水果
        newFruitInit(leftFruit, true);

        Fruit rightFruit = Instantiate(fruitClone, fruitClone.transform.position, fruitClone.transform.rotation) as Fruit;
        rightFruit.SetType(newFruit.type + 2); //生成随机水果
        newFruitInit(rightFruit, false);
    }
    //销毁切中的水果，若不为炸弹就生成两半水果
    void DeestroyFruit(bool isHit)
    {
        if (isHit)
        {
            if(newFruit.type != Contant.Type_Boom)
            {
                this.score++;
                CreateLeftRightFruit();
            }
            else
            {
                this.score--;
            }
            scoreText.text = score.ToString() + "分";
            Destroy(newFruit.gameObject);
            CreateNewFruit();
        }
        
    }
    //生成随机水果,随机发射位置
    void CreateNewFruit()
    {
        newFruit = Instantiate(fruitClone) as Fruit; //调用预制,克隆了一个物体默认克隆出来的物体是在场景跟下面
        newFruit.transform.SetParent(transform);
        //由于我们将Game脚本添加给了canvas，所以他的RectTransform以canvas为基准
        RectTransform fruitRT = newFruit.transform as RectTransform;
        fruitRT.anchoredPosition3D = new Vector3(0, 0, 0);
        int fruitX = Random.Range(minX, maxX);
        fruitRT.anchoredPosition = new Vector2(fruitX, minY);
        fruitRT.localScale = new Vector3(1, 1, 1);
        int[] fruitType = { Contant.Type_Boom, Contant.Type_Apple, Contant.Type_Banana, Contant.Type_Basaha, Contant.Type_Peach, Contant.Type_Sandia };
        int fruitTypeIndex = Random.Range(0, 5);
        newFruit.SetType(fruitType[fruitTypeIndex]);

        Rigidbody2D rig = newFruit.GetComponent<Rigidbody2D>();
        if(fruitX > 0)
        {
            rig.AddForce(new Vector2(-forceX, forceY));
        }
        else 
        {
            rig.AddForce(new Vector2(forceX, forceY));
        }
    }

    //得到骨骼的游戏界面的坐标
    Vector3 GetjointPos(int iJointIndex)
    {
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {            
         
            if (manager.IsUserDetected())
            {
                this.userId = manager.GetPrimaryUserID();

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                    Vector3 posJoint = manager.GetJointKinectPosition(userId, iJointIndex);

                    if (posJoint != Vector3.zero)
                    {
                        // 3 d位置深度
                        Vector2 posDepth = manager.MapSpacePointToDepthCoords(posJoint);
                        ushort depthValue = manager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

                        if (depthValue > 0)
                        {
                            // 颜色pos深度pos
                            Vector2 posColor = manager.MapDepthPointToColorCoords(posDepth, depthValue);

                            float xNorm = (float)posColor.x / manager.GetColorImageWidth();
                            float yNorm = 1.0f - (float)posColor.y / manager.GetColorImageHeight();
                          
                            Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(xNorm, yNorm, distanceToCamera));
                            return vPosOverlay;
                        }
                    }
                }

            }
        }
        return Vector3.zero;
    }
    //拖尾效果添加
    void TrailingFollow(Vector3 handRightPos, Vector3 handLeftPos)
    {
        if (IsRightHandOpen())
        {
            rightTrail.transform.position = Vector3.Lerp(rightTrail.transform.position, handRightPos, smoothFactor * Time.deltaTime);
        }
        if (IsLeftHandOpen())
        {
            leftTrail.transform.position = Vector3.Lerp(leftTrail.transform.position, handLeftPos, smoothFactor * Time.deltaTime);
        }     
    }
    bool IsRightHandOpen()
    {
        KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(this.userId);
        if (rightHandState == KinectInterop.HandState.Open)
        {
            return true;
        }
        return false;
    }
    bool IsLeftHandOpen()
    {
        KinectInterop.HandState leftHandState = KinectManager.Instance.GetLeftHandState(this.userId);
        if (leftHandState == KinectInterop.HandState.Open)
        {
            return true;
        }
        return false;
    }
    //判断是否切中“水果”
    bool IsHit(Vector3 handRightPos, Vector3 handLeftPos)
    {
        Vector2 handRightJoinsPos = new Vector2(handRightPos.x, handRightPos.y);
        RectTransform rtf = newFruit.transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(rtf, handRightJoinsPos, null))
        {//意外的bug，如果只有一个摄像机，就只能最后的参数填写null
            if (IsRightHandOpen())
            {
                return true;
            }
        }
        Vector2 handLeftJoinsPos = new Vector2(handLeftPos.x, handLeftPos.y);
        if (RectTransformUtility.RectangleContainsScreenPoint(rtf, handLeftJoinsPos, null))
        {
            if (IsLeftHandOpen())
            {
                return true;
            }
        }
        return false;
    }
    //游戏计时器
    void GameTimer()
    {
        floatTime += Time.deltaTime;//0秒增加每帧时间

        if (floatTime > 1)//到达1秒的时候，输出当前游戏时间（秒）
        {
            this.gameTime++;
            floatTime = 0;
        }
      
    }
}
