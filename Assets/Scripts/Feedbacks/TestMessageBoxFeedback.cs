using System;
using DevLib.BattleSystem.Feedback;
using Test;

namespace Feedbacks
{
    public class TestMessageBoxFeedback : AbstractFeedback
    {
        public override void PlayFeedback()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

            WindowsNative.MessageBox(
                IntPtr.Zero,
                "Unity에서 WinAPI 호출 성공",
                "테스트",
                0);

#endif
        }
    }
}