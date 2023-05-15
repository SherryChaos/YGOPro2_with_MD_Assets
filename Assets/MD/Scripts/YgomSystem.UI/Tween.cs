using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace YgomSystem.UI
{
    // Token: 0x020003FA RID: 1018
    ////[Token(Token = "0x20003FA")]
    public abstract class Tween : MonoBehaviour
    {
        // Token: 0x060019F7 RID: 6647 RVA: 0x0000AC08 File Offset: 0x00008E08
        ////[Token(Token = "0x60019F7")]
        ////[Address(RVA = "0x5D3C30", Offset = "0x5D2C30", VA = "0x1805D3C30")]
        private static float BounceOut(float k)
        {
            return 0f;
        }

        // Token: 0x060019F8 RID: 6648 RVA: 0x0000AC20 File Offset: 0x00008E20
        ////[Token(Token = "0x60019F8")]
        ////[Address(RVA = "0x5D3B00", Offset = "0x5D2B00", VA = "0x1805D3B00")]
        private static float BounceIn(float k)
        {
            return 0f;
        }

        // Token: 0x060019F9 RID: 6649 RVA: 0x0000AC38 File Offset: 0x00008E38
        ////[Token(Token = "0x60019F9")]
        ////[Address(RVA = "0x5D3D30", Offset = "0x5D2D30", VA = "0x1805D3D30")]
        public static float EasingValue(float k, Tween.Easing e)
        {
            return 0f;
        }

        // Token: 0x060019FA RID: 6650 RVA: 0x0000AC50 File Offset: 0x00008E50
        ////[Token(Token = "0x60019FA")]
        ////[Address(RVA = "0x5D4500", Offset = "0x5D3500", VA = "0x1805D4500")]
        private float GetEasing(float k)
        {
            return 0f;
        }

        // Token: 0x060019FB RID: 6651 RVA: 0x00002050 File Offset: 0x00000250
        ////[Token(Token = "0x60019FB")]
        ////[Address(RVA = "0x2A43D0", Offset = "0x2A33D0", VA = "0x1802A43D0", Slot = "4")]
        protected virtual void CaptureAwake()
        {
        }

        // Token: 0x060019FC RID: 6652 RVA: 0x00002050 File Offset: 0x00000250
        ////[Token(Token = "0x60019FC")]
        ////[Address(RVA = "0x2A43D0", Offset = "0x2A33D0", VA = "0x1802A43D0", Slot = "5")]
        protected virtual void CaptureFrom()
        {
        }

        // Token: 0x060019FD RID: 6653
        ////[Token(Token = "0x60019FD")]
        protected abstract void OnSetValue(float par);

        // Token: 0x060019FE RID: 6654 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x60019FE")]
        //[Address(RVA = "0x445160", Offset = "0x444160", VA = "0x180445160")]
        private void Awake()
        {
        }

        // Token: 0x060019FF RID: 6655 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x60019FF")]
        //[Address(RVA = "0x5D44A0", Offset = "0x5D34A0", VA = "0x1805D44A0")]
        private void ExecSetup()
        {
        }

        // Token: 0x06001A00 RID: 6656 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A00")]
        //[Address(RVA = "0x5D4210", Offset = "0x5D3210", VA = "0x1805D4210")]
        private void ExecPlay(float time, bool forceUpdate = false)
        {
        }

        // Token: 0x06001A01 RID: 6657 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A01")]
        //[Address(RVA = "0x5D4FC0", Offset = "0x5D3FC0", VA = "0x1805D4FC0")]
        private void OnDestroy()
        {
        }

        // Token: 0x06001A02 RID: 6658 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A02")]
        //[Address(RVA = "0x5D44A0", Offset = "0x5D34A0", VA = "0x1805D44A0")]
        private void Start()
        {
        }

        // Token: 0x06001A03 RID: 6659 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A03")]
        //[Address(RVA = "0x5D6A20", Offset = "0x5D5A20", VA = "0x1805D6A20")]
        private void Update()
        {
        }

        // Token: 0x06001A04 RID: 6660 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A04")]
        //[Address(RVA = "0x5D5120", Offset = "0x5D4120", VA = "0x1805D5120")]
        public void Play()
        {
        }

        // Token: 0x06001A05 RID: 6661 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A05")]
        //[Address(RVA = "0x5D5000", Offset = "0x5D4000", VA = "0x1805D5000")]
        public void Pause()
        {
        }

        // Token: 0x06001A06 RID: 6662 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A06")]
        //[Address(RVA = "0x5D5220", Offset = "0x5D4220", VA = "0x1805D5220")]
        public void Stop()
        {
        }

        // Token: 0x06001A07 RID: 6663 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A07")]
        //[Address(RVA = "0x5D4150", Offset = "0x5D3150", VA = "0x1805D4150")]
        public void End()
        {
        }

        // Token: 0x06001A08 RID: 6664 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A08")]
        //[Address(RVA = "0x5D51E0", Offset = "0x5D41E0", VA = "0x1805D51E0")]
        public void Reset()
        {
        }

        // Token: 0x06001A09 RID: 6665 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A09")]
        //[Address(RVA = "0x5D51A0", Offset = "0x5D41A0", VA = "0x1805D51A0")]
        public void ResetWithTimeDelta()
        {
        }

        // Token: 0x06001A0A RID: 6666 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A0A")]
        //[Address(RVA = "0x5D4D90", Offset = "0x5D3D90", VA = "0x1805D4D90")]
        public void GotoAndPlay(float time)
        {
        }

        // Token: 0x06001A0B RID: 6667 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A0B")]
        //[Address(RVA = "0x5D4D50", Offset = "0x5D3D50", VA = "0x1805D4D50")]
        public void GotoAndPause(float time)
        {
        }

        // Token: 0x06001A0C RID: 6668 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A0C")]
        //[Address(RVA = "0x5D3CD0", Offset = "0x5D2CD0", VA = "0x1805D3CD0")]
        public void DestroySelf()
        {
        }

        // Token: 0x06001A0D RID: 6669 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A0D")]
        //[Address(RVA = "0x5D5010", Offset = "0x5D4010", VA = "0x1805D5010")]
        public void PlayLabel(string _label)
        {
        }

        // Token: 0x06001A0E RID: 6670 RVA: 0x0000AC68 File Offset: 0x00008E68
        //[Token(Token = "0x6001A0E")]
        //[Address(RVA = "0x5D4E50", Offset = "0x5D3E50", VA = "0x1805D4E50")]
        public bool IsPlaying(string _label = "", bool isActive = false)
        {
            return default(bool);
        }

        // Token: 0x06001A0F RID: 6671 RVA: 0x0000AC80 File Offset: 0x00008E80
        //[Token(Token = "0x6001A0F")]
        //[Address(RVA = "0x5D4DD0", Offset = "0x5D3DD0", VA = "0x1805D4DD0")]
        public bool IsFinished()
        {
            return default(bool);
        }

        // Token: 0x06001A10 RID: 6672 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A10")]
        //[Address(RVA = "0x5D6490", Offset = "0x5D5490", VA = "0x1805D6490")]
        public static void TargetPlayLabel(GameObject target, string _label = "", bool includeChildren = false, bool wakeup = false)
        {
        }

        // Token: 0x06001A11 RID: 6673 RVA: 0x0000AC98 File Offset: 0x00008E98
        //[Token(Token = "0x6001A11")]
        //[Address(RVA = "0x5D5FE0", Offset = "0x5D4FE0", VA = "0x1805D5FE0")]
        public static bool TargetIsPlaying(GameObject target, string _label = "", bool includeChildren = false, bool isActive = false)
        {
            return default(bool);
        }

        // Token: 0x06001A12 RID: 6674 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A12")]
        //[Address(RVA = "0x5D5C90", Offset = "0x5D4C90", VA = "0x1805D5C90")]
        public static void TargetGotoAndPlayLabel(GameObject target, float time, string _label = "", bool includeChildren = false, bool wakeup = false)
        {
        }

        // Token: 0x06001A13 RID: 6675 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A13")]
        //[Address(RVA = "0x5D5940", Offset = "0x5D4940", VA = "0x1805D5940")]
        public static void TargetGotoAndPauseLabel(GameObject target, float time, string _label = "", bool includeChildren = false, bool wakeup = false)
        {
        }

        // Token: 0x06001A14 RID: 6676 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A14")]
        //[Address(RVA = "0x5D6270", Offset = "0x5D5270", VA = "0x1805D6270")]
        public static void TargetPauseLabel(GameObject target, string _label = "", bool includeChildren = false)
        {
        }

        // Token: 0x06001A15 RID: 6677 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A15")]
        //[Address(RVA = "0x5D67D0", Offset = "0x5D57D0", VA = "0x1805D67D0")]
        public static void TargetStopLabel(GameObject target, string _label = "", bool includeChildren = false, string exlabel = "")
        {
        }

        // Token: 0x06001A16 RID: 6678 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A16")]
        //[Address(RVA = "0x5D54D0", Offset = "0x5D44D0", VA = "0x1805D54D0")]
        public static void TargetEndLabel(GameObject target, string _label = "", bool includeChildren = false)
        {
        }

        // Token: 0x06001A17 RID: 6679 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A17")]
        //[Address(RVA = "0x5D5700", Offset = "0x5D4700", VA = "0x1805D5700")]
        public static void TargetForwardLabel(GameObject target, float sec, string _label = "", bool includeChildren = false)
        {
        }

        // Token: 0x06001A18 RID: 6680 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A18")]
        //[Address(RVA = "0x5D5280", Offset = "0x5D4280", VA = "0x1805D5280")]
        public static void TargetCaptureFrom(GameObject target, string _label = "", bool includeChildren = false, bool force = false)
        {
        }

        // Token: 0x06001A19 RID: 6681 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A19")]
        //[Address(RVA = "0x5D5240", Offset = "0x5D4240", VA = "0x1805D5240")]
        public static void TargetCaptureFrom(Tween tween, bool force)
        {
        }

        // Token: 0x06001A1A RID: 6682 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A1A")]
        //[Address(RVA = "0x5D3830", Offset = "0x5D2830", VA = "0x1805D3830")]
        public static void AllPlayLabel(string label)
        {
        }

        // Token: 0x06001A1B RID: 6683 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A1B")]
        //[Address(RVA = "0x5D3990", Offset = "0x5D2990", VA = "0x1805D3990")]
        public static void AllStopLabel(string label)
        {
        }

        // Token: 0x06001A1C RID: 6684 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A1C")]
        //[Address(RVA = "0x5D36D0", Offset = "0x5D26D0", VA = "0x1805D36D0")]
        public static void AllPauseLabel(string label)
        {
        }

        // Token: 0x06001A1D RID: 6685 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A1D")]
        //[Address(RVA = "0x5D3570", Offset = "0x5D2570", VA = "0x1805D3570")]
        public static void AllEndLabel(string label)
        {
        }

        // Token: 0x06001A1E RID: 6686 RVA: 0x00002052 File Offset: 0x00000252
        //[Token(Token = "0x6001A1E")]
        //[Address(RVA = "0x5D4AD0", Offset = "0x5D3AD0", VA = "0x1805D4AD0")]
        public static List<Tween> GetTweenTarget(GameObject target, string _label = "", bool includeChildren = false)
        {
            return null;
        }

        // Token: 0x06001A1F RID: 6687 RVA: 0x00002052 File Offset: 0x00000252
        //[Token(Token = "0x6001A1F")]
        //[Address(RVA = "0x5D4920", Offset = "0x5D3920", VA = "0x1805D4920")]
        public static List<Tween> GetTweenAll(string label)
        {
            return null;
        }

        // Token: 0x06001A20 RID: 6688 RVA: 0x00002050 File Offset: 0x00000250
        //[Token(Token = "0x6001A20")]
        //[Address(RVA = "0x5D6B80", Offset = "0x5D5B80", VA = "0x1805D6B80")]
        protected Tween()
        {
        }

        // Token: 0x040012C3 RID: 4803
        //[Token(Token = "0x40012C3")]
        //[FieldOffset(Offset = "0x0")]
        private static readonly float FRAMERATE_LIMIT;

        // Token: 0x040012C4 RID: 4804
        //[Token(Token = "0x40012C4")]
        //[FieldOffset(Offset = "0x18")]
        public string label;

        // Token: 0x040012C5 RID: 4805
        //[Token(Token = "0x40012C5")]
        //[FieldOffset(Offset = "0x20")]
        //[SerializeField]
        public Tween.Easing easing;

        // Token: 0x040012C6 RID: 4806
        //[Token(Token = "0x40012C6")]
        //[FieldOffset(Offset = "0x24")]
        //[SerializeField]
        public Tween.Style style;

        // Token: 0x040012C7 RID: 4807
        //[Token(Token = "0x40012C7")]
        //[FieldOffset(Offset = "0x28")]
        //[SecField]
        //[SerializeField]
        public float duration;

        // Token: 0x040012C8 RID: 4808
        //[Token(Token = "0x40012C8")]
        //[FieldOffset(Offset = "0x2C")]
        //[SecField]
        //[SerializeField]
        public float setupWait;

        // Token: 0x040012C9 RID: 4809
        //[Token(Token = "0x40012C9")]
        //[FieldOffset(Offset = "0x30")]
        //[SerializeField]
        //[SecField]
        public float startDelay;

        // Token: 0x040012CA RID: 4810
        //[Token(Token = "0x40012CA")]
        //[FieldOffset(Offset = "0x34")]
        //[SerializeField]
        public bool ignoreTimeScale;

        // Token: 0x040012CB RID: 4811
        //[Token(Token = "0x40012CB")]
        //[FieldOffset(Offset = "0x38")]
        //[SerializeField]
        public UnityEvent onFinished;

        // Token: 0x040012CC RID: 4812
        //[Token(Token = "0x40012CC")]
        //[FieldOffset(Offset = "0x40")]
        //[SerializeField]
        public bool callOnFinishedDestroy;

        // Token: 0x040012CD RID: 4813
        //[Token(Token = "0x40012CD")]
        //[FieldOffset(Offset = "0x48")]
        //[HideInInspector]
        public AnimationCurve curve;

        // Token: 0x040012CE RID: 4814
        //[Token(Token = "0x40012CE")]
        //[FieldOffset(Offset = "0x50")]
        protected float timeDelta;

        // Token: 0x040012CF RID: 4815
        //[Token(Token = "0x40012CF")]
        //[FieldOffset(Offset = "0x54")]
        protected float crntTime;

        // Token: 0x040012D0 RID: 4816
        //[Token(Token = "0x40012D0")]
        //[FieldOffset(Offset = "0x58")]
        private bool isCaptured;

        // Token: 0x040012D1 RID: 4817
        //[Token(Token = "0x40012D1")]
        //[FieldOffset(Offset = "0x5C")]
        private float setupWaitCount;

        // Token: 0x040012D2 RID: 4818
        //[Token(Token = "0x40012D2")]
        //[FieldOffset(Offset = "0x60")]
        private bool isExecFinished;

        // Token: 0x020003FB RID: 1019
        //[Token(Token = "0x20003FB")]
        public enum Easing
        {
            // Token: 0x040012D4 RID: 4820
            //[Token(Token = "0x40012D4")]
            Linear,
            // Token: 0x040012D5 RID: 4821
            //[Token(Token = "0x40012D5")]
            CubicIn,
            // Token: 0x040012D6 RID: 4822
            //[Token(Token = "0x40012D6")]
            CubicOut,
            // Token: 0x040012D7 RID: 4823
            //[Token(Token = "0x40012D7")]
            CubicInOut,
            // Token: 0x040012D8 RID: 4824
            //[Token(Token = "0x40012D8")]
            BackIn,
            // Token: 0x040012D9 RID: 4825
            //[Token(Token = "0x40012D9")]
            BackOut,
            // Token: 0x040012DA RID: 4826
            //[Token(Token = "0x40012DA")]
            BackInOut,
            // Token: 0x040012DB RID: 4827
            //[Token(Token = "0x40012DB")]
            BounceIn,
            // Token: 0x040012DC RID: 4828
            //[Token(Token = "0x40012DC")]
            BounceOut,
            // Token: 0x040012DD RID: 4829
            //[Token(Token = "0x40012DD")]
            BounceInOut,
            // Token: 0x040012DE RID: 4830
            //[Token(Token = "0x40012DE")]
            Customize,
            // Token: 0x040012DF RID: 4831
            //[Token(Token = "0x40012DF")]
            QuartIn,
            // Token: 0x040012E0 RID: 4832
            //[Token(Token = "0x40012E0")]
            QuartOut,
            // Token: 0x040012E1 RID: 4833
            //[Token(Token = "0x40012E1")]
            QuartInOut
        }

        // Token: 0x020003FC RID: 1020
        //[Token(Token = "0x20003FC")]
        public enum Style
        {
            // Token: 0x040012E3 RID: 4835
            //[Token(Token = "0x40012E3")]
            Once,
            // Token: 0x040012E4 RID: 4836
            //[Token(Token = "0x40012E4")]
            Loop,
            // Token: 0x040012E5 RID: 4837
            //[Token(Token = "0x40012E5")]
            PingPong,
            // Token: 0x040012E6 RID: 4838
            //[Token(Token = "0x40012E6")]
            PingPongLoop,
            // Token: 0x040012E7 RID: 4839
            //[Token(Token = "0x40012E7")]
            SyncLoop,
            // Token: 0x040012E8 RID: 4840
            //[Token(Token = "0x40012E8")]
            SyncPingPongLoop
        }
    }
}
