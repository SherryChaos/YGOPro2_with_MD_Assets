using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace YgomSystem.UI
{
    // Token: 0x020003FD RID: 1021
    //[Token(Token = "0x20003FD")]
    //[Attribute(Name = "AddComponentMenu", RVA = "0x64FA0", Offset = "0x643A0")]
    public class TweenAlpha : Tween
    {
        // Token: 0x06001A22 RID: 6690 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A22")]
        //[Address(RVA = "0xC9A400", Offset = "0xC99400", VA = "0x180C9A400", Slot = "5")]
        protected override void CaptureFrom()
        {
        }

        // Token: 0x06001A23 RID: 6691 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A23")]
        //[Address(RVA = "0xC9A750", Offset = "0xC99750", VA = "0x180C9A750", Slot = "6")]
        protected override void OnSetValue(float par)
        {
        }

        // Token: 0x06001A24 RID: 6692 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A24")]
        //[Address(RVA = "0xC9A9D0", Offset = "0xC999D0", VA = "0x180C9A9D0")]
        public TweenAlpha()
        {
        }

        // Token: 0x040012E9 RID: 4841
        //[Token(Token = "0x40012E9")]
        //[FieldOffset(Offset = "0x68")]
        //[SerializeField]
        //[Attribute(Name = "RangeAttribute", RVA = "0x64FD0", Offset = "0x643D0")]
        public float from;

        // Token: 0x040012EA RID: 4842
        //[Token(Token = "0x40012EA")]
        //[FieldOffset(Offset = "0x6C")]
        //[Attribute(Name = "RangeAttribute", RVA = "0x65130", Offset = "0x64530")]
        //[SerializeField]
        public float to;

        // Token: 0x040012EB RID: 4843
        //[Token(Token = "0x40012EB")]
        //[FieldOffset(Offset = "0x70")]
        public bool isRecusive;

        // Token: 0x040012EC RID: 4844
        //[Token(Token = "0x40012EC")]
        //[FieldOffset(Offset = "0x78")]
        private CanvasGroup canvasGroup;

        // Token: 0x040012ED RID: 4845
        //[Token(Token = "0x40012ED")]
        //[FieldOffset(Offset = "0x80")]
        private List<KeyValuePair<Graphic, Color>> childGraps;
    }
}
