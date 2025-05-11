// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }

        Pass
        {
            // �������� ���������� �����-������
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ���������� ���������� ��� �������� � �����
            sampler2D _MainTex; // ��������
            float4 _MainTex_ST; // ������� ������������� ��� ��������
            float4 _Color;      // ����, ������� �����-�����

            // ��������� ��� ������ � ����������
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            // ��������� ������
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color; // �������� ���� � ������ �����-������
                return o;
            }

            // ����������� ������
            half4 frag(v2f i) : SV_Target
            {
                // �������� ���� �� ��������
                half4 texColor = tex2D(_MainTex, i.uv);

                // ���������� ���� � ������ �����-������ � ������������
                return texColor * i.color;
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}