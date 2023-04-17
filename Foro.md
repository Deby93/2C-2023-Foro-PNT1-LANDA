# Foro 📖

## Objetivos 📋
Desarrollar un sistema, que permita la administración general de un Foro (de cara a los administradores): Usuarios Miembros, Administradores, Pregunta, Respuesta, MeGusta, etc., como así también, permitir a los usuarios puedan navegar el foro y realizar preguntas y/o respuestas.
Utilizar Visual Studio 2019 preferentemente y crear una aplicación utilizando ASP.NET MVC Core (versión a definir por el docente 3.1 o 6.0).

<hr />

## Enunciado 📢
La idea principal de este trabajo práctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relevó e identificó la información aquí contenida. 
A partir de este momento, deberán comprender lo que se está requiriendo y construir dicha aplicación, 

Deben recopilar todas las dudas que tengan y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, él nos ayudará a conseguir la información ya un poco más procesada. 
Es importante destacar, que este proceso, no debe esperar a ser en clase; es importante, que junten algunas consultas, sea de índole funcional o técnicas, en lugar de cada consulta enviarla de forma independiente.

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relación del paciente con Turno es 1:1 o 1:N?

2.Está bien que encaremos la validación del turno activo, con una propiedad booleana en el Turno?

<hr />

### Proceso de ejecución en alto nivel ☑️
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/).
 - Adicionar todos los modelos dentro de la carpeta Models cada uno en un archivo separado.
 - Especificar todas las restricciones y validaciones solicitadas a cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Crear las relaciones entre las entidades
 - Crear una carpeta Data que dentro tendrá al menos la clase que representará el contexto de la base de datos DbContext. 
 - Crear el DbContext utilizando base de datos en memoria (con fines de testing inicial). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades en el DbContext.
 - Crear el Scaffolding para permitir los CRUD de las entidades al menos solicitadas en el enunciado.
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Realizar un sistema de login con al menos los roles equivalentes a <Usuario Cliente> y <Usuario Administrador> (o con permisos elevados).
 - Si el proyecto lo requiere, generar el proceso de registración. 
 - Un administrador podrá realizar todas tareas que impliquen interacción del lado del negocio (ABM "Alta-Baja-Modificación" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente> sólo podrá tomar acción en el sistema, en base al rol que tiene.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos del lado de los permisos.
 - Todo lo referido a la presentación de la aplicaión (cuestiones visuales).
 
 Nota: Para la pre-carga de datos, las cuentas creadas por este proceso, deben cumplir las siguientes reglas:
 1. La contraseña por defecto para todas las cuentas pre-cargadas será: Password1!
 2. El UserName y el Email deben seguir la siguiente regla:  <classname>+<rolname si corresponde diferenciar>+<indice>@ort.edu.ar Ej.: cliente1@ort.edu.ar, empleado1@ort.edu.ar, empleadorrhh1@ort.edu.ar

<hr />

## Entidades 📄

- Persona
- Miembro
- Administrador
- Categoria
- Entrada
- Pregunta
- Respuesta
- Like (MeGusta)


`Importante: Todas las entidades deben tener su identificador unico. Id`

`
Las propiedades descriptas a continuación, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como así también las restricciones.
`

**Persona**
```
- UserName
- Password
- Email
- FechaAlta
```

**Administrador**
```
- Nombre
- Apellido
- Email
- FechaAlta
- Password
```

**Miembro**
```
- Nombre
- Apellido
- Email
- Telefono
- FechaAlta
- Password
- Entradas
- Preguntas
- Respuestas
- PreguntasYRespuestasQueMeGustan
```

**Categoria**
```
- Nombre
- Entradas
```

**Entrada**
```
- Titulo
- Fecha
- Categoria
- Miembro
- Preguntas
- Privada
- MiembrosHabilitados
```

**Pregunta**
```
- Descripcion
- Entrada
- Respuestas
- Miembro
- Fecha
- Activa
```

**Respuesta**
```
- Descripcion
- Pregunta
- Miembro
- Fecha
- Reacciones (colección de Likes, "MeGusta")
```


**Reaccion**
```
- Fecha
- MeGusta
- Respuesta
- Miembro
```

**NOTA:** aquí un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades ⌨️
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

**Administrador**
- Un administrador, solo puede crear nuevas categorias.
- Sacar un listado de cantidad de Entradas por categorias.
- Los administradores del Foro, deben ser agregados por otro Administrador.
	- Al momento, del alta del Administradores, se le definirá un username y la password será definida por el sistema.
    - También se le asignará a estas cuentas el rol de Administrador.

**Miembro**
- Puede auto registrarse.
- La autoregistración desde el sitio, es exclusiva para los usuarios miembros. Por lo cual, se le asignará dicho rol.
- Los miembros pueden navegar por el foro.
- Pueden crear Entradas, y automaticamente deberán generar una pregunta.
    - Pueden desactivar una pregunta en cualquier momento. Si está inactiva, no se dejará de ver, solo impedirá que carguen nuevas respuestas otros miembros.
    - No se puede cargar una respuesta de una pregunta del mismo miembro. Esta acción, debe estar deshabilitada.
- Puede crear nuevas categorias.
    - Antes de crearla, se le propondrá un listado de categorias ya existentes en orden alfabetico.
- A cualquier respuesta, un miembro (que no es el autor de la respuesta), puede poner Like (MeGusta), Dislike (NoMeGusta) o resetearlo (Quita la reacción a dicha respuesta).

**Reaccion**
- La reacción a una respuesta será validandola con las 3 posibilidades.
- Al quitar la reacción, no se desea guardar registro previo de la misma.
    - Un miembro, solo puede quitar las reacciones que uno mismo ha cargado.

**Entrada**
- Al generar una entrada por un miembro, quedarán los datos básicos asignados, como ser fecha, el miembro que la creó, etc.
    - La categoria puede ser una existente o una nueva que quiera crear en el momento.
- La entrada, creará junto con está la primer pregunta, que también, será este miembro el dueño.
    - Las entradas, listarán las preguntas en orden cronologico ascendente.
    - Estas preguntas, mostrarán al costado la cantidad de respuestas que recibieron.
- La entrada puede ser privada, en tal caso, se listará en el foro, con su titulo, pero solo miemrbros habilitados, podrán acceder a las preguntas y respuestas para interactuar.
    - El creador de la entrada, no necesita ser habilitado explicitamente.
    - Los miembros no habilitados pueden solitiar que se los habilite.
    - Un miembro autor de la entrada, podrá ver un listado de miembros que quieren ser habilitados, y habilitarlos uno por uno.
- Al acceder a una entrada, se deberá mostrar las preguntas, en orden descendente por cantidad de likes recibidos.

**Pregunta**
- Mientras que una pregunta esté activa, otros miembros, podrán dar respuestas a las preguntas.
- La entrada, puede tener más preguntas del mismo miembro, como asi también, recibir más preguntas de otros miembros.
- Se visualizará las respuestas en orden cronológico ascendente, al acceder a cada pregunta.
    - La respuesta con más likes, se deberá destacar visualmente. Ejemplo, en un recuadro verde. 
    - La respuesta con más dislikes, se deberá destacar visualmente. Ejemplo, en un recuadro rojo.

**Respuesta**
- Las respuestas será cargadas por miembros, que no son los creadores de la pregunta.
- Podrán recibir reacciones.

**Reacciones**
- Las reacciones, acerca de las respuestas, no pueden ser realizadas por los mismos autores de las respuestas. 


**Aplicación General**
- El foro, mostrará los encabezados en la home:
    - Un listado de las ultimas 5 entradas cargadas más recientemente. 
    - Un top 5, de Entradas con más preguntas y respuestas.
    - Un top 3, de los miembros con más entradas cargadas en el ultimo mes. 
- Se debe ofrecer también, navegación por categorias. 
- Los miembros no pueden eliminar las entradas, ni deshabilitarse.
- Solo los administradores pueden eliminar entradas, con sus preguntas y respuestas, en caso de que estas tengan contenido inapropiado. 
- Los accesos a las funcionalidades y/o capacidades, debe estar basada en los roles que tenga cada individuo.
