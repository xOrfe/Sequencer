namespace XO.Sequencer.Runtime
{
    public class SequenceTick
    {
        public TickType TickType { get; private set; } = TickType.None;
        public float WaitTime { get; private set; } = 0;
        public MethodDelegate MethodToCall { get; private set; } = null;
        public bool NextTickIsJoin { get; private set; } = false;

        public void MarkNextTickIsJoin()
        {
            NextTickIsJoin = true;
        }

        public void CallMethod(MethodDelegate methodToCall)
        {
            TickType = TickType.CallMethod;
            MethodToCall = methodToCall;
        }

        public void Wait(float time)
        {
            TickType = TickType.Wait;
            WaitTime = time;
        }
    }

    public enum TickType
    {
        None,
        Wait,
        CallMethod
    }
}