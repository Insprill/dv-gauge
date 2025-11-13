import bpy
import bmesh

# Replace here with the verticies copied from the verticies.json file
indices = set([1, 2, 3])

obj = bpy.context.object
me = obj.data
bm = bmesh.from_edit_mesh(me)

for vert in bm.verts:
    vert.select = vert.index in indices

bmesh.update_edit_mesh(me)
