ˆShader "Advanced/Skybox" {
Properties {
 _MainTex ("Base layer (RGB)", 2D) = "white" {}
 _DetailTex ("2nd layer (RGB)", 2D) = "white" {}
 _ScrollX ("Base layer Scroll speed X", Float) = 1
 _ScrollY ("Base layer Scroll speed Y", Float) = 0
 _Scroll2X ("2nd layer Scroll speed X", Float) = 1
 _Scroll2Y ("2nd layer Scroll speed Y", Float) = 0
 _AMultiplier ("Layer Multiplier", Float) = 0.5
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Geometry+10" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry+10" "RenderType"="Opaque" }
  ZWrite Off
  Fog { Mode Off }
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying lowp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp float _AMultiplier;
uniform highp float _Scroll2Y;
uniform highp float _Scroll2X;
uniform highp float _ScrollY;
uniform highp float _ScrollX;
uniform highp vec4 _DetailTex_ST;
uniform highp vec4 _MainTex_ST;

uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2.x = _ScrollX;
  tmpvar_2.y = _ScrollY;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _Scroll2X;
  tmpvar_3.y = _Scroll2Y;
  highp vec4 tmpvar_4;
  tmpvar_4.x = _AMultiplier;
  tmpvar_4.y = _AMultiplier;
  tmpvar_4.z = _AMultiplier;
  tmpvar_4.w = _AMultiplier;
  tmpvar_1 = tmpvar_4;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_2 * _Time.xy)));
  xlv_TEXCOORD1 = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  xlv_TEXCOORD2 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _DetailTex;
uniform sampler2D _MainTex;
void main ()
{
  gl_FragData[0] = ((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_DetailTex, xlv_TEXCOORD1)) * xlv_TEXCOORD2);
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying lowp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp float _AMultiplier;
uniform highp float _Scroll2Y;
uniform highp float _Scroll2X;
uniform highp float _ScrollY;
uniform highp float _ScrollX;
uniform highp vec4 _DetailTex_ST;
uniform highp vec4 _MainTex_ST;

uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2.x = _ScrollX;
  tmpvar_2.y = _ScrollY;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _Scroll2X;
  tmpvar_3.y = _Scroll2Y;
  highp vec4 tmpvar_4;
  tmpvar_4.x = _AMultiplier;
  tmpvar_4.y = _AMultiplier;
  tmpvar_4.z = _AMultiplier;
  tmpvar_4.w = _AMultiplier;
  tmpvar_1 = tmpvar_4;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_2 * _Time.xy)));
  xlv_TEXCOORD1 = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  xlv_TEXCOORD2 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _DetailTex;
uniform sampler2D _MainTex;
void main ()
{
  gl_FragData[0] = ((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_DetailTex, xlv_TEXCOORD1)) * xlv_TEXCOORD2);
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES"
}
}
 }
}
}