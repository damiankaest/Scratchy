using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace Scratchy.Persistence.DB
{
    public class ScratchItDbContextFactory : IDesignTimeDbContextFactory<ScratchItDbContext>
    {
        public ScratchItDbContext CreateDbContext(string[] args)
        {
            // Pfad zum Startprojekt (Scratchy) ermitteln
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Scratchy");

            // Konfiguration aufbauen
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Verbindungszeichenfolge abrufen
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Die Verbindungszeichenfolge 'DefaultConnection' wurde nicht gefunden oder ist leer.");
            }

            // DbContextOptions erstellen
            var optionsBuilder = new DbContextOptionsBuilder<ScratchItDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ScratchItDbContext(optionsBuilder.Options);
        }
    }
}
