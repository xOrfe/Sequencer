using System.Collections.Generic;

namespace XO.Sequencer.Runtime
{
    public class Sequencer : SingletonUtil<Sequencer>
    {
        private readonly List<Sequence> _sequences = new List<Sequence>();

        private void Update()
        {
            HandleSequences();
        }

        public void InjectSequence(Sequence sequence)
        {
            _sequences.Add(sequence);
        }

        public void DetachSequence(Sequence sequence)
        {
            _sequences.Remove(sequence);
        }

        private void HandleSequences()
        {
            if (_sequences.Count <= 0) return;
            foreach (var sequence in _sequences.ToArray())
            {
                if (!sequence.IsPlaying) continue;
                sequence.HandleSequenceTick();
            }
        }
    }
}