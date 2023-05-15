using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine;

namespace YgomSystem.UI
{
    // Token: 0x02000406 RID: 1030
    //[Token(Token = "0x2000406")]
    //[Attribute(Name = "AddComponentMenu", RVA = "0x65820", Offset = "0x64C20")]
    public class TweenPosition : Tween
    {
        // Token: 0x06001A3C RID: 6716 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A3C")]
        //[Address(RVA = "0xC9E250", Offset = "0xC9D250", VA = "0x180C9E250", Slot = "5")]
        protected override void CaptureFrom()
        {
        }

        // Token: 0x06001A3D RID: 6717 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A3D")]
        //[Address(RVA = "0xC9E2B0", Offset = "0xC9D2B0", VA = "0x180C9E2B0", Slot = "6")]
        protected override void OnSetValue(float par)
        {
        }

        // Token: 0x06001A3E RID: 6718 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A3E")]
        //[Address(RVA = "0xC9E4B0", Offset = "0xC9D4B0", VA = "0x180C9E4B0")]
        public TweenPosition()
        {
        }

        // Token: 0x0400130D RID: 4877
        //[Token(Token = "0x400130D")]
        //[FieldOffset(Offset = "0x68")]
        private RectTransform rtrans;

        // Token: 0x0400130E RID: 4878
        //[Token(Token = "0x400130E")]
        //[FieldOffset(Offset = "0x70")]
        //[SerializeField]
        public Vector3 from;

        // Token: 0x0400130F RID: 4879
        //[Token(Token = "0x400130F")]
        //[FieldOffset(Offset = "0x7C")]
        //[SerializeField]
        public Vector3 to;
    }
}
