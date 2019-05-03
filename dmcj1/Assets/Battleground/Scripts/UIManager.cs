using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //下方枪械UI显示
    [Header("枪械UI")]
    public Image[] gunImage;
    //目前子弹数量显示
    [Header("目前子弹的Text")]
    public Text bulletsAmountText;
    public GameObject bulletsControl;
    //备用子弹数
    [Header("备用子弹数")]
    public Text backupBulletsAmounts;
    //可拾取物品的信息
    [Header("可拾取物品的信息")]
    public Text itemText;
    //帧数显示
    [Header("帧数显示")]
    public Text fpsText;
    //绷带的数量在背包中
    [Header("绷带的数量")]
    public Text bandageAmountText;
    //饮料的数量在背包中
    [Header("饮料的数量")]
    public Text drinkAmountText;
    //血条
    [Header("血条")]
    public Slider healthSlider;
    //返回大厅的Button
    [Header("返回大厅的Button")]
    public Button backLobby;
    //返回游戏的Button
    [Header("返回游戏的Button")]
    public Button backGame;
    //返回操作的面板
    [Header("返回操作的面板")]
    public GameObject backPanel;
    //复活的时间显示
    [Header("复活的时间")]
    public Text resurrectionText;
    //准星图片
    [Header("准星图片")]
    public Image aimImage;
    //背包面板
    [Header("背包面板")]
    public GameObject backpackPanel;
    //绷带Button
    [Header("绷带Button")]
    public Button useBandageButton;
    //急救包Button
    [Header("急救包Button")]
    public Button useFirstAidButton;
    //饮料Button
    [Header("饮料Button")]
    public Button useDrinkButton;
    //使用道具时间显示
    [Header("使用道具时间显示")]
    public Text useItemTimeText;
    //加血时间显示的图片
    [Header("加血时间显示的图片")]
    public Image useItemImage;
    //其他玩家的Canvas
    [Header("其他玩家的Canvas")]
    public Canvas[] otherPlayerCanvas;
    //死亡面板
    [Header("死亡面板")]
    public GameObject deadPanel;
    //被攻击提示
    [Header("被攻击提示")]
    public GameObject damagePanel;
    //是否全自动开火
    [Header("开火模式")]
    public Text isAuto;

    //
    public Slider mouseSpeed;
    public Slider mouseAimSpeed;
}
