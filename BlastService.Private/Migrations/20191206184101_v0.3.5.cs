using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Reflection;

namespace BlastService.Private.Migrations
{
    public partial class v035 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "BlastService.Private.Scripts.BlastDS_sp_DeleteProject.sql";
            var deleteSp = string.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                deleteSp = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(deleteSp))
            {
                migrationBuilder.Sql(deleteSp);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropSp = @"DROP PROCEDURE IF EXISTS ""DeleteProject""";
            migrationBuilder.Sql(dropSp);
        }
    }
}
