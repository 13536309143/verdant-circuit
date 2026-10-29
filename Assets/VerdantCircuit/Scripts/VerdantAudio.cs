using UnityEngine;

namespace Verdant
{
    public sealed class VerdantAudio : MonoBehaviour
    {
        AudioSource ambience,effects;float volume;
        public void Initialize(float level)
        {
            ambience=gameObject.AddComponent<AudioSource>();effects=gameObject.AddComponent<AudioSource>();
            const int rate=16000;const int seconds=32;float[] samples=new float[rate*seconds];
            float[] notes={130.81f,164.81f,196f,146.83f,174.61f,220f,110f,130.81f,164.81f,98f,123.47f,146.83f};
            for(int n=0;n<samples.Length;n++)
            {
                float t=(float)n/rate;int chord=(int)(t/8)%4;float envelope=Mathf.Sin(Mathf.PI*(t%8)/8);float value=0;
                for(int k=0;k<3;k++){float hz=notes[chord*3+k];value+=Mathf.Sin(2*Mathf.PI*hz*t)*.025f+Mathf.Sin(2*Mathf.PI*(hz*2+.18f)*t)*.009f;}
                samples[n]=value*envelope;
            }
            var clip=AudioClip.Create("禾湾 · 四季和弦",samples.Length,1,rate,false);clip.SetData(samples,0);ambience.clip=clip;ambience.loop=true;SetVolume(level);ambience.Play();
        }
        public void SetVolume(float value){volume=Mathf.Clamp01(value);if(ambience!=null)ambience.volume=volume;if(effects!=null)effects.volume=volume*.28f;}
        public void PlayCue(float pitch)
        {
            if(effects==null||volume<=0)return;int rate=16000;float[] data=new float[1600];for(int i=0;i<data.Length;i++){float t=(float)i/rate;data[i]=Mathf.Sin(2*Mathf.PI*pitch*t)*Mathf.Exp(-t*42)*.25f;}
            var clip=AudioClip.Create("Farm Chime",data.Length,1,rate,false);clip.SetData(data,0);effects.PlayOneShot(clip);Destroy(clip,.3f);
        }
        void OnDestroy(){if(ambience!=null&&ambience.clip!=null)Destroy(ambience.clip);}
    }
}
