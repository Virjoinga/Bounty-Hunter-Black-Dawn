Shader "iPhone/AlphaBlend_NoTag" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader { 
 Pass {
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture }
 }
}
}