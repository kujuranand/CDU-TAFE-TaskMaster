Shader "Cybernetic Walrus/Animated_Light_Cookie"
{
    Properties
    {
        _Tex("Primary Noise", 2D) = "white" {}
        _Tex2("Secondary Noise", 2D) = "white" {}
        _Range("Primary Cutoff", Range(0,1)) = 0.1
        _Range2("Secondary Cutoff", Range(0,1)) = 0.5
        _DistortStrength("Distort Strength", Range(0,1)) = 0.1
        _Smoothness("Cutoff Smoothness", Range(0,1)) = 0.1
        _SecondLayerStrength("Secondary Layer Strength", Range(0,1)) = 0.5
        _Speed("Speed", Range(-1,1)) = 0.2
        _Opacity("Opacity", Range(0,1)) = 0.2
        //_Rotation("Rotation", Float) = 0
        [Toggle(INVERT)] _INVERT("Invert", Float) = 0
    }

        SubShader
        {
            Lighting Off
            Blend One Zero

            Pass
            {
                CGPROGRAM
                #include "UnityCustomRenderTexture.cginc"

                #pragma vertex CustomRenderTextureVertexShader
                #pragma fragment frag
                #pragma target 3.0
                #pragma shader_feature MOVE
                #pragma shader_feature INVERT

                sampler2D _Tex, _Tex2;
                float _Range, _Smoothness, _Speed, _Range2, _Opacity, _DistortStrength, _SecondLayerStrength; //_Rotation;

                float4 frag(v2f_customrendertexture IN) : COLOR
                {
                    float time = fmod(_Time.x * _Speed * 5, 10);

                    //IN.globalTexcoord.xy -= 0.5;
                    //float s = -sin(radians(_Rotation));
                    //float c = cos(radians(_Rotation));
                    //float2x2 rotationMatrix = float2x2(c, -s, s, c);
                    //rotationMatrix *= 0.5;
                    //rotationMatrix += 0.5;
                    //rotationMatrix = rotationMatrix * 2 - 1;
                    //IN.globalTexcoord.xy = mul(IN.globalTexcoord.xy, rotationMatrix);
                    //IN.globalTexcoord.xy += 0.5;

                    float primNoise = tex2D(_Tex, (IN.globalTexcoord.xy * 1) + (time * 0.2)).r;
                    float secNoise = tex2D(_Tex2, (IN.globalTexcoord.xy * 2) + time + (primNoise * _DistortStrength));
                    float4 detailTex = tex2D(_Tex2, (IN.globalTexcoord.xy * 10)) + primNoise + secNoise;

                    float clouds = (primNoise * secNoise) * 0.5;
                    float4 layer2 = tex2D(_Tex2, (IN.globalTexcoord.xy) + (time * 0.1) + (primNoise * _DistortStrength));
                    clouds = smoothstep(_Range, _Range + _Smoothness, clouds * detailTex);
                    float secondClouds = smoothstep(_Range2, _Range2 + _Smoothness, layer2.r * detailTex) * _SecondLayerStrength;
                    clouds = (clouds + secondClouds);
                    clouds = saturate(clouds);

    #if INVERT
                    clouds = 1 - clouds;
    #endif

                    //clouds *= _Opacity;
                    _Opacity = 1 - _Opacity;
                    clouds += _Opacity;
                    clouds = saturate(clouds);

                    return clouds;
                }
                ENDCG
                }
        }
}