# Road Importer
Automated road asset creation utilities for Cities: Skylines. The mod reads FBX/OBJ models and XML files containing parameters for the road to make CRP assets in the road editor.

## Installation
The mod is to be installed the same as a local mod in the game, but it also keeps its own data. The compiled file needs to be placed in `C:\Users\%USERNAME%\AppData\Local\Colossal Order\Cities_Skylines\Addons\Mods\RoadImporter\`. The post-build script in the Visual Studio project file does this automaticaly, if the DLL file is compiled from source. 

The Road Importer mod uses the directory `C:\Users\%USERNAME%\AppData\Local\Colossal Order\Cities_Skylines\RoadImporter\` as its data folder containing the input files for asset creation. These input files are described as below.
- `imports.txt`: the names of the roads to import. A new line is ***not*** appended at the end of the file.
- `imports/`: the directory containing the models for segments and nodes in each road, the thumbnail icons, and the road parameters as an XML file. Filenames in the folders should **start with the name of the road** to identify which road a file belongs to. The names of the meshes and their textures are also defined in `<CSMesh>` sections in the the XML file.
- `textures/`: the directory containing textures to be used on the roads. For the specific use of CSUR, all road assets share the same set of textures, so the mod only imports a single copy of textures for efficiency.

## Usage
Using the mod follow the steps below:

1. Enable the “Road Importer” mod and open Asset Editor to create a new road asset. 
2. When Asset Editor is loaded, a dialog box will appear which shows the number of roads found in the imports.txt file and lists them.
3.	Select any road as template. Most data about the template will be cleared, but **the level of the road and DLC requirements** are preserved.
4.	Press **CTRL+L** to start the import job. The UI will be manipulated by the mod so you can just leave the game running on its own and do anything else on your PC.
5.	The job is finished after saving the last asset in the list, and you will be able to move your view or start/pause the simulation. Then you can exit the Asset Editor, and road assets are generated. 

## Performance
For optimal performance of Road Importer, the mod needs to be run in the `-noWorkshop` mode of Cities: Skylines and the number of extra mods and assets enabled should be kept at a minimum. On our hardware platform used for building CSUR roads (AMD R7 3700X, 64 GB DDR4-3600, NVIDIA RTX 2080S, 1 TB PCIE Gen4 SSD), creation of each road asset roughly took 10 seconds, while the actual amount of time taken will depend on the complexity of road asset and your hardware configurations.

Please be advised that **memory leaks** may occur in the Asset Editor. This implies that in a exceedingly large job, the memory used by `Cities.exe` will increase to >10 or >20 GB, eventually leading to the game crashing. **It is recommended that the total number of roads in a single job is kept smaller than 200.** 

Please be advised that **thread leaks** may occur in the Asset Editor. When a model is being imported, the game will compress any reasonably-sized texture and **texture compression adds 32 threads to the total thread count in `Cities.exe`.** The Asset Editor uses about 150-170 threads when it is initially loaded, and the game will crash when the number of threads exceeds 1024. This implies that the number of *unique* textures involved in a road import job should be always smaller than **25**. 

## Texture Optimization
Road Importer provides different texture optimization levels for creating packages of large numbers of road assets. It is defined using the `optLevel` variable in `Environment.cs`;
- `optLevel == 0`: Texture is not optimized, and the full set of textures will be saved into the CRP file. This leads to a significant waste of disk space for many roads sharing the same textures.
- `optLevel == 1`: Base textures is optimized, and only the LOD will be saved into the CRP file. This is the currently most stable implementation and have been used on CSUR roads.
- `optLevel == 2`: Base and LOD textures are optimized, and the CRP file does not contain any texture at all. This requires rebuilding LODs after loading the game and applying the textures. Rebuilding LODs for assets *sharing the exact same texture* may lead to problems.

Please refer to our `CSURLoader` repository about implementations on how external base textures for roads are applied after loading the game.

## Sample Input 
A ZIP file containing a sample set of input for three CSUR roads is also included in the repository. The file needs to be unzipped at the `C:\Users\%USERNAME%\AppData\Local\Colossal Order\Cities_Skylines\` folder. The model elements and textures for these assets are licenced under the **CC-BY-NC-ND-4.0** license (the strictest Creative Common license). **Do not share or publish the files contained in the sample file anywhere without consent from the CSUR Team.**




