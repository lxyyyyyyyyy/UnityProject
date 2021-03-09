Shader "Custom/OppositeFrame"
{
    Properties {
        _ServerScore("ServerScore", Float) = 0
        _ClientScore("ClientScore", Float) = 0
		_MainTex("CameraScene", 2D) = "White" {}
    }

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
 
		Pass
	    {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            float _ServerScore, _ClientScore;
			sampler2D _MainTex;  

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
        
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
        
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
        
        
            fixed4 frag(v2f i) : SV_Target
            {
				float delta = 0.006;
				float4 color;
				if (_ServerScore <= _ClientScore && (i.uv.x < delta || i.uv.x > 1 - delta || i.uv.y < delta || i.uv.y > 1 - delta)) {
					color = float4(1.0, 0.0, 0.0, 1.0);
				} else {
					color = tex2D(_MainTex, i.uv);
				} 
                return color;
            }
            ENDCG
        }
	}
}