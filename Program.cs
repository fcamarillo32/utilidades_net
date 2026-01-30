var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ⚡ Habilitar Swagger incluso fuera de Development para pruebas en Plesk
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Ajusta el endpoint si tu app está en un subdirectorio
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
    c.RoutePrefix = "swagger"; // Acceder en /swagger
});

// ⚡ HTTPS
// Solo activar si Plesk tiene certificado. 
// Si da problemas de 403/404, coméntalo temporalmente
// app.UseHttpsRedirection();

app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// ⚡ Si tu app está en un subdirectorio en Plesk, puedes usar:
// app.UsePathBase("/miapp");

app.Run();
