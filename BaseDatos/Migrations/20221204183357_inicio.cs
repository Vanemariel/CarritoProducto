using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseDatos.Migrations
{
    public partial class inicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TablaUsuario",
                columns: table => new
                {
                    DNI = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vip = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablaUsuario", x => x.DNI);
                });

            migrationBuilder.CreateTable(
                name: "TablaCarrito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioTotal = table.Column<float>(type: "real", nullable: false),
                    DniUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablaCarrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablaCarrito_TablaUsuario_DniUsuario",
                        column: x => x.DniUsuario,
                        principalTable: "TablaUsuario",
                        principalColumn: "DNI",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablaProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarritoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablaProductos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablaProductos_TablaCarrito_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "TablaCarrito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Entity_Id",
                table: "TablaCarrito",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TablaCarrito_DniUsuario",
                table: "TablaCarrito",
                column: "DniUsuario");

            migrationBuilder.CreateIndex(
                name: "Entity_Id1",
                table: "TablaProductos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TablaProductos_CarritoId",
                table: "TablaProductos",
                column: "CarritoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TablaProductos");

            migrationBuilder.DropTable(
                name: "TablaCarrito");

            migrationBuilder.DropTable(
                name: "TablaUsuario");
        }
    }
}
