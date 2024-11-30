Shader "Advanced/Animation_Texture_16_Block" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NumTexTiles ("Num tex tiles", Vector) = (1,4,0,0)
        _ReplaySpeed ("Replay speed - FPS", Float) = 4
        _Color ("Color", Color) = (1,1,1,1)
    }
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying lowp vec4 xlv_COLOR;
varying highp vec4 xlv_TEXCOORD0;
uniform highp float _ReplaySpeed;
uniform highp vec4 _NumTexTiles;
uniform highp vec4 _Color;

uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 index_1;
  highp vec4 tile_2;
  lowp vec4 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = (((_glesColor.w * 60.0) + _Time.y) * _ReplaySpeed);
  highp float tmpvar_5;
  tmpvar_5 = floor(tmpvar_4);
  highp float tmpvar_6;
  tmpvar_6 = (tmpvar_5 + 1.0);
  highp float tmpvar_7;
  tmpvar_7 = (tmpvar_4 - tmpvar_5);
  highp vec2 tmpvar_8;
  tmpvar_8 = (1.0/(_NumTexTiles.xy));
  index_1.zw = _glesMultiTexCoord0.zw;
  index_1.x = floor((_glesMultiTexCoord0.x / 0.25));
  index_1.y = floor((_glesMultiTexCoord0.y / 0.25));
  highp vec2 tmpvar_9;
  tmpvar_9.x = tmpvar_5;
  tmpvar_9.y = floor((tmpvar_5 / _NumTexTiles.x));
  tile_2.xy = tmpvar_9;
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_6;
  tmpvar_10.y = floor((tmpvar_6 / _NumTexTiles.x));
  tile_2.zw = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (tile_2 / _NumTexTiles.xyxy);
  highp vec4 tmpvar_12;
  tmpvar_12 = (fract(abs(tmpvar_11)) * _NumTexTiles.xyxy);
  highp float tmpvar_13;
  if ((tmpvar_11.x >= 0.0)) {
    tmpvar_13 = tmpvar_12.x;
  } else {
    tmpvar_13 = -(tmpvar_12.x);
  };
  highp float tmpvar_14;
  if ((tmpvar_11.y >= 0.0)) {
    tmpvar_14 = tmpvar_12.y;
  } else {
    tmpvar_14 = -(tmpvar_12.y);
  };
  highp float tmpvar_15;
  if ((tmpvar_11.z >= 0.0)) {
    tmpvar_15 = tmpvar_12.z;
  } else {
    tmpvar_15 = -(tmpvar_12.z);
  };
  highp float tmpvar_16;
  if ((tmpvar_11.w >= 0.0)) {
    tmpvar_16 = tmpvar_12.w;
  } else {
    tmpvar_16 = -(tmpvar_12.w);
  };
  highp vec4 tmpvar_17;
  tmpvar_17.x = tmpvar_13;
  tmpvar_17.y = tmpvar_14;
  tmpvar_17.z = tmpvar_15;
  tmpvar_17.w = tmpvar_16;
  tile_2 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18.xyz = (_Color.xyz * _glesColor.xyz);
  tmpvar_18.w = tmpvar_7;
  tmpvar_3 = tmpvar_18;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((((_glesMultiTexCoord0.xyxy - (index_1 * 0.25)) + (tmpvar_17 * 0.25)) * tmpvar_8.xyxy) + (index_1 * 0.25));
  xlv_COLOR = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_COLOR;
varying highp vec4 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
void main ()
{
  gl_FragData[0] = (texture2D (_MainTex, xlv_TEXCOORD0.xy) * xlv_COLOR);
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
}
 }
}
}