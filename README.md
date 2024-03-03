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

## Notas del proyecto

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

## Animaciones consideradas para utilizar en la tesina
    > Iddle (Listo)
	> Turn (Normal, Air and crouch) (Listo)
	> Crouch (Listo)
	> Jump (Listo)
	> Walk (Listo)
	> Go Back (Listo)
	> Run (Listo) (Iddle1 puede ser el primer frame, también puede ser que el dash de Chie6 sea parte de Run)
	> Dash (Listo) (Revisar Chie2 y Chie6; Me parece que el dash de Chie6 es para el aire)
	> Dash Back (Listo)
	> Damage2 (Listo)
	> CrouchDamage1 (Listo)
	> Recovering (Listo) (El frame 3 tal vez se pueda usar para cuando regresa al piso estando en el aire)
	> InFloor (Listo)
	> Dead (Listo)
	> Punch1 (Listo)
	> SpecialKick1 (Listo)
	> SomersaultKick (Listo)
	> Kick1 (Listo)
	> Kick2 (Listo)
	> Kick3 (Listo)
	> SpecialKick2 (Opcional) (Omitido porque no me gustó la animación)
	> SpecialKick3 (Listo)
	> CrouchKick1 (Listo) (Tambien esta un CrouchKick2 implementar el más fácil; CrouchKick1 implementado)
	> Kick4 (Listo)
	> SpecialKick4 (Listo)
	> AirKick1 (Listo)
	> Special2 (Si se va a usar arreglarlo primero) (La animación esta medio rara) (Parece ser que a mitad de las patadas se tiene que ciclar para lograr un buen bucle, no lo hice pero el clip ya esta ahí)
	> DashAttack (Listo)
	> Punch2 (Listo)
	> Entry (Creada la animacion "Land" a partir de la recomendación de los frames) (Frame 14, 15 y 16 se pueden usar para aterrizar, tal vez se puede complementar con el frame 3 de recovering)
	> Stomp

> [!NOTE]
> Estas animaciones son las que yo esperaría alcanzar a implementar, sin embargo, esta lista está sujeta a cambios

## Transiciones entre animaciones
    Iddle:
        > Turn normal
        > Start Crouching
        > StartJumping
        > StartWalking
        > StartGoingBackwards
        > StartRunning
        > Dash
        > DashBack
        > Damage
        > Low Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    Turn Normal:
        > Iddle
        > StartCrouching
        > StartJumping
        > StartWalking
        > StartGoinBackwards
        > Dash
        > DashBack
        > Damage
        > Low Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    StartCrouching:
        > Crouch
    Crouch:
        > Iddle
        > StartJumping
    StartJumping:
        > Jump
    Jump:
        > StartFalling
    StartFalling:
        > Fall
    Fall:
        > Iddle
        > TurnNormal
        > StartCrouching
        > StartJumping
        > StartWalking
        > StartGoingBackwards
        > Low Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    StartWalking:
        > Walk
    Walk:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > StartGoingBackwards
        > StartRunning
        > Dash
        > Damage
        > Low 
        > Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    StartGoingBackwards:
        > GoingBackwards
    GoingBackwards:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > StartWalking
        > DashBack
        > Damage
        > Low Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    StartRunning:
        > Run
    Run:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > Walk
        > StartGoingBackwards
        > Damage
        > Low Punch / Kick
        > Middle Punch / Kick
        > Hard Punch / Kick
    Dash:
        > Iddle
        > Turn Normal
        > Walk
        > GoingBackwards
    DashBack:
        > Iddle
        > Turn Normal
        > Walk
        > GoingBackwards
    Damage:
        > Iddle
    LowPunch:
        > ChainLowPunch
        > Damage
    ChainLowPunch:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > StartWalking
        > StartGoingBackwards
        > StartRunning
        > Dash
        > DashBack
        > Damage
        > Middle Punch
    MiddlePunch:
        > ChainMiddlePunch
        > Damage
    ChainMiddlePunch:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > StartWalking
        > StartGoingBackwards
        > StartRunning
        > Dash
        > DashBack
        > Damage
        > Hard Punch
    HardPunch:
        > ChainHardPunch
        > Damage
    ChainHardPunch:
        > Iddle
        > Turn Normal
        > StartCrouching
        > StartJumping
        > StartWalking
        > StartGoingBackwards
        > StartRunning
        > Dash
        > DashBack
## Mecánicas del juego
    > Un golpe -> débil, medio y fuerte
    > Una patada -> débil, media y fuerte
    > Dos ataques especiales
    > Golpe en el aire
    > Golpe agachado
    > Capacidad para bloquear ataques
    > Caminar
    > Saltar
    > Correr
    > Esquivar
    > Cada golpe debe desplazar al enemigo un poco (inercia)

Además de los puntos enumerados, el jugador podrá encadenar ataques (combos) simples

    > Golpe débil -> Golpe medio -> Golpe fuerte -> Especial 1
    > Patada débil -> Patada media -> Patada fuerte -> Especial 2

Para lograr los especiales va ser necesario que se ejecuten la secuencia de tres golpes / patadas enumeradas anteriormente y además apretar un botón adicional que detonará el ataque especial correspondiente. 

> [!NOTE]
> Desde el estado Iddle, Caminando, Corriendo debe ser posible ejecutar los golpes y las patadas de 
  manera individual, pero cada golpe tiene un tiempo de recuperación, por eso las cadenas de golpes
  por ejemplo si se usa golpe débil y golpe fuerte, el personaje se debe sentir "tonto" pues el 
  tiempo de recuperación tiene que hacer que los golpes no sean instantaneos a menos que se ejecuten
  como la secuencia correcta


## Tareas (28/12/2023)
****Acciones****

**Motion**

- [x] Caminar: Movimiento horizontal del personaje. Se puede hacer cuando este en el suelo en estado Iddle. Las siguientes acciones interrumpen el comportamiento de esta acción:
- Agacharse
- Saltar (Solo interrumpe animación)
- Dash
- DashBack
- Correr (No interrumpe, modifica)
- Acción Damage

- [x] Saltar: Movimiento vertical del personaje. Se puede hacer cuando este en el suelo en estado Iddle o caminando. Las siguientes acciones interrumpen el comportamiento de esta acción:
- Acción Damage
- Acción Emote (Solo animación)

- [x] Agacharse: Es una ilusión generada principalmente con ayuda de la animación, esta acción activa/desactiva el hitbox correspondiente. Se puede hacer cuando este en el suelo en cualquier estado que no sea un estado de una acción Damage. Las siguientes acciones interrumpel el comportamiento de esta acción:
- Acción Damage
- Acción Emote (Solo animación)

- [x] Correr: Movimiento horizontal acelerado del personaje. Se puede hacer cuando Chie esta en el suelo en estado Iddle y Caminando (Solo caminando hacía adelante). Las siguientes acciones interrumpen el comportamiento de esta acción;
- Saltar (Solo animación)
- Agacharse
- Dash

- [x] Dash: Movimiento que permite desplazarse rapidamente y ser invulnerable por cierto periodo de tiempo. Se puede hacer cuando Chie esta en el suelo en estado Iddle, Caminando y Corriendo. Ninguna acción puede interrumpir a esta.

- [x] DashBack: Movimiento que permite desplazarse rapidamente hacia atrás. Se puede hacer cuando Chie esta en el suelo en estado Iddle o Caminando (hacia atrás). Ninguna acción puede interrumpir esta

**Damage**
- [x] Golpes bajos medios y fuertes: Se pueden realizar cuando Chie esta en estado Iddle, caminando o corriendo. Cada golpe aporta un punto a la secuencia de golpes. Al final de cada golpe hay un tiempo de recuperación que no permite hacer ninguna acción Damage. A mitad de animación de cada golpe es posible realizar el ataque que sigue en la secuencia: Low -> Middle -> Hard, si esa secuencia no se respeta, los golpes terminarán y se aplicara el tiempo de recuperación. Ninguna acción puede interrumpir a esta.

- [x] Golpe especial: Se puede realizar cuando Chie haya completado la secuencia de tres golpes de manera correcta. Ninguna acción puede interrumpir a esta

- [x] Patadas bajas, medias y fuertes: Se pueden realizar cuando Chie esta en estado Iddle, caminando o corriendo. Cada patada aporta un punto a la secuencia de patadas. Al final de cada patada hay un tiempo de recuperación que no permite hacer ninguna acción Damage. A mitad de animación de cada patada es posible realizar el ataque que sigue en la secuencia: Low -> Middle -> Hard, si esa secuencia no se respeta, las patadas terminarán y se aplicará el tiempo de recuperación. Ninguna acción puede interrumpir a esta. Si puede tomar daño estando en este estado

- [x] Patada especial: Se puede realizar cuando Chie haya completado la secuencia de tres patadas de manera correcta. Ninguna acción puede interrumpir a esta. No puede tomar daño estando en este estado

- [x] Patada agachado: Esta patada se puede hacer cuando Chie esta agachada. Ninguna acción puede interrumpir a esta.

- [x] Bloqueo: Mecánica para bloquear golpes, los golpes bloqueados pueden
tener fisicas menores para hacer notar el contacto de los golpes.

- [x] Bloqueo agachado: La misma tarea que el bloqueo normal

- [x] Sistema de golpes - bloqueos: Algunos golpes solo se podrán bloquear
de manera normal o agachado.

- [x] Vida: vida de cada personaje junto con la implementación del daño de
cada golpe o patada

__Nota: Tomar en cuenta que todo lo que no especifique que no puede tomar daño o que es invulnerable es considerado como que si se le puede hacer daño__


Actualización del Código a Clean Architecture

Acciones reales: Son acciones ejecutadas por una entrada del usuario

Acciones virtuales: Son acciones que se ejecutan debido a que se cumplio una condición que no es una entrada del
                    usuario

La clase InputManager es responsable de validar si es posible ejecutar la acción real requerida por la entrada del 
usuario pero no las ejecuta.

La clase abstracta Character es responsable de validar las acciones virtuales y las ejecuta

Las clases que deriben de Character son responsables de ejecutar las acciones reales.

FixedUpdate se utiliza para ejecutar acciones que tengan que ver con movimientos y tambien para comprobar colisiones
que no tengan que ver con ataques

Update se utiliza para ejecutar los ataques porque la ejecución de un ataque es una corrutina



Tuneo de parametros de los golpes:

Cada golpe puede proporcionar su propio tiempo de stun al enemigo, este tiempo se aplica al final del tiempo de animación del daño, como no queremos dar tanta ventaja a esto los valores tendrán que ser muy 
cercanos. La cantidad de daño que se realice debe elevar la cantidad de tiempo que se debe esperar para poder dar otro ataque y tambien la cantidad de tiempo que el personaje esta stuneado

LowPunch:
    Daño: 3
    hitStun: 0.2 -> nuevo valor 0.05
    hitCD: 0.1 -> 0.1

MiddlePunch:
    Daño: 5
    hitStun: (nuevos valores) 0.2
    cd: (nuevos valores) 0.2

HardPunch:
    Daño: 7
    hitStun: 0.5 -> 0.3
    cd: 0.8 -> nuevo valor 0.4

SpecialPunch:
    Daño: 10
    hitStun: 1 -> nuevo valor 0.45
    cd: 1 -> nuevo valor 0.6

LowKick:
    Daño: 4
    hitStun: 0.35 -> nuevo valor 0.1
    cd: 0.3 -> nuevo valor 0.15

MiddleKick:
    Daño: 6 
    hitStun: 0.3 -> nuevo valor 0.25
    cd: 0.55 -> 0.25

HardKick:
    Daño: 8
    hitStun: 0.5 -> nuevo valor 0.4
    cd: 0.85 -> nuevo valor 0.45

SpecialKick:
    Daño: 11
    hitStun: 1 -> nuevo valor 0.6
    cd: 1 -> nuevo valor 0.8
