/* 
 *Copyright(C) 2017 by zhanghao 
 *All rights reserved. 
 *FileName:     SlefAdaption 
 *Version:      2017 
 *UnityVersion：5.6.2
 *Date:         2017-12-1
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Runtime.InteropServices;
using System;

public class SlefAdaption : MonoBehaviour
{
    public WebCamTexture _webtex;

    public string _devicesName;

    private ImageTargetBehaviour mImageTargetBehaviour = null;

    int width;

    int height;

    int leftUpPositionX;

    int leftUpPositionY;

    [DllImport("user32.dll")]

    public static extern int WindowFromPoint(int xPoint, int yPoint);

    [DllImport("user32.dll")]       

    public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
    //参数分别是：x,y分别表示窗口左上角在屏幕的什么位置，width，height为0表示不改变窗口大小。

    [DllImport("user32.dll")]

    public static extern int GetForegroundWindow();

    [DllImport("user32.dll")]

    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]

    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]

    public static extern int GetWindowText(int hwnd, string lpString, int cch);

    [DllImport("user32.dll")]

    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]

    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]

    public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);
 
    void Start()
    {

        mImageTargetBehaviour = GetComponent<ImageTargetBehaviour>();

        if (mImageTargetBehaviour == null)
        {
            Debug.Log("ImageTargetBehaviour not found ");
        }

        //StartCoroutine(ForceRes());//此协程改变分辨率及更改程序运行窗口大小

        //StartCoroutine(StartCamera());//此协程开启计算机摄像头
    }

 
    void Update()
    {

        if (mImageTargetBehaviour == null)
        {
            Debug.Log("ImageTargetBehaviour not found");
            return;
        }

        Vector2 targetSize = mImageTargetBehaviour.GetSize();//得到imagetarget的像素大小

        float targetAspect = targetSize.x / targetSize.y;

        Vector3 pointOnTarget = new Vector3(-0.5f, 0, -0.5f / targetAspect);//pointOnTarget=new Vector3(-0.5,0,-0.37)


        Vector3 targetPointInWorldRef = transform.TransformPoint(pointOnTarget);//将imagetarget的本地坐标转化成世界坐标


        Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef); //将imagetarget的世界坐标转化成屏幕坐标

        Debug.Log("target point in screen coords: " + screenPoint.x + ", " + screenPoint.y);

        leftUpPositionX = Math.Abs((int)screenPoint.x);

        leftUpPositionY = Math.Abs((int)screenPoint.y);
    }

    private void OnGUI()
    {
        if (_webtex != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _webtex);
        }     
    }

    IEnumerator ForceRes()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            Screen.SetResolution(500,500, false);

            yield return new WaitForSeconds(1f);

            SetWindowPos(GetActiveWindow(), 0, leftUpPositionX, leftUpPositionY, 5, 5, 0x001);
        }
    }

    IEnumerator StartCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;

            _devicesName = devices[0].name;

            _webtex = new WebCamTexture(_devicesName, 400, 300, 12);

            _webtex.Play();
        }
    }
}

