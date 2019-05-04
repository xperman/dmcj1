using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateView : MonoBehaviour
{
    //private:
    //旋转的临时变量
    private float rotationX = 0f;
    private float humanRotationX = 0f;
    //摄像机
    private Camera myCamera;
    //public: 
    //鼠标水平速度
    public float sensitivityHor = 2.0f;
    //鼠标垂直速度
    public float sensitivityVert = 2.0f;
    //鼠标向上最小翻转角度
    public float minimumVert = -45.0f;
    //鼠标向上最大翻转角度
    public float maximumVert = 45.0f;
    //平滑差值系数
    public float smoothTime = 2f;
    public GameObject human;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible =false;
        myCamera = this.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Rotate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Rotate()
    {
        float x = Input.GetAxis("Mouse Y");
        float y = Input.GetAxis("Mouse X");

        rotationX -= x * sensitivityVert;
        humanRotationX-= x * sensitivityVert;

        rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
        humanRotationX = Mathf.Clamp(humanRotationX, -10f, 10f);
        float delta = y * sensitivityHor;
        float rotationY = transform.localEulerAngles.y + delta;
        float smoothX = Smooth(rotationX);
        float smoothXHuman = Smooth(humanRotationX);
        transform.localEulerAngles = new Vector3(0f, rotationY, 0);
        myCamera.transform.localEulerAngles = new Vector3(smoothX, 0, 0);
        human.transform.localEulerAngles= new Vector3(smoothXHuman, 0, 0);
    }

    private float Smooth(float x)
    {
        float smooth = Mathf.Lerp(x, x, 2f * Time.deltaTime);
        return smooth;
    }
}
