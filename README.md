# Sistema de Gestión Inmobiliaria

Sistema web desarrollado en ASP.NET Core para la gestión integral de una inmobiliaria, incluyendo administración de propietarios, inquilinos, inmuebles, contratos y pagos.

## Características Principales

### 🔐 Autenticación y Autorización
- Sistema de login con autenticación por cookies
- Roles diferenciados: Administrador y Empleado
- Solo los administradores pueden gestionar usuarios
- Políticas de autorización para limitar eliminaciones

### 👥 Gestión de Personas
- **ABM de Propietarios**: Gestión completa de propietarios
- **ABM de Inquilinos**: Administración de inquilinos
- **ABM de Usuarios**: Gestión de usuarios del sistema con roles
- **Edición de Perfil**: Cambio de contraseña y avatar de usuario

### 🏠 Gestión de Inmuebles
- **ABM de Inmuebles**: Administración completa de propiedades
- **ABM de Tipos de Inmuebles**: Gestión de categorías
- **Listado de Inmuebles Disponibles**: Filtrado por estado
- **Inmuebles por Propietario**: Vista específica por propietario
- **Búsqueda por Fechas**: Inmuebles disponibles en un rango de fechas

### 📋 Gestión de Contratos
- **ABM de Contratos**: Administración completa de contratos
- **Contratos Vigentes**: Listado de contratos activos por fechas
- **Contratos por Inmueble**: Historial de contratos por propiedad
- **Validación de Superposición**: Control de fechas superpuestas
- **Renovación de Contratos**: Creación automática de nuevos contratos
- **Terminación Anticipada**: Con registro de multas

### 💰 Gestión de Pagos
- **ABM de Pagos**: Administración de pagos por contrato
- **Pagos por Contrato**: Listado específico con opción de crear nuevos
- **Pago de Multas**: Registro de pagos de multas por terminación anticipada
- **Auditoría de Pagos**: Registro de usuario creador y anulador

### 📊 Dashboard y Usabilidad
- **Dashboard Principal**: Estadísticas del sistema
- **Menú de Navegación**: Acceso rápido a todas las funcionalidades
- **Notificaciones**: Mensajes de éxito y error
- **Validaciones**: Control de datos de entrada
- **Responsive Design**: Interfaz adaptable a diferentes dispositivos

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core 8.0
- **Base de Datos**: MySQL con Entity Framework Core
- **Frontend**: Bootstrap 5, Font Awesome
- **Autenticación**: ASP.NET Core Identity con Cookies
- **ORM**: Entity Framework Core con Pomelo MySQL

## Estructura del Proyecto

```
├── Controllers/          # Controladores MVC
│   ├── HomeController.cs
│   ├── UsuariosController.cs
│   ├── PropietariosController.cs
│   ├── InquilinosController.cs
│   ├── InmueblesController.cs
│   ├── ContratosController.cs
│   └── PagosController.cs
├── Models/              # Modelos de datos
│   ├── Usuario.cs
│   ├── Propietario.cs
│   ├── Inquilino.cs
│   ├── Inmueble.cs
│   ├── Contrato.cs
│   ├── Pagos.cs
│   ├── TipoInmueble.cs
│   └── InmobiliariaDbContext.cs
├── Views/               # Vistas Razor
│   ├── Shared/
│   ├── Home/
│   ├── Usuarios/
│   ├── Propietarios/
│   ├── Inquilinos/
│   ├── Inmuebles/
│   ├── Contratos/
│   └── Pagos/
└── wwwroot/             # Archivos estáticos
    ├── css/
    ├── js/
    └── images/
```

## Instalación y Configuración

### Prerrequisitos
- .NET 8.0 SDK
- MySQL Server
- Visual Studio 2022 o VS Code

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone [URL_DEL_REPOSITORIO]
   cd Milena-Miselli
   ```

2. **Configurar la base de datos**
   - Crear una base de datos MySQL
   - Actualizar la cadena de conexión en `appsettings.json`
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=tu_password;"
     }
   }
   ```

3. **Ejecutar migraciones**
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar el proyecto**
   ```bash
   dotnet run
   ```

## Usuarios de Prueba

### Administrador
- **Email**: admin@inmobiliaria.com
- **Contraseña**: admin123
- **Rol**: Administrador

### Empleado
- **Email**: empleado@inmobiliaria.com
- **Contraseña**: empleado123
- **Rol**: Empleado

## Funcionalidades por Rol

### Administrador
- ✅ Acceso completo a todas las funcionalidades
- ✅ Gestión de usuarios (crear, editar, eliminar)
- ✅ Eliminación de registros
- ✅ Anulación de pagos
- ✅ Acceso a información de auditoría

### Empleado
- ✅ Gestión de propietarios, inquilinos, inmuebles y contratos
- ✅ Creación y edición de pagos
- ✅ Acceso a reportes y listados
- ❌ No puede gestionar usuarios
- ❌ No puede eliminar registros
- ❌ No puede anular pagos

## Base de Datos

### Diagrama de Entidad-Relación

El sistema incluye las siguientes entidades principales:

- **Usuarios**: Gestión de usuarios del sistema
- **Propietarios**: Dueños de inmuebles
- **Inquilinos**: Arrendatarios
- **Inmuebles**: Propiedades disponibles
- **TiposInmueble**: Categorías de inmuebles
- **Contratos**: Contratos de alquiler
- **Pagos**: Pagos realizados por contratos

### Relaciones
- Un Propietario puede tener múltiples Inmuebles
- Un Inmueble pertenece a un Propietario
- Un Contrato relaciona un Inquilino con un Inmueble
- Un Contrato puede tener múltiples Pagos
- Los Usuarios crean y gestionan Contratos y Pagos

## Características Técnicas

### Seguridad
- Autenticación por cookies
- Autorización basada en roles
- Validación de datos de entrada
- Protección contra ataques CSRF

### Validaciones
- Superposición de fechas en contratos
- Validación de estados de inmuebles
- Control de integridad referencial
- Validación de permisos por rol

### Auditoría
- Registro de usuario creador en contratos y pagos
- Registro de usuario que termina contratos
- Registro de usuario que anula pagos
- Información visible solo para administradores

## Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Contacto

Para consultas o soporte, contactar a:
- **Email**: [tu-email@ejemplo.com]
- **GitHub**: [tu-usuario-github]

---

**Desarrollado con ❤️ usando ASP.NET Core**







