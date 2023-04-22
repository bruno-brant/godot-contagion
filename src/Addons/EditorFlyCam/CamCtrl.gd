@tool
extends VBoxContainer

var main_plugin: EditorPlugin = null
var tracked_cam: Node3D = null
@onready var toggle: CheckButton = $FollowEditor


func _ready() -> void:
	var main_plugins = get_tree().get_nodes_in_group("EditorFlyCamMain")
	
	if len(main_plugins) < 1:
		printerr("Could not get EditorFlyCamMain plugin")
		return
		
	main_plugin = main_plugins[0]


func _process(_delta: float) -> void:
	update_cam()
	

func update_cam():
	if not toggle.button_pressed or null == main_plugin or null == tracked_cam:
		return	
		
	if not main_plugin._currCam:
		printerr("No current cam in main_plugin")
		return
		
	tracked_cam.global_transform = main_plugin._currCam.global_transform


func _on_FollowEditor_toggled(button_pressed: bool) -> void:
	update_cam()
