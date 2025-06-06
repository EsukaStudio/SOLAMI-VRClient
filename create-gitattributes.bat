@echo off
echo # Unity LFS 配置 > .gitattributes
echo. >> .gitattributes
echo # 图像文件 >> .gitattributes
echo *.jpg filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.jpeg filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.png filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.tga filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.tif filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.tiff filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.psd filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.exr filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.hdr filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 音频文件 >> .gitattributes
echo *.mp3 filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.wav filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.ogg filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.aif filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.aiff filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 视频文件 >> .gitattributes
echo *.mp4 filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.mov filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.avi filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.webm filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 3D模型 >> .gitattributes
echo *.fbx filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.obj filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.max filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.blend filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.dae filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.mb filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.ma filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.3ds filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.glb filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.gltf filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # Unity特定 >> .gitattributes
echo *.unitypackage filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.asset filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.unity filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 库文件 >> .gitattributes
echo *.dll filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.pdb filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.so filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.a filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 字体 >> .gitattributes
echo *.ttf filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.otf filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 压缩文件 >> .gitattributes
echo *.zip filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.rar filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.7z filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.tar filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.gz filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 其他二进制文件 >> .gitattributes
echo *.pdf filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.exe filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo *.bin filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo. >> .gitattributes
echo # 特定项目中的大文件 >> .gitattributes
echo Assets/01_Models/Character/chappie/chappie.fbx filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo "Assets/06_UI/Fonts/DENGB SDF.asset" filter=lfs diff=lfs merge=lfs -text >> .gitattributes
echo Assets/06_UI/Fonts/msyh.asset filter=lfs diff=lfs merge=lfs -text >> .gitattributes
