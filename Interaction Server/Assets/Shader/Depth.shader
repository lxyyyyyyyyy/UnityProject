Shader "Custom/Depth" 
{  
	SubShader {  
		
		Pass{  
			CGPROGRAM  
			#pragma vertex vert  
			#pragma fragment frag  
			#include "UnityCG.cginc"  
 
			sampler2D _CameraDepthTexture;  
			struct v2f 
			{  
			   float4 pos : SV_POSITION;  
			   float4 scrPos:TEXCOORD1;  
			};  
 
			v2f vert (appdata_base v)
			{  
			   v2f f;  		  
			   f.pos = UnityObjectToClipPos (v.vertex);  
			   f.scrPos=ComputeScreenPos(f.pos);  
			   return f;  
			}  
			  
			half4 frag (v2f f) : COLOR
			{  
			   float depthValue =Linear01Depth (tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(f.scrPos)).r);
			   return half4(depthValue,depthValue,depthValue,1);  
			}  
			ENDCG  
		}  
	}  
	FallBack "Diffuse"  
} 