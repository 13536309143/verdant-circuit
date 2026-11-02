Shader "Verdant/PlanetSurface" {
Properties { _Tint("Tint", Color)=(1,1,1,1) _Locked("Locked",Range(0,1))=0 _Clouds("Clouds",Range(0,1))=0 }
SubShader { Tags {"RenderType"="Opaque"} Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_instancing
#include "UnityCG.cginc"
struct appdata {float4 vertex:POSITION;float3 normal:NORMAL;fixed4 color:COLOR;UNITY_VERTEX_INPUT_INSTANCE_ID};
struct v2f {float4 pos:SV_POSITION;fixed4 color:COLOR;float3 normal:TEXCOORD0;float3 local:TEXCOORD1;};fixed4 _Tint;float _Locked,_Clouds;
v2f vert(appdata v){UNITY_SETUP_INSTANCE_ID(v);v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.normal=UnityObjectToWorldNormal(v.normal);o.local=normalize(v.vertex.xyz);o.color=v.color*_Tint;return o;}
fixed4 frag(v2f i):SV_Target {
float light=.24+.76*saturate(dot(normalize(i.normal),normalize(float3(-.5,.7,-.8))));
float cloud=saturate((sin(i.local.x*17+_Time.y*.035)+sin(i.local.z*21+i.local.y*13)+sin(i.local.y*29-i.local.x*9)-1.3)*.8)*_Clouds;
float3 color=lerp(i.color.rgb,float3(.85,.92,1),cloud);float grey=dot(color,float3(.299,.587,.114));color=lerp(color,grey*.65+.08,_Locked);
return fixed4(color*light,1);}
ENDCG
} } }
