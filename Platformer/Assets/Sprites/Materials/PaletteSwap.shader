Shader "Custom/PaletteSwap"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Count ("Count", Float) = 1

        _FromColors0("FromColor0", Color) = (1,1,1,1)
        _ToColors0("ToColor0", Color) = (1,0,0,1)
        _FromColors1("FromColor1", Color) = (1,1,1,1)
        _ToColors1("ToColor1", Color) = (0,1,0,1)
        _FromColors2("FromColor2", Color) = (1,1,1,1)
        _ToColors2("ToColor2", Color) = (0,0,1,1)
        _FromColors3("FromColor3", Color) = (1,1,1,1)
        _ToColors3("ToColor3", Color) = (1,1,0,1)
        _FromColors4("FromColor4", Color) = (1,1,1,1)
        _ToColors4("ToColor4", Color) = (0,1,1,1)
        _FromColors5("FromColor5", Color) = (1,1,1,1)
        _ToColors5("ToColor5", Color) = (1,0,1,1)
        _FromColors6("FromColor6", Color) = (1,1,1,1)
        _ToColors6("ToColor6", Color) = (0.5,0.5,0.5,1)
        _FromColors7("FromColor7", Color) = (1,1,1,1)
        _ToColors7("ToColor7", Color) = (0.25,0.25,0.25,1)
        _FromColors8("FromColor8", Color) = (1,1,1,1)
        _ToColors8("ToColor8", Color) = (1,0.5,0,1)
        _FromColors9("FromColor9", Color) = (1,1,1,1)
        _ToColors9("ToColor9", Color) = (0,0.5,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _FromColors0, _ToColors0;
            float4 _FromColors1, _ToColors1;
            float4 _FromColors2, _ToColors2;
            float4 _FromColors3, _ToColors3;
            float4 _FromColors4, _ToColors4;
            float4 _FromColors5, _ToColors5;
            float4 _FromColors6, _ToColors6;
            float4 _FromColors7, _ToColors7;
            float4 _FromColors8, _ToColors8;
            float4 _FromColors9, _ToColors9;

            float _Count;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float4 fromColors[10] = {
                    _FromColors0, _FromColors1, _FromColors2, _FromColors3, _FromColors4,
                    _FromColors5, _FromColors6, _FromColors7, _FromColors8, _FromColors9
                };
                float4 toColors[10] = {
                    _ToColors0, _ToColors1, _ToColors2, _ToColors3, _ToColors4,
                    _ToColors5, _ToColors6, _ToColors7, _ToColors8, _ToColors9
                };

                int maxCount = min((int)_Count, 10);
                for (int j = 0; j < maxCount; j++)
                {
                    if (abs(col.r - fromColors[j].r) < 0.01 &&
                        abs(col.g - fromColors[j].g) < 0.01 &&
                        abs(col.b - fromColors[j].b) < 0.01)
                    {
                        col.rgb = toColors[j].rgb;
                        break;
                    }
                }

                return col;
            }

            ENDCG
        }
    }
}