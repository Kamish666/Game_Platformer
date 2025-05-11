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
            // Включаем смешивание альфа-канала
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Объявление переменных для текстуры и цвета
            sampler2D _MainTex; // Текстура
            float4 _MainTex_ST; // Матрица трансформации для текстуры
            float4 _Color;      // Цвет, включая альфа-канал

            // Структуры для вершин и фрагментов
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

            // Вершинный шейдер
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color; // Передаем цвет с учетом альфа-канала
                return o;
            }

            // Фрагментный шейдер
            half4 frag(v2f i) : SV_Target
            {
                // Получаем цвет из текстуры
                half4 texColor = tex2D(_MainTex, i.uv);

                // Возвращаем цвет с учетом альфа-канала и прозрачности
                return texColor * i.color;
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}