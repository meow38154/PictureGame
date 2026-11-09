using Agents.Players;
using Command;
using CoreSystem.GrabSystem;
using DevLib.EventChannelSystem;
using UI.Wall_Paper.Files.SO;
using UI.Wall_Paper.Files.Windows;

namespace CoreSystem.Events
{
    public abstract class JEvent<T, V> : GameEvent where T : JEvent<T, V>
    {
        public V Value { get; private set; }

        public T InitData(V value)
        {
            Value = value;
            return (T)this;
        }
    }
    
    public static class CreateEvents
    {
        public static readonly BoolEvent BoolEvent = new();
        public static readonly FloatEvent FloatEvent = new();
        public static readonly StringEvent StringEvent = new();
        public static readonly ControlMovementEvent ControlMovementEvent = new();
        public static readonly GrabbableObjectEvent GrabbableObjectEvent = new();
        public static readonly AbstractFileSOEvent AbstractFileSOEvent = new();
        public static readonly AbstractWindowEvent AbstractWindowEvent = new();
        public static readonly CommandInvokeEvent CommandInvokeEvent = new();
    }

    public class BoolEvent : JEvent<BoolEvent, bool> { }
    public class FloatEvent : JEvent<FloatEvent, float> { }
    public class StringEvent : JEvent<StringEvent, string> { }

    public class ControlMovementEvent : JEvent<ControlMovementEvent, IControlMovement> { }

    public class AbstractFileSOEvent : JEvent<AbstractFileSOEvent, AbstractFileSO> { }
    
    public class AbstractWindowEvent : JEvent<AbstractWindowEvent, AbstractWindows> { }

    public class GrabbableObjectEvent : JEvent<GrabbableObjectEvent, IGrabbable> { }
    
    public class CommandInvokeEvent : JEvent<CommandInvokeEvent, CommandInvokeEventStruct> { }
}