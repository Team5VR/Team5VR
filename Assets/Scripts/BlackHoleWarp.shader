Shader "Unlit/BlackHoleWarp"
{
  Properties
    {
		_DiscWidth ("Width of the accretion disc of the black hole", float) = 0.1
		_DiscOuterRadius ("Object relative ourter disc radius", Range(0,1)) = 1
		_DiscInnerRadius ("Object relative disc inner radius", Range(0,1)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }
        Cull Front
    
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"                   
    
            static const float maxFloat = 3.402823466e+38;
    
            struct Attributes
            {
                float4 posObjectSpace : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
    
            struct v2f
            {
                float4 posCameraSpace : SV_POSITION;
                float3 posWorldSpace : TEXCOORD0;
                
                float3 centre : TEXCOORD1;
                float3 objectScale : TEXCOORD2;
            };
                
            v2f vert(Attributes IN)
            {
                v2f OUT = (v2f) 0;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.posObjectSpace.xyz);
                
                OUT.posCameraSpace = vertexInput.positionCS;
                OUT.posWorldSpace = vertexInput.positionWS;
                
                // Object information, based upon Unity's shadergraph library functions
                OUT.centre = UNITY_MATRIX_M._m03_m13_m23;
                OUT.objectScale = float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                            length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                            length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z)));
                
                return OUT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            }
			
			float _DiscWidth;
			float _DiscOuterRadius;
			float _DiscInnerRadius;
			
			// Returns the distance to the surface of the black hole and how far it penetrates
			// If inside the black hole the distance will be 0
			// If the ray misses black hole, the distanceTo = max float & distanceThrough = 0
			// Given rayDirection must be normal
			
			float2 sphereIntersection(float3 rayDirection, float3 rayOrigin, float3 centre, float radius)
			{
				float3 offset = rayOrigin - centre;
				const float a = 1;
				float b = 2 * dot(offset, rayDirection);
				float c = dot(offset, offset) - radius * radius;
				
				// < 0 is no
				// == 0 hits doesn't intersect
				// > 0 intersects through the sphere
				float intersection = b * b - 4 * a * c;
				if (intersection > 0)
				{
					float s = sqrt(intersection);
					float distanceToNear = max(0, (-b - s) / (2 * a));
					float distanceToFar = (-b + s) / (2 * a);
					
					if(distanceToFar >= 0)
					{
						return float2(distanceToNear, distanceToFar - distanceToNear);
					}
				}
				// Ray missed or didn't completely intersect
				return float2(maxFloat, 0);
			}
			
			float2 infiniteCylinderIntersection(float3 rayDirection, float3 rayOrigin, float3 cylinderOrigin, float3 cylinderDirection, float cylinderRadius)
			{
				float3 a0 = rayDirection - dot(rayDirection, cylinderDirection) * cylinderDirection;
				float a = dot(a0, a0);
				
				float3 dP = rayOrigin - cylinderOrigin;
				float3 c0 = dP - dot(dP, cylinderDirection) * cylinderDirection;
				float c = dot(c0, c0) - cylinderRadius * cylinderRadius;
				
				float b = 2 * dot(a0, c0);
				
				// < 0 is no
				// == 0 hits doesn't intersect
				// > 0 intersects through the sphere
				float intersection = b * b - 4 * a * c;				
				if (intersection > 0)
				{
					float s = sqrt(intersection);
					float distanceToNear = max(0, (-b-s)/(2*a));
					float distanceToFar = (-b+s)/(2*a);
				
					if(distanceToFar >= 0)
					{
						return float2(distanceToNear, distanceToFar - distanceToNear);
					}
				}
				// Ray missed or didn't completely intersect
				return float2(maxFloat, 0);
			}
			
			float infinitePlaneIntersect(float3 rayDirection, float3 rayOrigin, float3 planeOrigin, float3 planeDirection)
			{
				float a = 0;
				float b = dot(rayDirection, planeDirection);
				float c = dot(rayOrigin, planeDirection) - dot(planeDirection, planeOrigin);
				
				float intersection = b * b - 4 * a * c;
				
				return -c/b;
			}
			
			float discIntersection(float3 rayOrigin, float3 rayDirection, float3 plane1, float3 plane2, float3 discDirection, float discRadius, float innerRadius)
			{
				float discDistance = maxFloat;
				float2 cylinderIntersection = infiniteCylinderIntersection(rayDirection, rayOrigin, plane1, discDirection, discRadius);
				float cylinderDistance = cylinderIntersection.x;
				
				if(cylinderDistance < maxFloat)
				{
					float finiteC1 = dot(discDirection, rayOrigin + rayDirection * cylinderDistance - plane1);
					float finiteC2 = dot(discDirection, rayOrigin + rayDirection * cylinderDistance - plane2);
					
					// Ray intersects with the edges of the cylinder/discDirection
					if(finiteC1 > 0 && finiteC2 < 0 && cylinderDistance > 0)
					{
						discDirection = cylinderDistance;
					}
					else
					{
						float discRadiusSquared = discRadius * discRadius;
						float innerDiscRadiusSquared = innerRadius * innerRadius;
						
						float plane1Distance = max(infinitePlaneIntersect(rayDirection, rayOrigin, plane1, discDirection), 0);
						float3 q1 = rayOrigin + rayDirection * plane1Distance;
						float plane1q1DistanceSquared = dot(q1 - plane1, q1 - plane1);
						
						//Ray goes through the lower plane of the cylinder/disc
						if(plane1Distance > 0 && plane1q1DistanceSquared < discRadiusSquared && plane1q1DistanceSquared > innerDiscRadiusSquared)
						{
							if(plane1Distance < discDistance)
							{
								discDistance = plane1Distance;
							}
						}
						
						float plane2Distance = max(infinitePlaneIntersect(rayDirection, rayOrigin, plane2, discDirection), 0);
						float3 q2 = rayOrigin + rayDirection * plane2Distance;
						float plane2q2DistanceSquared = dot(q2 - plane2, q2 - plane2);
						
						//Ray goes through the upper plane of the cylinder/disc
						if(plane2Distance > 0 && plane2q2DistanceSquared < discRadiusSquared && plane2q2DistanceSquared > innerDiscRadiusSquared)
						{
							if(plane2Distance < discDistance)
							{
								discDistance = plane2Distance;
							}
						}
					}
				}
				return discDistance;
			}
                
            float4 frag(v2f IN) : SV_Target
            {
                // Initial ray information
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDirection = normalize(IN.posWorldSpace - _WorldSpaceCameraPos);
				
				float blackHoleRadius = 0.5 * min(min(IN.objectScale.x, IN.objectScale.y), IN.objectScale.z);
				float blackHoleOuterIntersection = sphereIntersection(rayDirection, rayOrigin, IN.centre, blackHoleRadius);
				
				// Disc information, direction is objects rotation
				float3 discDirection = normalize(mul(unity_ObjectToWorld, float4(0,1,0,0)).xyz);
				float3 plane1 = IN.centre - 0.5 * _DiscWidth * discDirection;
				float3 plane2 = IN.centre + 0.5 * _DiscWidth * discDirection;
				float discRadius = blackHoleRadius * _DiscOuterRadius;
				float innerRadius = blackHoleRadius * _DiscInnerRadius;
				
				// Raymarching
				float transmittance = 0;
				
				// If the ray intersects the outer black hole
				if(blackHoleOuterIntersection.x < maxFloat)
				{
					float discDistance = discIntersection(rayDirection, rayOrigin, plane1, plane2, discDirection, discRadius, innerRadius);
					if(discDistance < maxFloat)
					{
						transmittance = 1;
					}
				}
				
				
				
				float2 screenUV = IN.posCameraSpace.xy / _ScreenParams.xy;
				float3 backgroundColor = SampleSceneColor(screenUV);
				float3 color = lerp(backgroundColor, float3(1, 0, 0), transmittance);
				
                return float4(color, 1);
            }
            ENDHLSL
        }
    }
}
