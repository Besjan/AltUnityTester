using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Altom.AltUnityTester;

public class NewInputSystem : MonoBehaviour
{
    public static InputTestFixture InputTestFixture = new InputTestFixture();
    public static NewInputSystem Instance;
    public static Keyboard Keyboard;
    public static Mouse Mouse;
    public static Gamepad Gamepad;
    public static Touchscreen Touchscreen;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        InputTestFixture = new InputTestFixture();
        InputTestFixture.Setup();
        Keyboard = (Keyboard)InputSystem.GetDevice("AltUnityKeyboard");
        if (Keyboard == null)
        {
            Keyboard = InputSystem.AddDevice<Keyboard>("AltUnityKeyboard");
        }

        Mouse = (Mouse)InputSystem.GetDevice("AltUnityMouse");
        if (Mouse == null)
        {
            Mouse = InputSystem.AddDevice<Mouse>("AltUnityMouse");

        }
        Gamepad = (Gamepad)InputSystem.GetDevice("AltUnityGamepad");
        if (Gamepad == null)
        {
            Gamepad = InputSystem.AddDevice<Gamepad>("AltUnityGamepad");

        }
        Touchscreen = (Touchscreen)InputSystem.GetDevice("AltUnityTouchscreen");
        if (Touchscreen == null)
        {
            Touchscreen = InputSystem.AddDevice<Touchscreen>("AltUnityTouchscreen");
        }

    }
    internal static IEnumerator ScrollLifeCircle(float speed, float duration)
    {

        float currentTime = 0;
        float frameTime = 0;// using this because of a bug with yield return which waits only every other iteration
        while (currentTime <= duration - Time.fixedUnscaledDeltaTime)
        {
            InputTestFixture.Move(Mouse.current.scroll, new Vector2(speed * frameTime / duration, speed * frameTime / duration));
            var initialTime = Time.fixedUnscaledTime;
            yield return null;
            var afterTime = Time.fixedUnscaledTime;
            frameTime = afterTime - initialTime;
            currentTime += frameTime;
        }
        InputTestFixture.Set(Mouse.current.scroll, new Vector2(0, 0));
    }

    
    internal static IEnumerator ClickElementLifeCycle(GameObject target, int count, float interval)
    {
        Mouse.MakeCurrent();
        UnityEngine.Vector3 screenPosition;
        AltUnityRunner._altUnityRunner.FindCameraThatSeesObject(target, out screenPosition);
        InputTestFixture.Set(Mouse.current.position,screenPosition);
        for(int i=0;i<count;i++)        
        {
            float time = 0;
            InputTestFixture.Click(Mouse.leftButton);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
            time += Time.fixedUnscaledDeltaTime;
            if (i != count - 1 && time < interval)
                yield return new WaitForSecondsRealtime(interval-time);
        }
    }

    internal static IEnumerator ClickCoordinatesLifeCycle(UnityEngine.Vector2 screenPosition,int count, float interval)
    {
        Mouse.MakeCurrent();
        InputTestFixture.Set(Mouse.current.position,screenPosition);
        for( int i=0; i<count; i++)
        {
            float time = 0;
            InputTestFixture.Click(Mouse.leftButton);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
            time += Time.fixedUnscaledDeltaTime;
            if (i != count - 1 && time < interval)
                yield return new WaitForSecondsRealtime(interval-time);
        }
    }


}


