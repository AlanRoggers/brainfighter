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
    > Iddle a
        > Caminar
        > Retroceder
        > Voltear
        > Saltar
        > Agacharse
        > Correr
        > Dash
        > DashBack
        > Golpeado
        > Morir
        > Golpe 1
        > Golpe 2
        > Patada especial 1
        > Voltereta con ataque
        > Patada 1
        > Patada 2
        > Patada 3
        > PAtada 4
        > Patada especial 3
        > Patada especial 4
        > Ataque con Dash
    > Saltar
        > Voltear en el aire
        > Golpeado
        > Patada aerea
        > Land
    > Agachado
        > Iddle
        > Voltear agachado
        > Saltar
        > Golpeado agachado
        > Patada agachado

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
- Damage

- [x] Dash: Movimiento que permite desplazarse rapidamente y ser invulnerable por cierto periodo de tiempo. Se puede hacer cuando Chie esta en el suelo en estado Iddle, Caminando y Corriendo. Ninguna acción puede interrumpir a esta.

- [x] DashBack: Movimiento que permite desplazarse rapidamente hacia atrás. Se puede hacer cuando Chie esta en el suelo en estado Iddle o Caminando (hacia atrás). Ninguna acción puede interrumpir esta

**Damage**
- [x] Golpes bajos medios y fuertes: Se pueden realizar cuando Chie esta en estado Iddle, caminando o corriendo. Cada golpe aporta un punto a la secuencia de golpes. Al final de cada golpe hay un tiempo de recuperación que no permite hacer ninguna acción Damage. A mitad de animación de cada golpe es posible realizar el ataque que sigue en la secuencia: Low -> Middle -> Hard, si esa secuencia no se respeta, los golpes terminarán y se aplicara el tiempo de recuperación. Ninguna acción puede interrumpir a esta.

- [x] Golpe especial: Se puede realizar cuando Chie haya completado la secuencia de tres golpes de manera correcta. Ninguna acción puede interrumpir a esta

- [x] Patadas bajas, medias y fuertes: Se pueden realizar cuando Chie esta en estado Iddle, caminando o corriendo. Cada patada aporta un punto a la secuencia de patadas. Al final de cada patada hay un tiempo de recuperación que no permite hacer ninguna acción Damage. A mitad de animación de cada patada es posible realizar el ataque que sigue en la secuencia: Low -> Middle -> Hard, si esa secuencia no se respeta, las patadas terminarán y se aplicará el tiempo de recuperación. Ninguna acción puede interrumpir a esta. Si puede tomar daño estando en este estado

- [x] Patada especial: Se puede realizar cuando Chie haya completado la secuencia de tres patadas de manera correcta. Ninguna acción puede interrumpir a esta. No puede tomar daño estando en este estado

- [ ] SomersaultKick: Es una patada combinada con un Dash hacia atrás. Este ataque se puede hacer cuando Chie esta en estado Iddle o caminando hacía atrás. Ninguna acción puede interrumpir a esta.

- [ ] Patada en el aire: Esta patada se puede hacer cuando Chie no esta en el suelo. Ninguna acción puede interrumpir a esta.

- [ ] Patada agachado: Esta patada se puede hacer cuando Chie esta agachada. Ninguna acción puede interrumpir a esta.


- [ ] Revisar que todas las corrutinas que se ejecutan solo se ejecuten una sola vez, esto causa demasiados bugs si no se cuida

__Nota: Tomar en cuenta que todo lo que no especifique que no puede tomar daño o que es invulnerable es considerado como que si se le puede hacer daño__


# Cosas que debo recordar
La animación de Punch2End tiene el frame Run10, Emote1_1 y Emote1_0 ya que así no se ve tan cortada la animación
