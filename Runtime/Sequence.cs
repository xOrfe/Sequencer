using System;
using System.Collections.Generic;
using UnityEngine;

namespace XO.Sequencer.Runtime
{
    public delegate void MethodDelegate();

    public class Sequence
    {
        public bool IsPlaying { get; private set; }

        public MethodDelegate OnStart = null;
        public MethodDelegate OnUpdate = null;
        public MethodDelegate OnComplete = null;

        private readonly List<SequenceTick> _ticks = new();
        private int _currentTickIndex = 0;
        private bool _onStartInvoked = false;
        private float _tickStartTime;

        public Sequence()
        {
            Sequencer.Instance.InjectSequence(this);
        }

        public void Play()
        {
            if (_ticks.Count == 0)
            {
                Debug.LogWarning("No sequences injected.");
                return;
            }

            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Kill()
        {
            Clear();
        }

        public void Complete()
        {
            do
            {
                HandleSequenceTick(true);
            } while (IsPlaying);
        }

        private void OnCompleted()
        {
            OnComplete?.Invoke();
            Clear();
        }

        private void Clear()
        {
            Sequencer.Instance.DetachSequence(this);
            IsPlaying = false;

            OnStart = null;
            OnUpdate = null;
            OnComplete = null;

            _ticks.Clear();
            _currentTickIndex = 0;
            _onStartInvoked = false;
        }

        public SequenceTick Append()
        {
            var tick = new SequenceTick();
            _ticks.Add(tick);
            return tick;
        }

        public SequenceTick Join()
        {
            var tick = new SequenceTick();
            if (_ticks.Count > 0) _ticks[^1].MarkNextTickIsJoin();
            _ticks.Add(tick);
            return tick;
        }

        public void HandleSequenceTick(bool skipWaits = false)
        {
            if (!_onStartInvoked)
            {
                _tickStartTime = Time.time;
                OnStart?.Invoke();
                _onStartInvoked = true;
            }
            else OnUpdate?.Invoke();

            var tick = _ticks[_currentTickIndex];
            switch (tick.TickType)
            {
                case TickType.None:
                    MoveToNextTick();
                    break;
                case TickType.Wait:
                    if (skipWaits) MoveToNextTick();
                    else HandleWaitTick(tick);
                    break;
                case TickType.CallMethod:
                    HandleCallMethodTick(tick);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleWaitTick(SequenceTick tick)
        {
            if (_tickStartTime + tick.WaitTime < Time.time)
                MoveToNextTick();
        }

        private void HandleCallMethodTick(SequenceTick tick)
        {
            tick.MethodToCall?.Invoke();
            MoveToNextTick();
        }

        private void MoveToNextTick()
        {
            _tickStartTime = Time.time;
            _currentTickIndex++;
            if (_currentTickIndex >= _ticks.Count)
            {
                OnCompleted();
            }
        }
    }
}