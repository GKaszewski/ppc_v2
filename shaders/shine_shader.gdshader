shader_type canvas_item;

uniform vec4 shine_color : source_color = vec4(1.0);
uniform float shine_speed : hint_range(-10.0, 10.0, 0.1) = 1.0;
uniform float shine_size : hint_range(0.001, 1.0, 0.01) = 0.01;
uniform float shine_frequency: hint_range(1.0, 45.0, 0.5) = 5.0;
uniform float shine_angle_deg : hint_range(-360.0, 360.0, 1.0) = 45.0;

void fragment() {
	COLOR = texture(TEXTURE, UV);
	// Convert degrees to radians
	float angle_rad = radians(shine_angle_deg);

	// Calculate direction vector for the shine line
	vec2 shine_dir = vec2(cos(angle_rad), sin(angle_rad));

	// Project UV onto shine direction
	float projection = dot(UV, shine_dir);
	float shine = step(1.0 - shine_size * 0.5, 0.5 + 0.5 * sin(projection + TIME * shine_speed * shine_frequency));
	COLOR.rgb = mix(COLOR.rgb, shine_color.rgb, shine * shine_color.a);
}