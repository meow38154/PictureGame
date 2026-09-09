using DevLib.AnimatorSystem;
using UnityEngine;

namespace CoreSystem
{
    public interface ISceneTransition
    {
        Awaitable PlayAsync(HashDataSO hash);
    }
}