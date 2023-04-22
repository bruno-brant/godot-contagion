@tool
extends EditorInspectorPlugin 
class_name EditorFlyCam

const CamPanel = preload("res://addons/EditorFlyCam/CamView.tscn")
const CamCtrlPanel = preload("res://addons/EditorFlyCam/CamCtrl.tscn")

var cam_panel_instance
var ctrl_panel_instance

func _can_handle(object):
	return object is Path3D or object is Camera3D

## Add the custom draw
func _parse_property(object: Object, type: Variant.Type, name: String, hint_type: PropertyHint, hint_string: String, usage_flags: PropertyUsageFlags, wide: bool):
	# Place as first element
	if name == "keep_aspect":
		cam_panel_instance = CamPanel.instantiate()
		add_custom_control(cam_panel_instance)
		cam_panel_instance.get_node("CamView").tracked_cam = weakref(object)
		
		# Follow toggle
		ctrl_panel_instance = CamCtrlPanel.instantiate()
		add_custom_control(ctrl_panel_instance)
		ctrl_panel_instance.tracked_cam = object
	return false
