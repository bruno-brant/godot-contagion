shader_type canvas_item;

uniform vec4 non_edge_color = vec4(0, 0, 0, 0.7); // shadow
uniform vec4 edge_color = vec4 (1, 1, 0, 1); // yellow
uniform sampler2D SCREEN_TEXTURE: hint_screen_texture, filter_linear_mipmap;

void fragment() {
    vec2 texcoord = SCREEN_UV;
    vec3 tl = textureLod(SCREEN_TEXTURE, texcoord + vec2(-1,-1), 0).rgb;
    vec3 tc = textureLod(SCREEN_TEXTURE, texcoord + vec2( 0,-1), 0).rgb;
    vec3 tr = textureLod(SCREEN_TEXTURE, texcoord + vec2( 1,-1), 0).rgb;
    vec3 cl = textureLod(SCREEN_TEXTURE, texcoord + vec2(-1, 0), 0).rgb;
    vec3 cr = textureLod(SCREEN_TEXTURE, texcoord + vec2( 1, 0), 0).rgb;
    vec3 bl = textureLod(SCREEN_TEXTURE, texcoord + vec2(-1, 1), 0).rgb;
    vec3 bc = textureLod(SCREEN_TEXTURE, texcoord + vec2( 0, 1), 0).rgb;
    vec3 br = textureLod(SCREEN_TEXTURE, texcoord + vec2( 1, 1), 0).rgb;

    float sx = tr.r + cr.r*2.0 + br.r - tl.r - cl.r*2.0 - bl.r;
    float sy = bl.r + bc.r*2.0 + br.r - tl.r - tc.r*2.0 - tr.r;

    float s = sqrt(sx*sx+sy*sy);
	
    COLOR.rgba = mix(edge_color.rgba, non_edge_color.rgba, s);
}