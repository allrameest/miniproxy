require 'albacore'

$current_directory = File.dirname(__FILE__)

$env_buildconfiguration = "Debug"
$env_solutionname = "MiniProxy"
$env_publishpath = File.join($current_directory, "artifacts")

#--------------------------------------
# Flow control
#--------------------------------------

task :default => [:publish]

task :publish => [:buildconfiguration_release, :publish_reset, :publish_miniproxy]

#--------------------------------------
# Tasks
#--------------------------------------

task :buildconfiguration_debug do
  $env_buildconfiguration = "Debug"
end

task :buildconfiguration_release do
  $env_buildconfiguration = "Release"
end

task :publish_reset do
  reset_publish_folders
end

msbuild :publish_miniproxy do |msb|
  publish_webapplication msb, "#{$env_publishpath}/MiniProxy", "src/MiniProxy/MiniProxy.csproj", :Release
end


#--------------------------------------
# Helpers
#--------------------------------------

def publish_webapplication(msb, publish_folder_path, project_file_path, build_configuration)
  msb.properties = {
    :configuration => build_configuration,
    :webprojectoutputdir => "#{publish_folder_path}/",
    :outdir => "#{publish_folder_path}/bin/"
  }
  msb.solution = "#{project_file_path}"
end

def reset_publish_folders()
  rm_r $env_publishpath, :force => true
end