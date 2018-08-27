KinectFruitSlicing
========================

程序简介
------------------------
这是一个基于Kinect的切水果游戏<br>
#### 进入游戏首页的时候需要用户举右手进行注册<br>
![](https://github.com/TastSong/KinectFruitSlicing/blob/master/KinectFruitSlicing/%E6%95%88%E6%9E%9C%E5%9B%BE/0.jpg) <br>
#### 游戏页面<br>
![](https://github.com/TastSong/KinectFruitSlicing/blob/master/KinectFruitSlicing/%E6%95%88%E6%9E%9C%E5%9B%BE/1.jpg) <br>

环境配置
------------------------
* [unity2017.1.1](https://unity3d.com/cn/get-unity/download/archive)安装
* 安装[Kinect2驱动](https://www.microsoft.com/en-us/download/details.aspx?id=44561)
* VS2013(用其他版本应该没有问题，个人比较喜欢2013而已)
* 导入[KinectForWindows_UnityPro](https://github.com/TastSong/KinectUnityEazyDome/tree/master/MyKinectUnityTest/KinectForWindows_UnityPro_2.0.1410)包中的`Kinect.2.0.1410.19000`
* 再导入[Kinect v2.9 with MS-SDK20](https://pan.baidu.com/s/1IzXqhmEzjPIv9Wcg6BYd6g)，解压密码`xqps`

游戏设计
-----------------------
#### 游戏首页UI
* 建立2D游戏
* 新建canvas画布，并按照首页参照图进行布置画布，并调整game窗口的大小为1080*1920
* 这里强调一下就是Kinect的索引图的半透明是用canvas group组件实现的
* 手部的追踪是用第二个插件的DOME算法实现的

#### 水果的随机生成和销毁
* fruit.cs
  * 实现水果的分类包括炸弹
  * 设置当水果跳出游戏画面后自动销毁
* game.cs
  * 随机生成水果
  * 随机位置发射
  * 手部跟踪，实现拖尾效果
  * 切中水果判断，若切中两半水果的生成
  * 计分方式，切中水果加一分，切中炸弹减一分
  * 计时器，判断游戏结束
  
开发中的问题
----------------------
* 坐标转化算法，这里强力推荐DOME中的手部跟踪的坐标转算法
* 相机深度，对渲染场景的影响，depth越大，越后渲染
* 粒子系统的同步效果，可以用另一个相机渲染；这里我直接设置摄像机是正交模式，解决拖尾效果的同步渲染

  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
