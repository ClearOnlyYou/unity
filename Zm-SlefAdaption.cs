using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SlefAdaption : MonoBehaviour
{
    public Transform _cubeOne;
    public Transform _cubeTwo;

    public Transform _cubeThree;
    public Transform _cubeFour;
    private ImageTargetBehaviour mImageTargetBehaviour = null;
    Vector3 cubeSceenPoint;
    Vector3 cubeTwoSceenPoint;
    Vector3 cubeThreeSceenPoint;
    Vector3 cubeFourSceenPoint;
    void Start()
    {
        // We retrieve the ImageTargetBehaviour component
        // Note: This only works if this script is attached to an ImageTarget
        mImageTargetBehaviour = GetComponent<ImageTargetBehaviour>();

        if (mImageTargetBehaviour == null)
        {
            Debug.Log("ImageTargetBehaviour not found ");
        }
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

        // We define a point in the target local reference 
        // we take the bottom-left corner of the target, 
        // just as an example
        // Note: the target reference plane in Unity is X-Z, 
        // while Y is the normal direction to the target plane
        Vector3 pointOnTarget = new Vector3(-0.5f, 0, -0.5f / targetAspect);//pointOnTarget=new Vector3(-0.5,0,-0.37)

        Vector3 cubeOneCubeOnTarget = _cubeOne.position;
        Vector3 cubeTwoCubeOnTarget = _cubeTwo.position;


        Vector3 cubeThreeCubeOnTarget = _cubeThree.position;
        Vector3 cubeFourCubeOnTarget  = _cubeFour.position;

        // We convert the local point to world coordinates
        Vector3 targetPointInWorldRef = transform.TransformPoint(pointOnTarget);//将imagetarget的本地坐标转化成世界坐标

        Vector3 cubePointInWorldRef = transform.TransformPoint(cubeOneCubeOnTarget);
        Vector3 cubeTwoPointInWorldRef = transform.TransformPoint(cubeTwoCubeOnTarget);

        Vector3 cubeThreePointInWorldRef = transform.TransformPoint(cubeThreeCubeOnTarget);
        Vector3 cubeFourPointInWorldRef = transform.TransformPoint(cubeFourCubeOnTarget);
        // We project the world coordinates to screen coords (pixels)
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef); //将imagetarget的世界坐标转化成屏幕坐标

        cubeSceenPoint = Camera.main.WorldToScreenPoint(cubePointInWorldRef);
        cubeTwoSceenPoint = Camera.main.WorldToScreenPoint(cubeTwoPointInWorldRef);

        cubeThreeSceenPoint = Camera.main.WorldToScreenPoint(cubeThreePointInWorldRef);
        cubeFourSceenPoint = Camera.main.WorldToScreenPoint(cubeFourPointInWorldRef);




    }
    private void OnGUI()
    {
        float AB = (cubeTwoSceenPoint.x - cubeThreeSceenPoint.x) * (cubeTwoSceenPoint.x - cubeSceenPoint.x) + (cubeTwoSceenPoint.y - cubeThreeSceenPoint.y) * (cubeTwoSceenPoint.y - cubeSceenPoint.y);
        GUI.Label(new Rect(10, 100, 300, 100), "左上顶点坐标：" + "(" + cubeSceenPoint.x + "," + cubeSceenPoint.y + ")");
        GUI.Label(new Rect(10, 200, 300, 100), "右下顶点坐标：" + "(" + cubeThreeSceenPoint.x + "," + cubeThreeSceenPoint.y + ")");
        GUI.Label(new Rect(10, 300, 300, 100), "长：" + "(" + (cubeTwoSceenPoint.x- cubeSceenPoint.x) + ",宽：" +(cubeSceenPoint.y- cubeTwoSceenPoint.y) + ")");

        GUI.Label(new Rect(300, 300, 300, 100), AB + "");
        if (AB==0)
        {
            GUI.Label(new Rect(300, 100, 300, 100), "12与32垂直");
        }
        else
        {
            GUI.Label(new Rect(300, 100, 300, 100), "12与32不垂直");
        }  
        
     

        
    }
}

