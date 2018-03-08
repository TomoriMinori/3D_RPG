﻿//include file for important calculations during rendering
#ifndef MK_TOON_INC
	#define MK_TOON_INC

	#include "../Common/MKToonDef.cginc"
	/////////////////////////////////////////////////////////////////////////////////////////////
	// INC
	/////////////////////////////////////////////////////////////////////////////////////////////

	//world normal
	inline half3 WorldNormal(sampler2D normalMap, float2 uv, half3 tangent, half3 binormal, half3 normal)
	{
		half4 encode = tex2D(normalMap, uv);
		#if defined(UNITY_NO_DXT5nm)
			half3 local = encode.rgb * 2.0 - 1.0;
		#else
			half3 local = half3(2.0 * encode.a - 1.0, 2.0 * encode.g - 1.0, 0.0);
		#endif
		//local.z = sqrt(1.0 - dot(local, local));
		#if !defined(UNITY_NO_DXT5nm)
			local.z = sqrt(1.0 - saturate(dot(local.xy, local.xy)));
		#endif

		half3 t0, t1, t2;
		t0 = normalize(half3(tangent.x, binormal.x, normal.x));
		t1 = normalize(half3(tangent.y, binormal.y, normal.y));
		t2 = normalize(half3(tangent.z, binormal.z, normal.z));

		half3 n = 0;
		n.x = dot(t0, fixed3(0,0,1));
		n.y = dot(t1, fixed3(0,0,1));
		n.z = dot(t2, fixed3(0,0,1));
		return normalize(n);
	}

	//specular blinn phong
	inline half GetSpecular(half ndhv, half shine, half ndl)
	{
		return (ndl > 0.0) ? pow(ndhv, shine * SHINE_MULT) : 0.0;
	}

	//threshold based lighting type
	inline half TreshHoldLighting(half lThreshold, half smoothness, half v)
	{
		return smoothstep(lThreshold-smoothness*T_H, lThreshold+smoothness*T_H, v);
	}

	//Rim with smooth interpolation
	inline half3 RimDefault(half size, half3 vdn, fixed3 col, fixed intensity, half smoothness)
	{
		fixed r = pow ((1.0 - saturate(vdn)), size);
		r = smoothstep(r - smoothness, r + smoothness, vdn);
		return (1.0-r) * intensity * col.rgb;
	}

	//Brightness
	inline fixed3 BControl( fixed3 color, half b)
	{
		fixed3 bc = color * b;
		return bc;
	}
#endif