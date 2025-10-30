Shader "StarFX/StarRipple_SineAuto"
{
    Properties
    {
        _Color ("Ripple Color", Color) = (1,1,1,1)
        _Alpha ("Overall Alpha", Range(0,1)) = 1.0
        _Frequency ("Wave Frequency", Range(1,100)) = 20.0
        _WaveSpeed ("Wave Speed", Range(0,100)) = 2.0
        _InvertDirection ("Invert Direction (0=Out,1=In)", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;
            float _Alpha;
            float _Frequency;
            float _WaveSpeed;
            float _InvertDirection; // 0 = �Ѥ��ӥ~, 1 = �ѥ~�Ӥ�

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // �ɶ����i�ۦ�
                float phase = _Time.y * _WaveSpeed;

                // �ھڤ�V�������t
                float dir = (_InvertDirection > 0.5) ? 1.0 : -1.0;

                // �N sin �M�g�� 0~1
                float wave = 0.5 + 0.5 * sin(dist * _Frequency + dir * phase);

                // �����H�i����z����
                fixed4 col = _Color;
                col.a = wave * _Alpha;

                return col;
            }
            ENDCG
        }
    }
}
