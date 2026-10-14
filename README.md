# 🎾 Tenis Ahora - Backend Web API (.NET C#)

¡Bienvenido al repositorio backend de **Tenis Ahora**! Este sistema está diseñado para gestionar de manera integral la administración, reservas de canchas, insumos, cobros, clases y torneos del complejo deportivo *"El Deporte Saludable"*.

Este documento sirve como **guía paso a paso** para que todos los integrantes del equipo puedan entender la arquitectura del proyecto, configurar su entorno local, trabajar con **Entity Framework Core (Code-First)**, definir tablas con **Fluent API**, aplicar **migraciones** y mantener convenciones de código limpias e iguales para todos.

## 📋 Tabla de Contenidos

1. [Visión General y Reglas del Negocio](#1-visión-general-y-reglas-del-negocio)

2. [Arquitectura Hexagonal (Puertos y Adaptadores)](#2-arquitectura-hexagonal-puertos-y-adaptadores)

3. [Estructura del Proyecto y Solución](#3-estructura-del-proyecto-y-solución)

4. [Guía de Configuración Inicial (Comandos CLI)](#4-guía-de-configuración-inicial-comandos-cli)

5. [Modelado de Entidades y Fluent API (Code-First)](#5-modelado-de-entidades-y-fluent-api-code-first)

6. [Flujo de Trabajo con Migraciones (EF Core)](#6-flujo-de-trabajo-con-migraciones-ef-core)

7. [Guía para Ejecutar el Proyecto en Local](#7-guía-para-ejecutar-el-proyecto-en-local)

8. [Convenciones de Código y Nomenclatura (C# / .NET)](#8-convenciones-de-código-y-nomenclatura-c--net)

9. [Buenas Prácticas para el Equipo](#9-buenas-prácticas-para-el-equipo)

## 1. Visión General y Reglas del Negocio

El backend expone una **Web API RESTful** construida en C# / .NET 8. Las principales reglas de negocio contempladas en la documentación funcional son:

* **Reservas de Canchas:**

  * Duración máxima por alquiler: **2 horas**.

  * Antelación máxima de reserva: **30 días**.

  * Se permite reservar para **2 o 4 jugadores** (datos obligatorios de todos los participantes).

  * **Seña del 50%:** La reserva requiere abonar el 50% inicial para registrar la seña y el saldo restante al finalizar el partido.

  * **Control de Stock:** Cada alquiler descuenta raquetas, pelotas y redes. Si no hay stock disponible, el sistema debe impedir el alquiler de la cancha.

  * **Cancelaciones:** Cancelación con un mínimo de **6 horas** de antelación para reembolso total. Fuera de término aplica penalización.

* **Módulo de Clases y Profesores:**

  * Límite máximo de **30 alumnos por clase grupal**.

  * **Certificación Obligatoria:** Es requisito que los profesores y entrenadores registren y tengan validado su Título Habilitante antes de impartir clases.

* **Canchas y Mantenimiento:**

  * Tipos de cancha: *Ladrillo*, *Cemento* y *Pasto*.

  * Bloqueo temporal por tareas de mantenimiento impidiendo nuevas reservas en esa franja horaria.

## 2. Arquitectura Hexagonal (Puertos y Adaptadores)

Para mantener el código ordenado, mantenible y desacoplado, organizamos el proyecto en 4 capas concéntricas:

```
       ┌────────────────────────────────────────────────────────┐
       │                 TenisAhora.Api (HTTP)                  │
       │  ┌──────────────────────────────────────────────────┐  │
       │  │       TenisAhora.Infrastructure (EF Core)        │  │
       │  │  ┌────────────────────────────────────────────┐  │  │
       │  │  │        TenisAhora.Application (Casos)      │  │  │
       │  │  │  ┌──────────────────────────────────────┐  │  │  │
       │  │  │  │      TenisAhora.Domain (Reglas)      │  │  │  │
       │  │  │  └──────────────────────────────────────┘  │  │  │
       │  │  └────────────────────────────────────────────┘  │  │
       │  └──────────────────────────────────────────────────┘  │
       └────────────────────────────────────────────────────────┘

```

* **`TenisAhora.Domain` (Núcleo Puro):** Contiene las entidades (`Cliente`, `Reserva`, `Cancha`), Objetos de Valor (*Value Objects*), Enums e interfaces de repositorios (Puertos). **No depende de ningún paquete externo ni base de datos.**

* **`TenisAhora.Application` (Lógica de Negocio):** Contiene los casos de uso, DTOs (*Data Transfer Objects*), validaciones y servicios de aplicación. Depende únicamente de `Domain`.

* **`TenisAhora.Infrastructure` (Adaptadores de Salida):** Implementa el acceso a datos mediante **Entity Framework Core**, repositorios, configuraciones de la base de datos (`DbContext`), migraciones y servicios externos (ej. pasarela de pago Mercado Pago).

* **`TenisAhora.Api` (Adaptadores de Entrada):** Expone los endpoints HTTP REST (`Controllers`), controladores de autenticación, middleware de errores y la configuración de inyección de dependencias en `Program.cs`.

## 3. Estructura del Proyecto y Solución

La estructura de archivos dentro del repositorio debe reflejar la siguiente jerarquía:

```
TenisAhora/
├── src/
│   ├── TenisAhora.Domain/
│   │   ├── Entities/                 # Cliente.cs, Reserva.cs, Cancha.cs, Profesor.cs, Equipamiento.cs
│   │   ├── Enums/                    # TipoCancha.cs, EstadoReserva.cs
│   │   ├── Exceptions/               # DomainException.cs (Ej: ReservaExcedeTiempoException)
│   │   └── Interfaces/               # IClienteRepository.cs, IReservaRepository.cs (Puertos)
│   │
│   ├── TenisAhora.Application/
│   │   ├── Dtos/                     # ClienteDto.cs, CrearReservaDto.cs
│   │   ├── Interfaces/               # IServicioCobro.cs, IServicioStock.cs
│   │   └── UseCases/                 # Reservas/CrearReservaUseCase.cs, Canchas/ConsultarDisponibilidadUseCase.cs
│   │
│   ├── TenisAhora.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/       # ClienteConfiguration.cs, ReservaConfiguration.cs (Fluent API)
│   │   │   └── Repositories/         # ClienteRepository.cs, ReservaRepository.cs
│   │   ├── ExternalServices/         # MercadoPagoQrService.cs
│   │   └── Migrations/               # Archivos de migración generados automáticamente por EF Core
│   │
│   └── TenisAhora.Api/
│       ├── Controllers/              # ClientesController.cs, ReservasController.cs
│       ├── Program.cs                # Inyección de dependencias y middlewares
│       └── appsettings.json          # Cadenas de conexión y configuración global
│
└── TenisAhora.sln

```

## 4. Guía de Configuración Inicial (Comandos CLI)

Si estás clonando el proyecto por primera vez o creando la estructura inicial desde la terminal de .NET CLI:

### Paso 1: Crear la solución y los proyectos

```
# Crear la carpeta principal
mkdir TenisAhora
cd TenisAhora

# Crear la solución
dotnet new sln -n TenisAhora

# Crear las capas en la carpeta src
dotnet new classlib -o src/TenisAhora.Domain
dotnet new classlib -o src/TenisAhora.Application
dotnet new classlib -o src/TenisAhora.Infrastructure
dotnet new webapi -o src/TenisAhora.Api

# Agregar proyectos a la solución
dotnet sln add src/TenisAhora.Domain/TenisAhora.Domain.csproj
dotnet sln add src/TenisAhora.Application/TenisAhora.Application.csproj
dotnet sln add src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj
dotnet sln add src/TenisAhora.Api/TenisAhora.Api.csproj

```

### Paso 2: Vincular las referencias entre capas

```
# Application depende de Domain
dotnet add src/TenisAhora.Application/TenisAhora.Application.csproj reference src/TenisAhora.Domain/TenisAhora.Domain.csproj

# Infrastructure depende de Application y Domain
dotnet add src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj reference src/TenisAhora.Domain/TenisAhora.Domain.csproj
dotnet add src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj reference src/TenisAhora.Application/TenisAhora.Application.csproj

# API depende de Infrastructure y Application
dotnet add src/TenisAhora.Api/TenisAhora.Api.csproj reference src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj
dotnet add src/TenisAhora.Api/TenisAhora.Api.csproj reference src/TenisAhora.Application/TenisAhora.Application.csproj

```

### Paso 3: Instalar paquetes de Entity Framework Core

```
# Paquetes en Infrastructure
dotnet add src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/TenisAhora.Infrastructure/TenisAhora.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools

# Paquete en API (necesario para ejecutar herramientas de diseño de migraciones)
dotnet add src/TenisAhora.Api/TenisAhora.Api.csproj package Microsoft.EntityFrameworkCore.Design

```

## 5. Modelado de Entidades y Fluent API (Code-First)

En la estrategia **Code-First**, las tablas de la base de datos se generan exclusivamente a partir del código en C#. Para no contaminar el dominio con atributos de bases de datos SQL (`[Key]`, `[Required]`), utilizamos **Fluent API** en la capa de **Infraestructura**.

### 1️⃣ Crear la Entidad en `TenisAhora.Domain/Entities/Cliente.cs`

```
namespace TenisAhora.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Relación: Un cliente puede tener múltiples reservas
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

```

### 2️⃣ Mapear la Tabla con Fluent API en `TenisAhora.Infrastructure/Persistence/Configurations/ClienteConfiguration.cs`

```
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TenisAhora.Domain.Entities;

namespace TenisAhora.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    modelBuilder.Entity<Cliente>( entity =>
    {
        // Nombre explícito de la tabla
        entity.ToTable("Clientes");

        // Clave Primaria
        entity.HasKey(c => c.Id);
        // Índice Único para el DNI (Regla RF-01)
        entity.HasIndex(c => c.Dni).IsUnique();
        // Restricciones de columnas
        entity.Property(c => c.Dni)
            .IsRequired()
            .HasMaxLength(9);
        entity.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);
        entity.Property(c => c.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        entity.Property(c => c.Telefono)
            .IsRequired()
            .HasMaxLength(20);
    });

        // Relación 1 a Muchos
        /*builder.HasMany(c => c.Reservas)
            .WithOne(r => r.Cliente)
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Restrict); Impide borrar el cliente si tiene reservas asociadas*/
    }
}

```

### 3️⃣ Registrar el `DbContext` en `TenisAhora.Infrastructure/Persistence/ApplicationDbContext.cs`

```
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TenisAhora.Domain.Entities;

namespace TenisAhora.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Cancha> Canchas => Set<Cancha>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Equipamiento> Equipamientos => Set<Equipamiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Carga automáticamente todas las clases de configuración que implementen IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

```

## 6. Flujo de Trabajo con Migraciones (EF Core)

### Requisito Previo

Cada integrante del equipo debe instalar la herramienta CLI de EF Core de forma global en su máquina (solo se ejecuta **una vez**):

```
dotnet tool install --global dotnet-ef

```

*(Si ya la tenían instalada, actualizarla con `dotnet tool update --global dotnet-ef`)*

### Comandos de Migración Día a Día

Ejecuta siempre los siguientes comandos parado desde la **raíz de la solución**:

#### 🟢 A. Crear una nueva migración

Cada vez que crees o modifiques una Entidad, Enum o archivo de Configuración Fluent API:

```
dotnet ef migrations add <NombreDeLaMigracion> --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api

```

*Ejemplo:*

```
dotnet ef migrations add InitialCreate --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api

```

> 📌 **¿Qué significan los parámetros?**
>
> * `--project`: Le indica a EF dónde está ubicado el `DbContext` y dónde guardar las migraciones (`TenisAhora.Infrastructure`).
>
> * `--startup-project`: Le indica a EF dónde encontrar la cadena de conexión en `appsettings.json` (`TenisAhora.Api`).

#### 🟡 B. Aplicar la migración a la base de datos local

Para impactar físicamente los cambios en la base de datos configurada:

```
dotnet ef database update --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api

```

#### 🔴 C. Revertir cambios o deshacer migraciones

* **Si cometiste un error y AÚN NO ejecutaste `database update`:**

  ```
  dotnet ef migrations remove --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api
  
  ```

* **Si YA ejecutaste `database update` y quieres volver la BD a una migración anterior:**

  ```
  dotnet ef database update <NombreMigracionAnterior> --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api
  
  ```

### Migración Automática al Iniciar la API (Recomendado para Desarrollo)

Para evitar que tus compañeros deban ejecutar `database update` manualmente al bajar cambios de Git, puedes agregar esto en `src/TenisAhora.Api/Program.cs`:

```
using Microsoft.EntityFrameworkCore;
using TenisAhora.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con la cadena de conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migraciones automáticas al arrancar el proyecto en desarrollo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

```

## 7. Guía para Ejecutar el Proyecto en Local

1. Configura la cadena de conexión en `src/TenisAhora.Api/appsettings.json`:

   ```
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=TenisAhoraDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   
   ```

2. Aplica las migraciones a la base de datos local:

   ```
   dotnet ef database update --project src/TenisAhora.Infrastructure --startup-project src/TenisAhora.Api
   
   ```

3. Ejecuta la Web API:

   ```
   dotnet run --project src/TenisAhora.Api
   
   ```

4. Abre tu navegador e ingresa a Swagger para probar los endpoints:
   `https://localhost:7001/swagger` (o el puerto mostrado en la consola).

## 8. Convenciones de Código y Nomenclatura (C# / .NET)

Para garantizar un código limpio, profesional y legible, todo el equipo debe seguir las **convenciones estándar oficiales de Microsoft C#**:

### Tabla Resumen de Convenciones

| Elemento | Estilo de Escritura | Ejemplo | 
| ----- | ----- | ----- | 
| **Carpetas** | `PascalCase` | `UseCases/`, `Configurations/`, `Entities/` | 
| **Archivos `.cs`** | `PascalCase` (debe coincidir con la clase) | `ClienteService.cs`, `Reserva.cs` | 
| **Namespaces** | `PascalCase` | `TenisAhora.Domain.Entities` | 
| **Clases / Structs / Records** | `PascalCase` | `Cliente`, `ReservaConfiguration` | 
| **Interfaces** | `PascalCase` con prefijo **`I`** | `IClienteRepository`, `IServicioStock` | 
| **Enums y sus Valores** | `PascalCase` | `TipoCancha.Ladrillo`, `EstadoReserva.Pendiente` | 
| **Métodos / Funciones** | `PascalCase` | `ObtenerPorDniAsync()`, `CalcularSeña()` | 
| **Propiedades Publicas** | `PascalCase` | `public string Nombre { get; set; }` | 
| **Campos Privados (`private`)** | **`_camelCase`** (guion bajo + minúscula) | `private readonly ApplicationDbContext _context;` | 
| **Variables Locales** | `camelCase` | `var clienteExistente = ...;`, `decimal montoSeña;` | 
| **Parámetros de Métodos** | `camelCase` | `public void Cancelar(Guid reservaId, string motivo)` | 
| **Constantes (`const`)** | `PascalCase` | `public const int MaxAlumnosPorClase = 30;` | 

### Ejemplos Visuales de Código Conforme a las Convenciones

#### 1. Nombre de Archivos, Classes e Interfaces

```
 Archivo: src/TenisAhora.Domain/Interfaces/IClienteRepository.cs
 Interface: public interface IClienteRepository { ... }

 Archivo: src/TenisAhora.Domain/Entities/Reserva.cs
 Clase: public class Reserva { ... }

```

#### 2. Definición de Clase con Campos Privados, Propiedades y Métodos

```
namespace TenisAhora.Application.Services;

public class ReservaService : IReservaService
{
    // Campos privados inyectados: _camelCase
    private readonly IReservaRepository _reservaRepository;
    private readonly IServicioStock _servicioStock;

    // Constante: PascalCase
    public const int DuracionMaximaHoras = 2;

    // Constructor: Parámetros en camelCase
    public ReservaService(IReservaRepository reservaRepository, IServicioStock servicioStock)
    {
        _reservaRepository = reservaRepository;
        _servicioStock = servicioStock;
    }

    // Método en PascalCase, Parámetros en camelCase
    public async Task<bool> RegistrarReservaAsync(Guid clienteId, Guid canchaId, DateTime fechaInicio)
    {
        // Variables locales en camelCase
        var horarioDisponible = await _reservaRepository.ExisteDisponibilidadAsync(canchaId, fechaInicio);

        if (!horarioDisponible)
        {
            return false;
        }

        return true;
    }
}

```

#### 3. Métodos Asíncronos

Todos los métodos que utilicen `async` / `await` deben llevar el sufijo **`Async`**:

* ❌ `ObtenerCliente()`

* ✅ `ObtenerClienteAsync()`

## 9. Buenas Prácticas para el Equipo

1. **Mensajes de Commit claros:** Utiliza convenciones como:

   * `feat:` para nuevas características (ej. `feat: agregar registro de clientes`).

   * `fix:` para corregir errores (ej. `fix: corregir calculo de seña`).

   * `docs:` para actualizaciones en la documentación.

2. **Nunca editar una migración generada a mano:** Si necesitas cambiar una tabla o columna, modifica el modelo C#/Fluent API y crea una nueva migración.

3. **Respetar la Inyección de Dependencias:** Los controladores llaman a los casos de uso/servicios de `Application`, y estos a su vez interactúan con las interfaces de repositorios creadas en `Domain`.

4. **Tratamiento de Excepciones:** Manejar las validaciones de negocio con excepciones descriptivas en la capa de `Domain` o `Application` sin dejar bloques `try/catch` vacíos.
