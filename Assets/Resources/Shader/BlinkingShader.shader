Shader "Custom/URP_BlinkingEmissionShader_Transparent"
{
    Properties
    {
        _EmissionColor ("Emission Color", Color) = (1,1,0,1) // Light Yellow for blinking
        _BlinkSpeed ("Blink Speed", Range(0.1, 5.0)) = 1.0
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0 // Alpha control for transparency
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _EmissionColor;
            float _BlinkSpeed;
            float _Alpha; // Alpha value for fading

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Blinking effect using sin for smooth transitions
                half blink = (sin(_Time.y * _BlinkSpeed) * 0.5 + 0.5); // Blinks between 0 and 1

                // Combine emission color with blinking and alpha transparency
                half4 emissionColor = _EmissionColor;
                emissionColor.a = _Alpha * blink;

                // Output only emission color with transparency to blend with the original material
                return half4(emissionColor.rgb, emissionColor.a);
            }

            ENDHLSL
        }
    }
}
