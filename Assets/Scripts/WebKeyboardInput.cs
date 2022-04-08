using UnityEngine;

public class WebKeyboardInput : MonoBehaviour
{
    void Start()
    {
    #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keabord inputs
            WebGLInput.captureAllKeyboardInput = false;
    #endif
    }

    public void FocusCanvas(string focus)
    {
        Debug.Log(focus);
        if (focus == "0")
        {
            WebGLInput.captureAllKeyboardInput = true;
        } else
        {
            WebGLInput.captureAllKeyboardInput = false;
        }
    }
}
