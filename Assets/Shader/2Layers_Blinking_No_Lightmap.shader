Ø:Shader "Advanced/2_Layer_Blinking_No_Lightmap" {
Properties {
 _TintColor ("Tint Color", Color) = (1,1,1,1)
 _texBase ("MainTex", 2D) = "" {}
 _tex2 ("Texture2", 2D) = "" {}
 _FadeOutDistNear ("Near fadeout dist", Float) = 0
 _FadeOutDistFar ("Far fadeout dist", Float) = 1000
 _Multiplier ("Color multiplier", Float) = 1
 _Bias ("Bias", Float) = 0.1
 _TimeOnDuration ("ON duration", Float) = 0.5
 _TimeOffDuration ("OFF duration", Float) = 0.5
 _BlinkingTimeOffsScale ("Blinking time offset scale (seconds)", Float) = 5
 _SizeGrowStartDist ("Size grow start dist", Float) = 5
 _SizeGrowEndDist ("Size grow end dist", Float) = 50
 _MaxGrowSize ("Max grow size", Float) = 2.5
 _NoiseAmount ("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
 _Color ("Color", Color) = (1,1,1,1)
}
SubShader { 
 Pass {
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD0;
uniform highp vec4 _Color;
uniform highp float _NoiseAmount;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _TimeOffDuration;
uniform highp float _TimeOnDuration;
uniform highp float _Bias;
uniform highp float _Multiplier;
uniform highp float _FadeOutDistFar;
uniform highp float _FadeOutDistNear;
uniform highp vec4 _tex2_ST;
uniform highp vec4 _texBase_ST;


uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (gl_ModelViewProjectionMatrix * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _texBase_ST.xy) + _texBase_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _tex2_ST.xy) + _tex2_ST.zw);
  highp float tmpvar_4;
  tmpvar_4 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_5;
  tmpvar_5 = (gl_ModelViewMatrix * _glesVertex).xyz;
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = clamp ((tmpvar_6 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - clamp ((max ((tmpvar_6 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_9;
  y_9 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_10;
  tmpvar_10 = (tmpvar_4 / y_9);
  highp float tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * y_9);
  highp float tmpvar_12;
  if ((tmpvar_10 >= 0.0)) {
    tmpvar_12 = tmpvar_11;
  } else {
    tmpvar_12 = -(tmpvar_11);
  };
  highp float t_13;
  t_13 = max (min ((tmpvar_12 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_14;
  edge0_14 = (_TimeOnDuration * 0.75);
  highp float t_15;
  t_15 = max (min (((tmpvar_12 - edge0_14) / (_TimeOnDuration - edge0_14)), 1.0), 0.0);
  highp float tmpvar_16;
  tmpvar_16 = ((t_13 * (t_13 * (3.0 - (2.0 * t_13)))) * (1.0 - (t_15 * (t_15 * (3.0 - (2.0 * t_15))))));
  highp float tmpvar_17;
  tmpvar_17 = (tmpvar_4 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_18;
  tmpvar_18 = ((_NoiseAmount * (sin(tmpvar_17) * ((0.5 * cos(((tmpvar_17 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_19;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_19 = tmpvar_16;
  } else {
    tmpvar_19 = tmpvar_18;
  };
  highp float tmpvar_20;
  tmpvar_20 = (tmpvar_7 * tmpvar_7);
  highp vec4 tmpvar_21;
  tmpvar_21 = (((((tmpvar_20 * tmpvar_20) * (tmpvar_8 * tmpvar_8)) * _Color) * _Multiplier) * (tmpvar_19 + _Bias));
  tmpvar_2 = tmpvar_21;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD0;
uniform sampler2D _tex2;
uniform sampler2D _texBase;
void main ()
{
  gl_FragData[0] = (texture2D (_texBase, xlv_TEXCOORD0.xy) + (texture2D (_tex2, xlv_TEXCOORD0.zw) * xlv_TEXCOORD1));
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
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD0;
uniform highp vec4 _Color;
uniform highp float _NoiseAmount;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _TimeOffDuration;
uniform highp float _TimeOnDuration;
uniform highp float _Bias;
uniform highp float _Multiplier;
uniform highp float _FadeOutDistFar;
uniform highp float _FadeOutDistNear;
uniform highp vec4 _tex2_ST;
uniform highp vec4 _texBase_ST;


uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (gl_ModelViewProjectionMatrix * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _texBase_ST.xy) + _texBase_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _tex2_ST.xy) + _tex2_ST.zw);
  highp float tmpvar_4;
  tmpvar_4 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_5;
  tmpvar_5 = (gl_ModelViewMatrix * _glesVertex).xyz;
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = clamp ((tmpvar_6 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - clamp ((max ((tmpvar_6 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_9;
  y_9 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_10;
  tmpvar_10 = (tmpvar_4 / y_9);
  highp float tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * y_9);
  highp float tmpvar_12;
  if ((tmpvar_10 >= 0.0)) {
    tmpvar_12 = tmpvar_11;
  } else {
    tmpvar_12 = -(tmpvar_11);
  };
  highp float t_13;
  t_13 = max (min ((tmpvar_12 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_14;
  edge0_14 = (_TimeOnDuration * 0.75);
  highp float t_15;
  t_15 = max (min (((tmpvar_12 - edge0_14) / (_TimeOnDuration - edge0_14)), 1.0), 0.0);
  highp float tmpvar_16;
  tmpvar_16 = ((t_13 * (t_13 * (3.0 - (2.0 * t_13)))) * (1.0 - (t_15 * (t_15 * (3.0 - (2.0 * t_15))))));
  highp float tmpvar_17;
  tmpvar_17 = (tmpvar_4 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_18;
  tmpvar_18 = ((_NoiseAmount * (sin(tmpvar_17) * ((0.5 * cos(((tmpvar_17 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_19;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_19 = tmpvar_16;
  } else {
    tmpvar_19 = tmpvar_18;
  };
  highp float tmpvar_20;
  tmpvar_20 = (tmpvar_7 * tmpvar_7);
  highp vec4 tmpvar_21;
  tmpvar_21 = (((((tmpvar_20 * tmpvar_20) * (tmpvar_8 * tmpvar_8)) * _Color) * _Multiplier) * (tmpvar_19 + _Bias));
  tmpvar_2 = tmpvar_21;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD0;
uniform sampler2D _tex2;
uniform sampler2D _texBase;
void main ()
{
  gl_FragData[0] = (texture2D (_texBase, xlv_TEXCOORD0.xy) + (texture2D (_tex2, xlv_TEXCOORD0.zw) * xlv_TEXCOORD1));
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