# FanMasterBlazor

Proyecto web de mantenimiento para la tabla `dbo.NSNeedDecoration` en Azure SQL.

## Nota sobre Visual Basic + Blazor
Blazor no soporta componentes `.razor` escritos en Visual Basic en ASP.NET Core. Por esta limitación oficial del framework, la implementación se entrega en C# (compatible al 100% con Blazor Server).

## Funcionalidad incluida
- Pantalla de inicio de sesión para capturar `usuario` y `contraseña`.
- Armado dinámico de la cadena de conexión hacia:
  - Servidor: `fanserver.database.windows.net`
  - Base de datos: `FanMaster`
- CRUD completo para `dbo.NSNeedDecoration`:
  - Crear
  - Listar
  - Editar
  - Eliminar
- Validaciones de formulario alineadas con reglas de negocio (mes 1-12, units >= 0).

## Ejecutar
```bash
cd FanMasterBlazor
dotnet restore
dotnet run
```

## Campos manejados
- NSNeedDecorationID (identity)
- ForecastYear
- ForecastMonth
- Category
- Program
- ProductType
- VendorStream
- Units
- SourceFileName
- SourceAsOfDate
- LoadTimestampUTC (solo lectura, generado por SQL)
