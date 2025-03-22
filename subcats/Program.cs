var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnotherPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:8090")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
}); // Configura el servicio CORS con una pol�tica espec�fica

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Aseg�rate de que tienes esto antes de UseCors()

app.UseCors("AnotherPolicy"); // Aplica la pol�tica CORS espec�fica

app.UseAuthorization();

app.MapControllers();

app.Run();
