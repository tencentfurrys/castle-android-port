Shader "Shader Forge/Additive" {
Properties {
_HSV ("HSV", Color) = (1,0.5,1,1)
 _RGB ("RGB", Color) = (1,1,1,1)
 _Texture ("Texture", 2D) = "bump" { }
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha One
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
