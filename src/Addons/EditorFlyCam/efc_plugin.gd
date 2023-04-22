@tool
extends EditorPlugin

var _currCam = null
var _plugin: EditorFlyCam

func _enter_tree() -> void:
	print("FlyCam plugin Enter tree")
	
	_plugin = EditorFlyCam.new()
	
	add_inspector_plugin(_plugin)
	add_to_group("EditorFlyCamMain")
	
	
	
	print("added plugin")


func _exit_tree() -> void:
	print("FlyCam plugin Exit tree")
	remove_inspector_plugin(_plugin)


func handles(_o):
	# we want to get the camera so we just handle everything
	return true

func _forward_3d_gui_input(viewport_camera: Camera3D, event: InputEvent):
	print("Getting camera!")
	
	# Maybe there is a better way to get the editor camera?
	if _currCam != viewport_camera:
		_currCam = viewport_camera
		
	return EditorPlugin.AFTER_GUI_INPUT_PASS
		

func _forward_canvas_gui_input(event):
	print("gui input")
	pass

func _forward_spatial_gui_input(p_camera: Camera3D, p_event: InputEvent) -> bool:
	print("Getting camera!")
	# Maybe there is a better way to get the editor camera?
	if _currCam != p_camera:
		_currCam = p_camera
	return false
