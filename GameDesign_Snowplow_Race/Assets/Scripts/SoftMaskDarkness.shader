Shader "Custom/SoftMaskDarkness"
{
    Properties
    {
        _MainTex ("Darkness Texture", 2D) = "black" {} // Full-screen black overlay
        _MaskTex ("Headlight Mask", 2D) = "white" {} // Soft gradient headlight mask
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // Sample darkness texture
                fixed4 mask = tex2D(_MaskTex, i.uv); // Sample headlight mask texture

                // Subtract headlight mask smoothly from darkness
                col.rgb *= (1.0 - mask.a); 
                col.a = 1.0 - mask.a; // Ensure transparency works

                return col;
            }
            ENDCG
        }
    }
}
