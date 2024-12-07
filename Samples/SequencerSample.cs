using UnityEngine;
using XO.Sequencer.Runtime;

namespace XO.Sequencer.Samples
{
    public class SequencerSample : MonoBehaviour
    {
        private Sequence _sequence;

        private void Start()
        {
            PlaySample();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _sequence?.Complete();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                _sequence?.Kill();
            }
        }

        public void PlaySample()
        {
            _sequence = new Sequence();
            _sequence.OnStart += OnStart;
            _sequence.OnUpdate += OnUpdate;
            _sequence.OnComplete += OnComplete;

            _sequence.Append().CallMethod(Method1);
            _sequence.Append().Wait(1.0F);
            _sequence.Append().CallMethod(Method2);
            _sequence.Join().CallMethod(Method3);
            _sequence.Append().Wait(1000);

            _sequence.Play();
        }

        private void OnStart()
        {
            Debug.Log("Started!");
        }

        private void OnUpdate()
        {
            Debug.Log("Update!");
        }

        private void OnComplete()
        {
            Debug.Log("Completed!");
        }

        private void Method1()
        {
            Debug.Log("Method1!");
        }

        private void Method2()
        {
            Debug.Log("Method2!");
        }

        private void Method3()
        {
            Debug.Log("Method3!");
        }
    }
}