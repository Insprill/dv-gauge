import bpy
import subprocess
import sys

obj = bpy.context.active_object
selected_verts = [v for v in obj.data.vertices if v.select]

vert_indices = ", ".join(str(v.index) for v in selected_verts)

if sys.platform.startswith('linux'):
    subprocess.run(["wl-copy"], input=vert_indices, check=True, text=True)
elif sys.platform.startswith('win'):
    subprocess.run(["clip"], input=vert_indices, check=True, text=True)
