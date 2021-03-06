using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

namespace Utils
{
    public class AnimationRecorder : MonoBehaviour
    {
#if UNITY_EDITOR
        public AnimationClip clip;
        private GameObjectRecorder _recorder;
        
        // Start is called before the first frame update
        void Start()
        {
            _recorder = new GameObjectRecorder(gameObject);
            _recorder.BindComponentsOfType<Transform>(gameObject, true);
        }
        
        void LateUpdate()
        {
            if (clip == null)
                return;
            
            _recorder.TakeSnapshot(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (clip == null)
                return;

            if (_recorder.isRecording)
            {
                _recorder.SaveToClip(clip);
            }
        }
#endif
    }   
}
