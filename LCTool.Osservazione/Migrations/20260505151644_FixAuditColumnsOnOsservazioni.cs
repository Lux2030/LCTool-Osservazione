using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LCTool.Osservazione.Migrations
{
    public partial class FixAuditColumnsOnOsservazioni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[Osservazioni]', N'U') IS NULL
BEGIN
    THROW 50001, 'La tabella [dbo].[Osservazioni] non esiste nel database corrente.', 1;
END;

IF COL_LENGTH('dbo.Osservazioni', 'CreatedUser') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'CreatoDa') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Osservazioni.CreatedUser', 'CreatoDa', 'COLUMN';
END;

IF COL_LENGTH('dbo.Osservazioni', 'UpdatedUser') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'ModificatoDa') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Osservazioni.UpdatedUser', 'ModificatoDa', 'COLUMN';
END;

IF COL_LENGTH('dbo.Osservazioni', 'CreatoDa') IS NULL
BEGIN
    ALTER TABLE [dbo].[Osservazioni]
    ADD [CreatoDa] nvarchar(256) NULL;
END;

IF COL_LENGTH('dbo.Osservazioni', 'ModificatoDa') IS NULL
BEGIN
    ALTER TABLE [dbo].[Osservazioni]
    ADD [ModificatoDa] nvarchar(256) NULL;
END;

IF COL_LENGTH('dbo.Osservazioni', 'RevisionatoDa') IS NULL
BEGIN
    ALTER TABLE [dbo].[Osservazioni]
    ADD [RevisionatoDa] nvarchar(256) NULL;
END;

IF COL_LENGTH('dbo.Osservazioni', 'CreatedUser') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'CreatoDa') IS NOT NULL
BEGIN
    EXEC(N'
        UPDATE [dbo].[Osservazioni]
        SET [CreatoDa] = COALESCE([CreatoDa], [CreatedUser])
    ');

    EXEC(N'
        ALTER TABLE [dbo].[Osservazioni]
        DROP COLUMN [CreatedUser]
    ');
END;

IF COL_LENGTH('dbo.Osservazioni', 'UpdatedUser') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'ModificatoDa') IS NOT NULL
BEGIN
    EXEC(N'
        UPDATE [dbo].[Osservazioni]
        SET [ModificatoDa] = COALESCE([ModificatoDa], [UpdatedUser])
    ');

    EXEC(N'
        ALTER TABLE [dbo].[Osservazioni]
        DROP COLUMN [UpdatedUser]
    ');
END;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[Osservazioni]', N'U') IS NULL
BEGIN
    RETURN;
END;

IF COL_LENGTH('dbo.Osservazioni', 'RevisionatoDa') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Osservazioni]
    DROP COLUMN [RevisionatoDa];
END;

IF COL_LENGTH('dbo.Osservazioni', 'CreatoDa') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'CreatedUser') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Osservazioni.CreatoDa', 'CreatedUser', 'COLUMN';
END;

IF COL_LENGTH('dbo.Osservazioni', 'ModificatoDa') IS NOT NULL
   AND COL_LENGTH('dbo.Osservazioni', 'UpdatedUser') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Osservazioni.ModificatoDa', 'UpdatedUser', 'COLUMN';
END;
");
        }
    }
}