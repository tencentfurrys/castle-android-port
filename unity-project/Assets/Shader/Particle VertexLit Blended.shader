Shader "Particles/VertexLit Blended" {
Properties {
_EmisColor ("Emissive Color", Color) = (0.2,0.2,0.2,0)
 _MainTex ("Particle Texture", 2D) = "white" { }
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile _ PIXELSNAP_ON
    #include "UnityCG.cginc"

    sampler2D _MainTex;


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
        return c;
    }

    // CastlePort P8: single-variant (no split-alpha) for Adreno variant-selection safety
    fixed4 frag (v2f IN) : SV_Target
    {
        fixed4 c = SampleMain (IN.texcoord) * IN.color;

        return c;
    }
    ENDCG
}
}
