Shader "Sprites/Diffuse" {
Properties {
[PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
 _Color ("Tint", Color) = (1,1,1,1)
[MaterialToggle]  PixelSnap ("Pixel snap", Float) = 0
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    CGPROGRAM
    #pragma surface surf Lambert alpha
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #pragma multi_compile _ PIXELSNAP_ON
    sampler2D _MainTex;
    fixed4 _Color;
    #ifdef ETC1_EXTERNAL_ALPHA
        sampler2D _AlphaTex;
    #endif
    struct Input
    {
        float2 uv_MainTex;
        fixed4 color : COLOR;
    };
    void surf (Input IN, inout SurfaceOutput o)
    {
        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * IN.color * _Color;
        #ifdef ETC1_EXTERNAL_ALPHA
        c.a = tex2D (_AlphaTex, IN.uv_MainTex).r;
        #endif
        o.Albedo = c.rgb;
        o.Alpha = c.a;
    }
    ENDCG
}
}
