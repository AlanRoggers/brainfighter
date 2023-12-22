# Brain Fighter
Brain Fighter es un juego de peleas que intenta imitar la jugabilidad de algunos juegos populares del mercado de peleas pero utilizando inteligencia artificial como motor de los NPC

La rama pricipal se creo a partir de la rama manual-anim del siguiente repo https://github.com/AlanRoggers/FighterGym

> [!WARNING]
> El paquete de ML Agents esta instalado de manera manual, la primera vez se tendrá que reinstalar el paquete según el lugar donde este

Para una instalación inicial de el paquete ML Agents seguir el tutorial de instalación:
	https://unity-technologies.github.io/ml-agents/Installation/

> [!TIP]
> Usar Conda para crear el entorno virtual

Versión de Python que se utilizó y funcionó correctamente: 3.10.12

# Notas del proyecto

El personaje que se está utilizando se llama Chie Satonaka, las animaciones pertenecen al juego Persona 4 Arena Ultimax, se importaron todas las animaciones proporcionadas en la
página de SpriteResource pero puede que falten algunas o incluso algunos frames.

En total se crearon 21 SpriteSheets que contienen todas las animaciones de Chie a continuación se enumeran todas las animaciones con lo que se me ocurrió que representaban de acuerdo con los frames:

    Chie0: Iddle, Turn, Crouch, Emote1, Turn (crouch)

    Chie1: Turn (air), Go Back, Jump, Walk

    Chie2: Block, Dash, DashBack, Run

    Chie3: CrouchBlock, CrouchDamage1, Damage1, Damage2

    Chie4: Damage3, Damage4, InFloor, Recovering

    Chie5: Damage5, Damage6, Damage7, Dead, FlyingCauseThrewIt

    Chie6: Backflip, CrouchKnee, Damage8, Dash, Handler1, Xray

    Chie7: Handler2, Punch1, SomersaultKick, SpecialKick1

    Chie8: Emote2, Emote3, Kick1

    Chie9: Kick2, Kick3, SpecialKick2

    Chie10: CrouchKick1, CrouchKick2, Kick4, SpecialKick3

    Chie11: AirKick1, Emote4, EMote5, Emote6, SpecialKick4

    Chie12: AirGrab, Emote7, Emote8, Frontflip, Grab

    Chie13: Backflip, Emote9

    Chie14: Special1, Special?, Special2

    Chie15: DashAttack, Kick5

    Chie16: KneeAttack, Punch2, Special3

    Chie17: Emote10, Emote11, Emote12

    Chie18: CrouchKick3, Emote13, Emote14, Kick6

    Chie19: Emote15, Emote16

    Chie20: Entry

    Chie21: Emote17, Emote18, Emote19, Stomp

# Animaciones consideradas para utilizar en la tesina
    > Iddle
	> Turn (Normal, Air and crouch)
	> Crouch
	> Jump
	> Walk
	> Go Back
	> Run (Iddle1 puede ser el primer frame, también puede ser que el dash de Chie6 sea parte de Run)
	> Dash (Revisar Chie2 y Chie6)
	> Dash Back
	> Damage2
	> CrouchDamage1
	> Recovering (El frame 3 tal vez se pueda usar para cuando regresa al piso estando en el aire)
	> InFloor
	> Dead
	> Punch1
	> SpecialKick1
	> SomersaultKick
	> Kick1
	> Kick2
	> Kick3
	> SpecialKick2 (Opcional)
	> SpecialKick3
	> CrouchKick1 (Tambien esta un CrouchKick2 implementar el más fácil)
	> Kick4
	> SpecialKick4
	> AirKick1
	> Special2 (La animación esta medio rara)
	> DashAttack
	> Punch2
	> Entry (Frame 14, 15 y 16 se pueden usar para aterrizar, tal vez se puede complementar con el frame 3 de recovering)
	> Stomp

> [!NOTE]
> Estas animaciones son las que yo esperaría alcanzar a implementar, sin embargo, esta lista está sujeta a cambios
