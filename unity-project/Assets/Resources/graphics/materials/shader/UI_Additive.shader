Shader "Noble Muffins/UI Additive" {
Properties {
_MainTex ("Texture", 2D) = "white" { }
 _StencilComp ("Stencil Comparison", Float) = 8
 _Stencil ("Stencil ID", Float) = 0
 _StencilOp ("Stencil Operation", Float) = 0
 _StencilWriteMask ("Stencil Write Mask", Float) = 255
 _StencilReadMask ("Stencil Read Mask", Float) = 255
 _ColorMask ("Color Mask", Float) = 15
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha One
    Stencil
    {
        Ref [_Stencil]
        Comp [_StencilComp]
        Pass [_StencilOp]
        ReadMask [_StencilReadMask]
        WriteMask [_StencilWriteMask]
    }
    ColorMask [_ColorMask]
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #pragma multi_compile _ PIXELSNAP_ON
    #include "UnityCG.cginc"

    sampler2D _MainTex;

    #ifdef ETC1_EXTERNAL_ALPHA
        sampler2D _AlphaTex;
    #endif

    struct appdata_t
    {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    fixed4 _Color;

    v2f vert (appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
        OUT.texcoord = IN.texcoord;
        OUT.color = IN.color;
        #ifdef PIXELSNAP_ON
        OUT.vertex = UnityPixelSnap (OUT.vertex);
        #endif
        return OUT;
    }

    fixed4 SampleMain (float2 uv)
    {
        fixed4 c = tex2D (_MainTex, uv);
        #ifdef ETC1_EXTERNAL_ALPHA
        c.a = tex2D (_AlphaTex, uv).r;
        #endif
        return c;
    }

    fixed4 frag (v2f IN) : SV_Target
    {
        fixed4 c = SampleMain (IN.texcoord) * IN.color;

        return c;
    }
    ENDCG
}
}
