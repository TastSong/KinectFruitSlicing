KinectFruitSlicing
========================

程序简介
------------------------
这是一个基于Kinect的切水果游戏<br>
## 进入游戏首页的时候需要用户举右手进行注册<br>
![](https://github.com/TastSong/KinectFruitSlicing/blob/master/KinectFruitSlicing/%E6%95%88%E6%9E%9C%E5%9B%BE/0.jpg) <br>
## 游戏页面<br>
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
## 游戏首页UI
* 建立2D游戏
* 新建canvas画布，并按照首页参照图进行布置画布，并调整game窗口的大小为1080*1920
* 这里强调一下就是Kinect的索引图的半透明是用canvas group组件实现的
* 手部的追踪是用第二个插件的DOME算法实现的
