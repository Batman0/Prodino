// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33132,y:33273,varname:node_4013,prsc:2|normal-2552-RGB,emission-3735-OUT,custl-8293-OUT,olwid-987-OUT,olcol-9840-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:9202,x:32180,y:33625,varname:node_9202,prsc:2;n:type:ShaderForge.SFN_LightColor,id:1097,x:32180,y:33776,varname:node_1097,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8293,x:32544,y:33979,varname:node_8293,prsc:2|A-9202-OUT,B-1097-RGB,C-9370-OUT;n:type:ShaderForge.SFN_LightVector,id:1876,x:31223,y:34192,varname:node_1876,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7210,x:31223,y:34333,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:1208,x:31587,y:34311,varname:node_1208,prsc:2,dt:1|A-1876-OUT,B-7210-OUT;n:type:ShaderForge.SFN_Multiply,id:9741,x:31902,y:34282,varname:node_9741,prsc:2|A-1208-OUT,B-7129-OUT;n:type:ShaderForge.SFN_Tex2d,id:7924,x:30922,y:33714,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:_Albedo,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a2373dfa6eec8c34490181329893c7e5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2552,x:32736,y:32621,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5e451c9c14631774593360dd97becadc,ntxv:3,isnm:True;n:type:ShaderForge.SFN_HalfVector,id:2596,x:31223,y:34497,varname:node_2596,prsc:2;n:type:ShaderForge.SFN_Dot,id:3197,x:31397,y:34434,varname:node_3197,prsc:2,dt:1|A-7210-OUT,B-2596-OUT;n:type:ShaderForge.SFN_Power,id:3051,x:31586,y:34700,varname:node_3051,prsc:2|VAL-3197-OUT,EXP-9226-OUT;n:type:ShaderForge.SFN_Add,id:9370,x:32121,y:34532,varname:node_9370,prsc:2|A-9741-OUT,B-124-OUT;n:type:ShaderForge.SFN_Slider,id:1201,x:31024,y:35034,ptovrint:False,ptlb:Glossiness_Slider,ptin:_Glossiness_Slider,varname:_Glossiness_Slider,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:30,max:30;n:type:ShaderForge.SFN_Multiply,id:124,x:31772,y:35165,varname:node_124,prsc:2|A-3051-OUT,B-467-OUT;n:type:ShaderForge.SFN_Slider,id:467,x:31024,y:35190,ptovrint:False,ptlb:Specularity_Slider,ptin:_Specularity_Slider,varname:_Specularity_Slider,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:4;n:type:ShaderForge.SFN_Exp,id:9226,x:31401,y:34947,varname:node_9226,prsc:2,et:1|IN-1201-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1145,x:31257,y:32718,varname:node_1145,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7129,x:31223,y:34056,varname:node_7129,prsc:2|A-7924-RGB,B-9869-RGB;n:type:ShaderForge.SFN_Color,id:9869,x:30922,y:33934,ptovrint:False,ptlb:Albedo_Color,ptin:_Albedo_Color,varname:_Albedo_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2757353,c2:0.3462475,c3:0.375,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9326,x:31438,y:33176,ptovrint:False,ptlb:EmissiveMap,ptin:_EmissiveMap,varname:_EmissiveMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f3dd733cbafe7d2448e3eae3387b3d52,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3735,x:32104,y:33135,cmnt:EMISSIVE_CONTRIBUTION,varname:node_3735,prsc:2|A-1370-OUT,B-7659-OUT;n:type:ShaderForge.SFN_Power,id:1370,x:31606,y:32928,varname:node_1370,prsc:2|VAL-1145-RGB,EXP-1941-OUT;n:type:ShaderForge.SFN_Slider,id:3999,x:30958,y:32902,ptovrint:False,ptlb:AmbientLight_Slider,ptin:_AmbientLight_Slider,varname:_AmbientLight_Slider,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.052599,max:5;n:type:ShaderForge.SFN_Multiply,id:7659,x:31604,y:33353,varname:node_7659,prsc:2|A-9326-RGB,B-9146-RGB;n:type:ShaderForge.SFN_Color,id:9146,x:31438,y:33367,ptovrint:False,ptlb:EmissiveMap_Color,ptin:_EmissiveMap_Color,varname:_EmissiveMap_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8482758,c3:0,c4:1;n:type:ShaderForge.SFN_OneMinus,id:1941,x:31332,y:32918,varname:node_1941,prsc:2|IN-3999-OUT;n:type:ShaderForge.SFN_Slider,id:987,x:32639,y:33809,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_987,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.04360101,max:1;n:type:ShaderForge.SFN_Color,id:9840,x:32639,y:33467,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_9840,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;proporder:7924-2552-1201-467-9869-9326-3999-9146-987-9840;pass:END;sub:END;*/

Shader "Shader Forge/Lit" {
    Properties {
        _Albedo ("Albedo", 2D) = "white" {}
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _Glossiness_Slider ("Glossiness_Slider", Range(1, 30)) = 30
        _Specularity_Slider ("Specularity_Slider", Range(1, 4)) = 1
        _Albedo_Color ("Albedo_Color", Color) = (0.2757353,0.3462475,0.375,1)
        _EmissiveMap ("EmissiveMap", 2D) = "bump" {}
        _AmbientLight_Slider ("AmbientLight_Slider", Range(0, 5)) = 2.052599
        _EmissiveMap_Color ("EmissiveMap_Color", Color) = (1,0.8482758,0,1)
        _Outline_Width ("Outline_Width", Range(0, 1)) = 0.04360101
        _Outline_Color ("Outline_Color", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Outline_Width;
            uniform float4 _Outline_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Outline_Width,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                return fixed4(_Outline_Color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Glossiness_Slider;
            uniform float _Specularity_Slider;
            uniform float4 _Albedo_Color;
            uniform sampler2D _EmissiveMap; uniform float4 _EmissiveMap_ST;
            uniform float _AmbientLight_Slider;
            uniform float4 _EmissiveMap_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _EmissiveMap_var = tex2D(_EmissiveMap,TRANSFORM_TEX(i.uv0, _EmissiveMap));
                float3 emissive = (pow(UNITY_LIGHTMODEL_AMBIENT.rgb,(1.0 - _AmbientLight_Slider))*(_EmissiveMap_var.rgb*_EmissiveMap_Color.rgb));
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 finalColor = emissive + (attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,normalDirection))*(_Albedo_var.rgb*_Albedo_Color.rgb))+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Glossiness_Slider))*_Specularity_Slider)));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Glossiness_Slider;
            uniform float _Specularity_Slider;
            uniform float4 _Albedo_Color;
            uniform sampler2D _EmissiveMap; uniform float4 _EmissiveMap_ST;
            uniform float _AmbientLight_Slider;
            uniform float4 _EmissiveMap_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 finalColor = (attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,normalDirection))*(_Albedo_var.rgb*_Albedo_Color.rgb))+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Glossiness_Slider))*_Specularity_Slider)));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
