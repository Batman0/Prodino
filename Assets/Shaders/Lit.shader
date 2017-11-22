// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33242,y:32793,varname:node_4013,prsc:2|normal-2552-RGB,emission-370-OUT,custl-8293-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:9202,x:32609,y:33816,varname:node_9202,prsc:2;n:type:ShaderForge.SFN_LightColor,id:1097,x:32609,y:33967,varname:node_1097,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8293,x:32945,y:34058,varname:node_8293,prsc:2|A-9202-OUT,B-1097-RGB,C-9370-OUT;n:type:ShaderForge.SFN_LightVector,id:1876,x:31741,y:33953,varname:node_1876,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7210,x:31741,y:34094,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:1208,x:32105,y:34072,varname:node_1208,prsc:2,dt:1|A-1876-OUT,B-7210-OUT;n:type:ShaderForge.SFN_Multiply,id:9741,x:32292,y:33915,varname:node_9741,prsc:2|A-1208-OUT,B-7129-OUT;n:type:ShaderForge.SFN_Tex2d,id:7924,x:31088,y:34066,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_7924,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fbad32031c1038c41bec65a52251bb38,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2552,x:32736,y:32621,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_2552,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:74a6817d8479fe74b92e277fe624b462,ntxv:3,isnm:True;n:type:ShaderForge.SFN_HalfVector,id:2596,x:31741,y:34258,varname:node_2596,prsc:2;n:type:ShaderForge.SFN_Dot,id:3197,x:31915,y:34195,varname:node_3197,prsc:2,dt:1|A-7210-OUT,B-2596-OUT;n:type:ShaderForge.SFN_Power,id:3051,x:32128,y:34466,varname:node_3051,prsc:2|VAL-3197-OUT,EXP-9226-OUT;n:type:ShaderForge.SFN_Add,id:9370,x:32696,y:34311,varname:node_9370,prsc:2|A-9741-OUT,B-124-OUT;n:type:ShaderForge.SFN_Slider,id:1201,x:31567,y:34550,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_1201,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:30,max:30;n:type:ShaderForge.SFN_Multiply,id:124,x:32328,y:34527,varname:node_124,prsc:2|A-3051-OUT,B-467-OUT;n:type:ShaderForge.SFN_Slider,id:467,x:31549,y:34717,ptovrint:False,ptlb:Specularity,ptin:_Specularity,varname:node_467,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:4;n:type:ShaderForge.SFN_Exp,id:9226,x:31898,y:34550,varname:node_9226,prsc:2,et:1|IN-1201-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1145,x:30919,y:32316,varname:node_1145,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7129,x:31725,y:33817,varname:node_7129,prsc:2|A-7924-RGB,B-9869-RGB;n:type:ShaderForge.SFN_Color,id:9869,x:31088,y:34286,ptovrint:False,ptlb:Albedo_Color,ptin:_Albedo_Color,varname:node_9869,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2757353,c2:0.3462475,c3:0.375,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9326,x:30864,y:32942,ptovrint:False,ptlb:Emissive_Map,ptin:_Emissive_Map,varname:node_9326,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f3dd733cbafe7d2448e3eae3387b3d52,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3735,x:31530,y:32901,varname:node_3735,prsc:2|A-1370-OUT,B-7659-OUT;n:type:ShaderForge.SFN_Power,id:1370,x:31258,y:32444,varname:node_1370,prsc:2|VAL-1145-RGB,EXP-3999-OUT;n:type:ShaderForge.SFN_Slider,id:3999,x:30875,y:32655,ptovrint:False,ptlb:AmbientLight_Slider,ptin:_AmbientLight_Slider,varname:node_3999,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-1,max:1;n:type:ShaderForge.SFN_Multiply,id:7659,x:31030,y:33119,varname:node_7659,prsc:2|A-9326-RGB,B-9146-RGB;n:type:ShaderForge.SFN_Color,id:9146,x:30864,y:33133,ptovrint:False,ptlb:Emissive_Map_Color,ptin:_Emissive_Map_Color,varname:node_9146,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8482758,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:370,x:32157,y:33248,varname:node_370,prsc:2|A-7129-OUT,B-3735-OUT;proporder:7924-2552-1201-467-9869-9326-3999-9146;pass:END;sub:END;*/

Shader "Shader Forge/Lit" {
    Properties {
        _Albedo ("Albedo", 2D) = "white" {}
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _Glossiness ("Glossiness", Range(1, 30)) = 30
        _Specularity ("Specularity", Range(1, 4)) = 1
        _Albedo_Color ("Albedo_Color", Color) = (0.2757353,0.3462475,0.375,1)
        _Emissive_Map ("Emissive_Map", 2D) = "bump" {}
        _AmbientLight_Slider ("AmbientLight_Slider", Range(-1, 1)) = -1
        _Emissive_Map_Color ("Emissive_Map_Color", Color) = (1,0.8482758,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
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
            uniform float _Glossiness;
            uniform float _Specularity;
            uniform float4 _Albedo_Color;
            uniform sampler2D _Emissive_Map; uniform float4 _Emissive_Map_ST;
            uniform float _AmbientLight_Slider;
            uniform float4 _Emissive_Map_Color;
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
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
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
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_7129 = (_Albedo_var.rgb*_Albedo_Color.rgb);
                float4 _Emissive_Map_var = tex2D(_Emissive_Map,TRANSFORM_TEX(i.uv0, _Emissive_Map));
                float3 emissive = (node_7129*(pow(UNITY_LIGHTMODEL_AMBIENT.rgb,_AmbientLight_Slider)*(_Emissive_Map_var.rgb*_Emissive_Map_Color.rgb)));
                float3 finalColor = emissive + (attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,normalDirection))*node_7129)+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Glossiness))*_Specularity)));
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
            uniform float _Glossiness;
            uniform float _Specularity;
            uniform float4 _Albedo_Color;
            uniform sampler2D _Emissive_Map; uniform float4 _Emissive_Map_ST;
            uniform float _AmbientLight_Slider;
            uniform float4 _Emissive_Map_Color;
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
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
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
                float3 node_7129 = (_Albedo_var.rgb*_Albedo_Color.rgb);
                float3 finalColor = (attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,normalDirection))*node_7129)+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Glossiness))*_Specularity)));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
