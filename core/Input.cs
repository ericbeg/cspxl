﻿#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OtkInput = OpenTK.Input;

namespace pxl
{

    public static class Input
    {
        internal static int mouse_x = 0;
        internal static int mouse_y = 0;

        public static bool IsKeyDown( Key key )
        {
            OtkInput.KeyboardState ks = OtkInput.Keyboard.GetState();
            return ks.IsKeyDown((OtkInput.Key)key); 
        }

        public static bool IsKeyUp(Key key)
        {
            OtkInput.KeyboardState ks = OtkInput.Keyboard.GetState();
            return ks.IsKeyUp((OtkInput.Key)key);
        }

        public static bool IsMouseButtonDown( MouseButton button )
        {
            OtkInput.MouseState ms = OtkInput.Mouse.GetState();
            return ms.IsButtonDown((OtkInput.MouseButton)button);
        }

        public static bool IsMouseButtonUp(MouseButton button)
        {
            OtkInput.MouseState ms = OtkInput.Mouse.GetState();
            return ms.IsButtonUp((OtkInput.MouseButton)button);
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(mouse_x, Screen.height -1 - mouse_y);
        }

        public static float GetMouseScroll()
        {
            OtkInput.MouseState ms = OtkInput.Mouse.GetState();
            return ms.WheelPrecise;
        }


    }

    public enum Key
    {
        Unknown = 0,
        LShift = 1,
        ShiftLeft = 1,
        RShift = 2,
        ShiftRight = 2,
        LControl = 3,
        ControlLeft = 3,
        RControl = 4,
        ControlRight = 4,
        AltLeft = 5,
        LAlt = 5,
        AltRight = 6,
        RAlt = 6,
        WinLeft = 7,
        LWin = 7,
        RWin = 8,
        WinRight = 8,
        Menu = 9,
        F1 = 10,
        F2 = 11,
        F3 = 12,
        F4 = 13,
        F5 = 14,
        F6 = 15,
        F7 = 16,
        F8 = 17,
        F9 = 18,
        F10 = 19,
        F11 = 20,
        F12 = 21,
        F13 = 22,
        F14 = 23,
        F15 = 24,
        F16 = 25,
        F17 = 26,
        F18 = 27,
        F19 = 28,
        F20 = 29,
        F21 = 30,
        F22 = 31,
        F23 = 32,
        F24 = 33,
        F25 = 34,
        F26 = 35,
        F27 = 36,
        F28 = 37,
        F29 = 38,
        F30 = 39,
        F31 = 40,
        F32 = 41,
        F33 = 42,
        F34 = 43,
        F35 = 44,
        Up = 45,
        Down = 46,
        Left = 47,
        Right = 48,
        Enter = 49,
        Escape = 50,
        Space = 51,
        Tab = 52,
        Back = 53,
        BackSpace = 53,
        Insert = 54,
        Delete = 55,
        PageUp = 56,
        PageDown = 57,
        Home = 58,
        End = 59,
        CapsLock = 60,
        ScrollLock = 61,
        PrintScreen = 62,
        Pause = 63,
        NumLock = 64,
        Clear = 65,
        Sleep = 66,
        Keypad0 = 67,
        Keypad1 = 68,
        Keypad2 = 69,
        Keypad3 = 70,
        Keypad4 = 71,
        Keypad5 = 72,
        Keypad6 = 73,
        Keypad7 = 74,
        Keypad8 = 75,
        Keypad9 = 76,
        KeypadDivide = 77,
        KeypadMultiply = 78,
        KeypadMinus = 79,
        KeypadSubtract = 79,
        KeypadAdd = 80,
        KeypadPlus = 80,
        KeypadDecimal = 81,
        KeypadEnter = 82,
        A = 83,
        B = 84,
        C = 85,
        D = 86,
        E = 87,
        F = 88,
        G = 89,
        H = 90,
        I = 91,
        J = 92,
        K = 93,
        L = 94,
        M = 95,
        N = 96,
        O = 97,
        P = 98,
        Q = 99,
        R = 100,
        S = 101,
        T = 102,
        U = 103,
        V = 104,
        W = 105,
        X = 106,
        Y = 107,
        Z = 108,
        Number0 = 109,
        Number1 = 110,
        Number2 = 111,
        Number3 = 112,
        Number4 = 113,
        Number5 = 114,
        Number6 = 115,
        Number7 = 116,
        Number8 = 117,
        Number9 = 118,
        Tilde = 119,
        Minus = 120,
        Plus = 121,
        LBracket = 122,
        BracketLeft = 122,
        BracketRight = 123,
        RBracket = 123,
        Semicolon = 124,
        Quote = 125,
        Comma = 126,
        Period = 127,
        Slash = 128,
        BackSlash = 129,
        LastKey = 130,
    }

    public enum MouseButton
    {
        Left = 0,
        Middle = 1,
        Right = 2,
        Button1 = 3,
        Button2 = 4,
        Button3 = 5,
        Button4 = 6,
        Button5 = 7,
        Button6 = 8,
        Button7 = 9,
        Button8 = 10,
        Button9 = 11,
        LastButton = 12,
    }


}
