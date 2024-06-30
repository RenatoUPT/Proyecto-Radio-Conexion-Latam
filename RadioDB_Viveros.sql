USE [master]
GO
/****** Object:  Database [RadioDB]    Script Date: 30/06/2024 13:05:34 ******/
CREATE DATABASE [RadioDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RadioDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RadioDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RadioDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RadioDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RadioDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RadioDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RadioDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RadioDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RadioDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RadioDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RadioDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RadioDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RadioDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RadioDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RadioDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RadioDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RadioDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RadioDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RadioDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RadioDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RadioDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RadioDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RadioDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RadioDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RadioDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RadioDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RadioDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RadioDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RadioDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RadioDB] SET  MULTI_USER 
GO
ALTER DATABASE [RadioDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RadioDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RadioDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RadioDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RadioDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RadioDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RadioDB', N'ON'
GO
ALTER DATABASE [RadioDB] SET QUERY_STORE = OFF
GO
USE [RadioDB]
GO
/****** Object:  Table [dbo].[Anuncios]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Anuncios](
	[idAnuncio] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [nvarchar](255) NOT NULL,
	[subtitulo] [nvarchar](255) NULL,
	[contenido] [text] NOT NULL,
	[fechaPublicacion] [datetime] NOT NULL,
	[idUsuario] [int] NULL,
	[idCategoria] [int] NULL,
	[idImagenPrincipal] [int] NULL,
	[idVideoPrincipal] [int] NULL,
	[estado] [nvarchar](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idAnuncio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[idCategoria] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnlacesRelacionados]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnlacesRelacionados](
	[idEnlace] [int] IDENTITY(1,1) NOT NULL,
	[idAnuncio] [int] NOT NULL,
	[url] [nvarchar](255) NOT NULL,
	[descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[idEnlace] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Eventos]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Eventos](
	[idEvento] [int] IDENTITY(1,1) NOT NULL,
	[nombreevento] [nvarchar](100) NOT NULL,
	[descripcion] [nvarchar](255) NOT NULL,
	[fechaevento] [datetime] NOT NULL,
	[lugar] [nvarchar](100) NOT NULL,
	[organizador] [nvarchar](100) NOT NULL,
	[estado] [nvarchar](1) NOT NULL,
	[capacidad] [int] NOT NULL,
	[idCategoria] [int] NOT NULL,
 CONSTRAINT [PK_Eventos] PRIMARY KEY CLUSTERED 
(
	[idEvento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Imagenes]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Imagenes](
	[idImagen] [int] IDENTITY(1,1) NOT NULL,
	[url] [nvarchar](255) NOT NULL,
	[descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[idImagen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramacionSemanal]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramacionSemanal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Dia] [nvarchar](50) NOT NULL,
	[Hora] [nvarchar](50) NOT NULL,
	[NombrePrograma] [nvarchar](100) NOT NULL,
	[idUsuario] [int] NOT NULL,
	[idCategoria] [int] NOT NULL,
 CONSTRAINT [PK__Programa__3214EC07B8CCC718] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[apellido] [nvarchar](100) NOT NULL,
	[correo] [nvarchar](100) NULL,
	[idRol] [int] NULL,
	[contrasena] [nvarchar](15) NOT NULL,
	[estado] [nvarchar](1) NULL,
 CONSTRAINT [PK__Usuarios__645723A6F89C9A34] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Videos]    Script Date: 30/06/2024 13:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Videos](
	[idVideo] [int] IDENTITY(1,1) NOT NULL,
	[url] [nvarchar](255) NOT NULL,
	[descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[idVideo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Anuncios] ON 

INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (2, N'¿Timoteo vs. Bowser? Una imagen de lucha libre peruana se viraliza en redes sociales', N'¿Es una imagen real? Sí y es de una empresa de lucha libre peruana llamada Gladiadores.', N'Gladiadores es una empresa que nació en 2018 de la mano de una agrupación de luchadores locales como Mansilla, Rafael de Salamanca y Reptil.

Realiza eventos una vez al mes en el llamado Danzak Arena ubicado en el distrito de Surquillo. En este lugar se produjo precisamente este combate entre Timoteo y Bowser.

El sábado 20 de mayo se llevó a cabo el evento: “Gladiadores 33: CHAU DANZAK”. Los precios rondaban entre los 60 y 100 soles.

Las imágenes se viralizaron en Twitter, desde donde se viralizaron a varias partes del mundo.', CAST(N'2024-05-25T00:00:00.000' AS DateTime), 1, 2, 14, 18, N'I')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (3, N'Japón: dos hombres trabajaron con Amazon para robar el nuevo Zelda antes de su lanzamient', N'“The Legend of Zelda: Tears of the Kingdom” ha causado gran expectativa y esta historia lo demuestra.', N'Dos hombres, de 21 y 24 años, tuvieron la misma idea para hacerse del nuevo Zelda.

Los jóvenes consiguieron trabajo en una empresa subcontratada por Amazon, en la que empezaron a laborar meses antes del esperado lanzamiento del 12 de mayo.

De repente, dejaron de ir al trabajo cuando la fecha de lanzamiento de “The Legend of Zelda: Tears of the Kingdom” estaba muy cerca.

Un supervisor llamó a la casa de uno de los hombres, donde su madre le informó que “estaba jugando videojuegos”. Posteriormente, el joven admitió que solo postuló al trabajo para jugar el último Zelda antes que todos.

El otro joven también admitió un plan similar, aunque también robó varios objetos promocionales con la intención de venderlos.

Ambos hombres fueron liberados, con la condición de retornar los objetos robados y pagar una compensación.  ', CAST(N'2024-05-25T00:00:00.000' AS DateTime), NULL, 2, 8, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (4, N'Un hombre asaltó una tienda con una pistola de Nintendo', N'Insólito asalto en Carolina del Sur utilizando un accesorio de Nintendo.', N'Insólito. Un hombre en Carolina del Sur, Estados Unidos, asaltó una tienda utilizando una pistola de Nintendo.

El departamento de policía de York County informa que David Joseph Dalesandro (25) ingresó a una tienda de la cadena Sharon Kwik Stop con el “arma”.

Para hacerla parecer real, el arma fue pintada de negro, contrastando con su color original: plomo.

Dalesandro realizó el asalto la tarde del 30 de mayo utilizando una máscara, peluca y una polera, mostrando al empleado a carga “su arma”, exigiendo dinero. En total se llevó 300 dólares, indica el reporte policial.

Policías encontraron al hombre en un estacionamiento cercano en posesión del "arma", un accesorio del Nintendo original (NES).', CAST(N'2024-05-29T00:56:02.627' AS DateTime), NULL, 2, 9, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (5, N'Streamer se rapó hasta las cejas por donación de 2500 dólares, pero usuario nunca envió el dinero', N'El donador del streamer terminó eliminando su cuenta y bloqueando de todas las redes sociales al creador de contenido, quien nunca vio el monto prometido.', N'Un streamer de Twitch se ha vuelto viral luego de que un suceso en una de sus transmisiones en vivo se diera a conocer por otras redes sociales.

Lacy, con 20 años, se estaba haciendo un nombre gracias a sus transmisiones de Fortnite en la plataforma, tanto que ha acumulado 83 mil seguidores en su canal. Pero el pasado fin de semana, en medio de un streaming que se podía alargar gracias a las donaciones de sus fanáticos, un usuario le pidió cortarse todo el cabello y hasta cejas por 2500 dólares, un monto que terminó siendo una estafa para él.

Mientras realizaba la transmisión en vivo, un usuario le envió 251 092 bits, una moneda propia de Twitch que equivale a 2510 dólares para que el jugador se afeitara toda la cabeza.,

Al ver el monto llegando a su cuenta, Lacy no lo pensó dos veces y aceptó.

Con una maquina rasuradora, el creador de contenidos se rapó todo el cabello e incluso las cejas para cumplir el reto.

Lastimosamente para él, el usuario pidió reembolso de toda la donación a Twitch, la cual fue aceptada por la plataforma.

“Me hiciste afeitarme la cabeza”, dijo Lacy en el streaming. “Me afeité las malditas cejas. ¿y haces un reembolso?”. La molestia del jugador era más que evidente.

Y en forma de burla, horas después el mismo usuario reapareció y envió un dólar de agradecimiento. ', CAST(N'2024-05-29T00:57:46.160' AS DateTime), NULL, 1, 10, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (6, N'Ladrones roban cartas coleccionables valorizadas en más de 300 mil dólares', N'Las cartas coleccionables pueden llegar a costar hasta millones de dólares, dependiendo de su rareza.', N'Las cartas pueden valer mucho para los coleccionistas. La semana pasada conocimos que Post Malone pagó más de 2 millones por la carta del Anillo Único en Magic: The Gathering.

La convención Gen Con 2023 en Indianápolis, Estados Unidos, ha sido el escenario de un robo de cartas coleccionables valorizadas en más de 300 mil dólares.

El robo ocurrió en los días preparativos para la convención en el Indiana Convention Center, con WRTV informando que los sospechosos se llevaron un palé de cartas hacia una ubicación no conocida.

La policía indica que las cartas están valorizadas en “más de 300 mil dólares”.

Todavía no se tiene información de qué compañía fue la afectada ni de qué juego de cartas se trata.

El medio especializado Dicebreaker informó que el robo no tiene que ver con Lorcana, el nuevo juego de cartas con personajes de Disney que ha causado gran expectativa, incluyendo colas de 16 horas para comprarlas.

Gen Con 2023 se realizó entre el 3 y el 6 agosto y se estima que atrajo a unas 65 mil personas.', CAST(N'2024-05-29T01:15:15.130' AS DateTime), NULL, 1, 11, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (8, N'Diseñadora hizo creer a medios colombianos que fue crucial en "El niño y la garza"', N'En sus primeras declaraciones, Geraldine Fernández aseguró haber realizado 25 mil fotogramas de la película de Hayao Miyazaki.', N'La colombiana Geraldine Fernández se convirtió brevemente en una estrella de los medios de su país.

La ilustradora de Barranquilla fue calificada de “ganar el Globo de Oro” tras afirmar que fue crucial en la elaboración de “El niño y la garza” del conocido Studio Ghibli con Hayao Miyazaki.

Fernández dio una entrevista a un medio digital, donde contaba sus proezas, afirmando que realizó unos 25 mil fotogramas para la película ganadora del Globo de Oro.

Tras ser entrevistada por varios medios colombianos, las redes sociales empezaron a cuestionar las afirmaciones de Fernández, que no aparecía en los créditos de una película en la que supuestamente fue una de las principales artistas.

Finalmente, Fernández reconoció que su participación en “El niño y la garza” no fue tan importante y que “trabajó en un par de escenas”.

En Mañanas Blu de Blu Radio, explicó la “confusión” sobre los supuestos 25 mil fotogramas que aseguró realizar.

"Sí, porque 25 mil fotogramas son casi entre 30 y 40 minutos de video. Ten en cuenta que un segundo de animación son 24 fotogramas. 25 mil fotogramas serían aproximadamente 30 o 40 minutos de video. Y junto con un grupo hice parte de un par de escenas en el cual era la composición de 25 mil fotogramas. Pero no hice los 25 mil fotogramas", afirmó.

Sobre su ausencia en los créditos de la película, indicó que su participación era considerada como parte de una “casa de soporte de animación” al ser una freelancer.', CAST(N'2024-05-29T01:20:20.363' AS DateTime), NULL, 5, 12, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (9, N'''X-Men ''97'': Un viaje a la nostalgia, la acción y el drama de la serie clásica, con un futuro prometedor', N'Los mutantes que tanto hemos amado están de vuelta en Disney+ con emocionantes aventuras. Esta es nuestra reseña sin spoilers de la primera temporada de X-Men ''97.', N'Fue a principios de la década de 1990 cuando X-Men llegó a la televisión como serie animada, a través de la señal de Fox Kids. El acontecimiento no fue cualquier cosa, ya que hablamos de una serie de culto, que marcó en definitiva lo que el fandom espera de cualquier futura adaptación del equipo de mutantes liderados por Charles Xavier.

Casi tres décadas después, Cíclope, Jean Grey, Wolverine y otros miembros del equipo regresan para continuar su legado en X-Men ''97, la nueva serie animada de Marvel. Esta propuesta no solo evoca una poderosa sensación de nostalgia para los fans más veteranos, sino que también está diseñada para cautivar a una nueva generación de seguidores.', CAST(N'2024-05-29T10:12:24.043' AS DateTime), NULL, 5, 13, NULL, N'A')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (10, N'Ejemplo Noticia 2.1', N'Sub2.1', N'descripcion', CAST(N'2024-05-29T10:12:57.270' AS DateTime), NULL, 1, 6, 8, N'I')
INSERT [dbo].[Anuncios] ([idAnuncio], [titulo], [subtitulo], [contenido], [fechaPublicacion], [idUsuario], [idCategoria], [idImagenPrincipal], [idVideoPrincipal], [estado]) VALUES (11, N'test1', N'test', N'test', CAST(N'2024-05-29T10:19:23.090' AS DateTime), NULL, 1, 5, 12, N'I')
SET IDENTITY_INSERT [dbo].[Anuncios] OFF
GO
SET IDENTITY_INSERT [dbo].[Categorias] ON 

INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (1, N'Redes Sociales')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (2, N'Geekos')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (3, N'Evento')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (4, N'Streaming')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (5, N'Internacional')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (6, N'Redes Nacionales')
INSERT [dbo].[Categorias] ([idCategoria], [nombre]) VALUES (7, N'Litio')
SET IDENTITY_INSERT [dbo].[Categorias] OFF
GO
SET IDENTITY_INSERT [dbo].[Eventos] ON 

INSERT [dbo].[Eventos] ([idEvento], [nombreevento], [descripcion], [fechaevento], [lugar], [organizador], [estado], [capacidad], [idCategoria]) VALUES (1, N'MARIO PARTY', N'asdasdjhaskdjahjkhljkyrukghjkghjkghjkghjkghjkghjk', CAST(N'2024-06-25T12:24:00.000' AS DateTime), N'Teatro Tacna', N'asdas', N'I', 50, 1)
INSERT [dbo].[Eventos] ([idEvento], [nombreevento], [descripcion], [fechaevento], [lugar], [organizador], [estado], [capacidad], [idCategoria]) VALUES (1002, N'EVENTO MARIIILOKO', N'En este evento podras socializar con toda la chaviza', CAST(N'2024-06-27T14:22:00.000' AS DateTime), N'Teatro Municipal', N'Renato Chambilla Mardinez', N'A', 20, 3)
SET IDENTITY_INSERT [dbo].[Eventos] OFF
GO
SET IDENTITY_INSERT [dbo].[Imagenes] ON 

INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (1, N'/Content/images/Anuncios/OpenGraph-TW-1200x630.jpg', NULL)
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (2, N'C:\Users\LEGION\Desktop\a\RadioConexionLatam\Content\images\Anuncios\logo.png', N'logo.png')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (3, N'/Content/images/Anuncios/logo.png', N'logo.png')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (4, N'/Content/images/Anuncios/29072.png', N'29072.png')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (5, N'/Content/images/Anuncios/29072.png', N'29072.png')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (6, N'/Content/images/Anuncios/logo.png', N'logo.png')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (7, N'/Content/images/Anuncios/181918_1430735.jpg', N'181918_1430735.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (8, N'/Content/images/Anuncios/571457_1435546.jpg', N'571457_1435546.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (9, N'/Content/images/Anuncios/543254_1435750.jpg', N'543254_1435750.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (10, N'/Content/images/Anuncios/340934_1445503.jpg', N'340934_1445503.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (11, N'/Content/images/Anuncios/105810_1460321.jpg', N'105810_1460321.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (12, N'/Content/images/Anuncios/Colombia.PNG', N'Colombia.PNG')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (13, N'/Content/images/Anuncios/1555673aam0120-comp-tk3-v008-r709115230-cjpg.jpg', N'1555673aam0120-comp-tk3-v008-r709115230-cjpg.jpg')
INSERT [dbo].[Imagenes] ([idImagen], [url], [descripcion]) VALUES (14, N'/Content/images/Anuncios/silueta (3).jpg', N'silueta (3).jpg')
SET IDENTITY_INSERT [dbo].[Imagenes] OFF
GO
SET IDENTITY_INSERT [dbo].[ProgramacionSemanal] ON 

INSERT [dbo].[ProgramacionSemanal] ([Id], [Dia], [Hora], [NombrePrograma], [idUsuario], [idCategoria]) VALUES (1, N'Lunes', N'4', N'Palestino', 3, 3)
INSERT [dbo].[ProgramacionSemanal] ([Id], [Dia], [Hora], [NombrePrograma], [idUsuario], [idCategoria]) VALUES (2, N'Martes', N'5', N'Diseno', 2, 2)
INSERT [dbo].[ProgramacionSemanal] ([Id], [Dia], [Hora], [NombrePrograma], [idUsuario], [idCategoria]) VALUES (11, N'Jueves', N'3', N'Hora Locaa', 4, 1)
SET IDENTITY_INSERT [dbo].[ProgramacionSemanal] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([idRol], [descripcion]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([idRol], [descripcion]) VALUES (2, N'Empleado')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([idUsuario], [nombre], [apellido], [correo], [idRol], [contrasena], [estado]) VALUES (1, N'Marcelo', N'Medina', N'johndoe@example.com', 1, N'123            ', N'I')
INSERT [dbo].[Usuarios] ([idUsuario], [nombre], [apellido], [correo], [idRol], [contrasena], [estado]) VALUES (2, N'Gustavo', N'Valle', N'gusvalle@gmail.com', 1, N'123123         ', N'A')
INSERT [dbo].[Usuarios] ([idUsuario], [nombre], [apellido], [correo], [idRol], [contrasena], [estado]) VALUES (3, N'Renato', N'Lupaca', N'renlupaca@gmail.com', 1, N'renlupaca      ', N'A')
INSERT [dbo].[Usuarios] ([idUsuario], [nombre], [apellido], [correo], [idRol], [contrasena], [estado]) VALUES (4, N'Richard', N'Mendoza', N'ricmenda@gmail.com', 2, N'123123', N'A')
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[Videos] ON 

INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (1, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (2, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (3, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (4, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (5, N'test', N'Video from test')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (6, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (7, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (8, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (9, N'test', N'Video from test')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (10, N'test2', N'Video from test2')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (11, N'test', N'Video from test')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (12, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (13, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (14, N'https://www.youtube.com/watch?v=cfT1K1Nu1jQ', N'Video from https://www.youtube.com/watch?v=cfT1K1Nu1jQ')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (15, N'https://www.youtube.com/watch?v=li2WyhmzTZM', N'Video from https://www.youtube.com/watch?v=li2WyhmzTZM')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (16, N'https://www.youtube.com/watch?v=li2WyhmzTZM', N'Video from https://www.youtube.com/watch?v=li2WyhmzTZM')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (17, N'https://www.youtube.com/watch?v=li2WyhmzTZM', N'Video from https://www.youtube.com/watch?v=li2WyhmzTZM')
INSERT [dbo].[Videos] ([idVideo], [url], [descripcion]) VALUES (18, N'https://www.youtube.com/watch?v=li2WyhmzTZM', N'Video from https://www.youtube.com/watch?v=li2WyhmzTZM')
SET IDENTITY_INSERT [dbo].[Videos] OFF
GO
ALTER TABLE [dbo].[Anuncios] ADD  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[Anuncios]  WITH CHECK ADD FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categorias] ([idCategoria])
GO
ALTER TABLE [dbo].[Anuncios]  WITH CHECK ADD FOREIGN KEY([idImagenPrincipal])
REFERENCES [dbo].[Imagenes] ([idImagen])
GO
ALTER TABLE [dbo].[Anuncios]  WITH CHECK ADD  CONSTRAINT [FK__Anuncios__idUsua__32E0915F] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuarios] ([idUsuario])
GO
ALTER TABLE [dbo].[Anuncios] CHECK CONSTRAINT [FK__Anuncios__idUsua__32E0915F]
GO
ALTER TABLE [dbo].[Anuncios]  WITH CHECK ADD FOREIGN KEY([idVideoPrincipal])
REFERENCES [dbo].[Videos] ([idVideo])
GO
ALTER TABLE [dbo].[EnlacesRelacionados]  WITH CHECK ADD FOREIGN KEY([idAnuncio])
REFERENCES [dbo].[Anuncios] ([idAnuncio])
GO
ALTER TABLE [dbo].[Eventos]  WITH CHECK ADD  CONSTRAINT [FK_Eventos_Categorias] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categorias] ([idCategoria])
GO
ALTER TABLE [dbo].[Eventos] CHECK CONSTRAINT [FK_Eventos_Categorias]
GO
ALTER TABLE [dbo].[ProgramacionSemanal]  WITH CHECK ADD  CONSTRAINT [FK_ProgramacionSemanal_Categorias] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categorias] ([idCategoria])
GO
ALTER TABLE [dbo].[ProgramacionSemanal] CHECK CONSTRAINT [FK_ProgramacionSemanal_Categorias]
GO
ALTER TABLE [dbo].[ProgramacionSemanal]  WITH CHECK ADD  CONSTRAINT [FK_ProgramacionSemanal_Usuarios] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuarios] ([idUsuario])
GO
ALTER TABLE [dbo].[ProgramacionSemanal] CHECK CONSTRAINT [FK_ProgramacionSemanal_Usuarios]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK__Usuarios__idRol__35BCFE0A] FOREIGN KEY([idRol])
REFERENCES [dbo].[Roles] ([idRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK__Usuarios__idRol__35BCFE0A]
GO
USE [master]
GO
ALTER DATABASE [RadioDB] SET  READ_WRITE 
GO
