// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/ColorChange"
{
    Properties
    {
        _Color ("MainColor", color) = (1,1,1,1)
        _SpecColor ("SpecColor", color) = (1,1,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Boundary ("Boundary", Float) = 0.0
        _Gradient ("Gradient", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 world : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _SpecColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Boundary;
            float _Gradient;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.world = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float a = clamp(((i.world.z - _Boundary) / _Gradient), 0, 1);
                fixed4 albedo = col * (_Color * a + _SpecColor * (1 - a));
                UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(albedo);
            }
            ENDCG
        }
    }
}
