Shader "Verdant/Atmosphere" {
Properties { _Tint("Tint",Color)=(.2,.6,1,1) }
SubShader {Tags {"Queue"="Transparent" "RenderType"="Transparent"} Blend SrcAlpha One ZWrite Off Cull Front Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct v2f {float4 pos:SV_POSITION;float3 normal:TEXCOORD0;float3 view:TEXCOORD1;};fixed4 _Tint;
v2f vert(float4 vertex:POSITION,float3 normal:NORMAL){v2f o;o.pos=UnityObjectToClipPos(vertex);float3 world=mul(unity_ObjectToWorld,vertex).xyz;o.normal=UnityObjectToWorldNormal(normal);o.view=_WorldSpaceCameraPos-world;return o;}
fixed4 frag(v2f i):SV_Target {float rim=pow(1-abs(dot(normalize(i.normal),normalize(i.view))),3);return fixed4(_Tint.rgb,rim*.32*_Tint.a);}
ENDCG
} } }
