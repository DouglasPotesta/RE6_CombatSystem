Modular City Alley Pack

Example scenes uses Standard Assets packs to give it a better look and playable possibilities.
Standard assets used are characters pack for firstpersoncharacter controller,
Effects pack for some camera effects and Cinematic Image Effects downloaded from unity asset store for more camera effects.
If you do not wish to import these effects please do not import the Standard Assets and Editor Folders to your project.

v1.1
- Added Props and materials:
	- Awnings: AwningBlue, AwningStipes, Sign_Beer, Sign_China
	- TrashSet: DumpsterOpen, Pallet
- Added more large wall pieces.
- Added Daytime Example scene.


Preview video: https://www.youtube.com/watch?v=VwgESKlj4rU

Tutorial video: https://youtu.be/43nzTYketOc

Webplayer playable demo Link: http://www.finwardstudios.com/AlleyPack/Alleypack_Example.html

Main Features:

- Build alleys and streets and populate them with props.
- Everything you see in the demo or videos are included.
- Package contains over 350 prefabs.
- Over 210 wall and floor pieces with swappable material possibilities.
- Over 110 high detailed unique props.
- Premade prefabs from every object with colliders.
- Snap to grid, light flicker and light optimization scripts included.
- Fast Create walls and floors with snap to grid tool. (ctrl + L to open snap options)
- Change wall and floor look easily by changing simply materials.
- Example scene included.
- Prefabs scene inlcuded.


Creating Walls/Buildings:

- Choose a wall type folder and one of the subgroups and drag a prefab to scene.
- Type A is a basic Wall.
- Type B has a concrete part on bottom.
- Type C has a concrete part on bottom and different kind on top.
- Type D is a basic wall but is higher.
- Press control + L to open snap to grid options. All pieces should snap next to eachother
with a value of 0.5, 0.5, 0.5 and angle of 90 deg. Transform gizmo needs to be set to Pivot and rotation to Local or the snap doesn't work.
- Use corners and pilars to add depth to your buildings.
- Once you have created a building you like, you can select the wall pieces
and change the Wall_ material to a different one if you like. All tiled wall materials start with Wall_ prefix.
- Use roofs folder to add metalplate roof or you can use floor pieces such as midfloor or sidewalk pieces.

Creating floors:

- Street pieces are a little bit lower than sidewalk pieces so use sidewalk pieces next to walls
or else there will be a gap between walls and the ground. Use snap to place these pieces aswell.
There is a little bit over 0.1 unit gab between street and sidewalk pieces.
- Use stonepavements to add a nicer transition between sidewalks and streets.
- Again you can select sidewalks or street pieces and change the materials. Floor materials start with Floor_ prefix.


Notes and Tips:

You need to use LINEAR COLOR SPACE to get best results and Deferred Rendering Path.
Change them from Player Settings.

When using directional light, to fix the light coming throug backside of the wall pieces, simply select all wallpieces and change
from Mesh renderer settings to Cast shadows two sided.

To change height of the street or sidewalk you can use stairs or hill pieces.
I recommend you play for a while with these pieces so you get more familiar with them and you will be able
to create much faster the type of buildings you want.

When you have a building you like and you like to use it more often, you can create an empty gameobject and drag the wall pieces to be the gameobject's children.
Then you can make a prefab out of it. Same goes for roads.

Because of static batching you don't need to combine all meshes together, unity does it for you. You just have to remember to bake occlusion data.
I recommend to bake occlusion culling for better performace and bake lightning to make the scene look better.

You can add LightFlicker script to lights you wan't to be realtime lights and adjust the parameters to get a flickering you like. You can add
light optimize script and change the parameter to shut down the light at a distance from player for better performance.

One or more textures on this 3D-model have been created with images from Textures.com.
These images may not be redistributed by default. Please visit www.textures.com for more information.

If you have any questions or want to give feedback please send email to support@finwardstudios.com
Thank you.