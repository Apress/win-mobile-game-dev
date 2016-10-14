plugins_menu = UI.menu("Plugins")
plugins_menu.add_item("ExportTexturedOBJ") { exportOBJ_textures }
plugins_menu.add_item("ExportOBJ") { exportOBJ_notextures }

def exportOBJ_textures()
	exportOBJ(true)
end

def exportOBJ_notextures()
	exportOBJ(false)
end

def exportOBJ(use_mtl)
	if($file_path = UI.savepanel "Export OBJ", "~", "sketch.obj") then
		if(use_mtl) then
			$mtl_data = getMTLData();
		end
		
		$obj_data = getOBJData(use_mtl);
		if($obj_data != nil) then
			if(use_mtl) then
				$mtl_path = "" + $file_path
				$mtl_path[-3,3] = "mtl"
				$obj_data = "mtllib #{$mtl_path}\n" + $obj_data
			end
			File.open($file_path, "w") { |fh| fh.write $obj_data }
		end
		if(use_mtl) then
			if($obj_data != nil) then
				File.open($mtl_path, "w") { |fh| fh.write $mtl_data }
			end
		end
	end
end

def getMTLData()
	materials = Sketchup.active_model.materials
	
	my_str = ""
	
	materials.each { |mtl|
		my_str += "newmtl #{mtl.display_name}\n"
		my_str += "Kd #{mtl.color.red/255.0} #{mtl.color.green/255.0} #{mtl.color.blue/255.0}\n"
		if(mtl.texture) then
			my_str += "map_Kd #{File.basename(mtl.texture.filename)}\n"
		end
		my_str += "illum 1\n"
		my_str += "\n"
	}
	
	return my_str
end

def getOBJData(use_mtl) 
	tw = Sketchup.create_texture_writer
	
	model = Sketchup.active_model
	if(!model) then
		UI.messagebox "No active model exists."
		return nil
	end
	
	entities = model.entities
	if(!entities) then
		UI.messagebox "The active model has no entities"
		return nil
	end
	
	$all_vertices_pointers = Array.new
	$all_texture_pointers = Array.new
	
	faces_string = ""
	
	entities.each { |entity|
		if(entity.typename == "Face") then
			my_mesh = entity.mesh 0
			
			n_points = my_mesh.count_points
			for i in 1..n_points
				my_point = my_mesh.point_at i
				
				my_index = $all_vertices_pointers.index(my_point)
				if(my_index) then
					my_index += 1
				else
					$all_vertices_pointers += [my_point]
				end
			end
			
			n_polys = my_mesh.count_polygons
			if(use_mtl) then
				if(entity.material != nil) then
					faces_string += "usemtl #{entity.material.display_name}\n"
				end
			end
			
			uvHelp = entity.get_UVHelper(true, nil, tw)
			for i in 1..n_polys
				faces_string += "f"
				my_mesh.polygon_points_at(i).each { |my_point|
					faces_string += " "
					faces_string += ($all_vertices_pointers.index(my_point)+1).to_s
					
					if(use_mtl) then
						uvq = uvHelp.get_front_UVQ(my_point)
						my_index = $all_texture_pointers.index(uvq)
						if(my_index) then
							my_index += 1
						else
							$all_texture_pointers += [uvq]
						end
						
						faces_string += "/" + ($all_texture_pointers.index(uvq)+1).to_s
					end
				}
				faces_string += "\n"
				
			end
		end
	}
	
	my_str = ""
	
	$all_vertices_pointers.each { |vert|
		my_str += to_obj_vertex(vert)
	}
	
	$all_texture_pointers.each { |uv|
		my_str += to_obj_texcoord(uv)
	}
	
	my_str += faces_string
	
	return my_str
end

def to_obj_vertex(vert)
	my_str = "v "
	my_str += vert.to_a.join(" ")
	my_str += "\n"
end

def to_obj_texcoord(uv)
	my_str = "vt "
	my_str += uv.x.to_f.to_s + " " + uv.y.to_f.to_s
	my_str += "\n"
end