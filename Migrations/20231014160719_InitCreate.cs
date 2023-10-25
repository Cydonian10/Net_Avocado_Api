using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiAvocados.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Shape = table.Column<string>(type: "text", nullable: false),
                    Hardiness = table.Column<string>(type: "text", nullable: false),
                    Taste = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Roles = table.Column<int[]>(type: "integer[]", nullable: true, defaultValue: new[] { 1 })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avocado",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sku = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avocado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avocado_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AvocadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenItem_Avocado_AvocadoId",
                        column: x => x.AvocadoId,
                        principalTable: "Avocado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Attribute",
                columns: new[] { "Id", "Description", "Hardiness", "Shape", "Taste" },
                values: new object[,]
                {
                    { new Guid("0d22fddc-da84-48d2-8df8-396879eab809"), "A seedling bred from 'Hass' x 'Thille' in 1982, 'Gwen' is higher yielding and more dwarfing than 'Hass' in California. The fruit has an oval shape, slightly smaller than 'Hass' (100–200 g or 3.5–7.1 oz), with a rich, nutty flavor. The skin texture is more finely pebbled than 'Hass', and is dull green when ripe. It is frost-hardy down to −1 °C (30 °F)", "−1 °C", "Plump", "Superb, is an avocado" },
                    { new Guid("4e05fe82-69d5-44ef-b1d0-b853ced86aa3"), "Developed from a chance seedling found in 1948 by James S. Reed in California, this cultivar has large, round, green fruit with a smooth texture and dark, thick, glossy skin. Smooth and delicate, the flesh has a slightly nutty flavor. The skin ripens green. A Guatemalan type, it is hardy to −1 °C (30 °F). Tree size is about 5 by 4", "-5 °C", "Pear", "Catchy, is an avocado" },
                    { new Guid("8bf12218-74ab-4d8a-b076-771e51ddf765"), "Developed by a farmer, James Bacon, in 1954, Bacon has medium-sized fruit with smooth, green skin with yellow-green, light-tasting flesh. When ripe, the skin remains green, but darkens slightly, and fruit yields to gentle pressure. It is cold-hardy down to −5 °C (23 °F)", "−5 °C", "Oval", "Creamy, is an avocado" },
                    { new Guid("a61ad1c0-6537-40e8-a56a-67874c251589"), "The 'Hass' is the most common cultivar of avocado. It produces fruit year-round and accounts for 80% of cultivated avocados in the world.[21][55] All 'Hass' trees are descended from a single 'mother tree' raised by a mail carrier named Rudolph Hass, of La Habra Heights, California.[20][55] Hass patented the productive tree in 1935. The 'mother tree', of uncertain subspecies, died of root rot and was cut down in September 2002.[21][55][56] 'Hass' trees have medium-sized (150–250 g or 5.3–8.8 oz), ovate fruit with a black, pebbled skin. The flesh has a nutty, rich flavor with 19% oil. A hybrid Guatemalan type can withstand temperatures to −1 °C (30 °F)", "−1 °C", "Oval", "Gorgeous, is an avocado" },
                    { new Guid("b0ef7a5d-1a30-4f5f-b442-bbc058687832"), "The Lamb Hass avocado is a cross between a Hass and Gwen avocado. The fruits are larger in size and later maturing than Hass. It is gaining in popularity as a commercial and backyard variety due to its exceptional flavor and easy peeling qualities. The tree has an upright, compact habit.", "-2 °C", "Obovate", "Great, is an avocado" },
                    { new Guid("c646a335-4aa4-4c76-b93b-dc681aed5bd8"), "The Fuerte avocado is the second largest commercial variety behind Hass. It is a long time California commercial variety valued for its winter season ripening and its B-Type blossom type which most growers plant adjacent to the Hass for a more consistent production cycle. This avocado tends to produce heavily in alternate years.", "-1 °C", "Pear", "Magnificent, is a strong avocado" },
                    { new Guid("d5712419-9ff3-4147-b470-7a95c8cc1c7d"), "First grown on the Pinkerton Ranch in Saticoy, California, in the early 1970s, 'Pinkerton' is a seedling of 'Hass' x 'Rincon'. The large fruit has a small seed, and its green skin deepens in color as it ripens. The thick flesh has a smooth, creamy texture, pale green color, good flavor, and high oil content. It shows some cold tolerance, to −1 °C (30 °F) and bears consistently heavy crops. A hybrid Guatemalan type, it has excellent peeling characteristics", "−1 °C", "Long pear", "Marvelous, is an avocado" },
                    { new Guid("ea5a8e7f-a7a4-48fb-9a35-0c733d66492b"), "A relatively new cultivar, it was, the pretty boy baby, discovered in South Africa in the early 1990s by Mr. A.G. (Dries) Joubert. Maluma Babyy. It is a chance seedling of unknown parentage", "1 °C", "Oval", "Catchy, is an avocado" }
                });

            migrationBuilder.InsertData(
                table: "Avocado",
                columns: new[] { "Id", "AttributeId", "Image", "Name", "Price", "Sku", "Stock" },
                values: new object[,]
                {
                    { new Guid("29176409-c767-4a18-95d9-b067516cd4de"), new Guid("d5712419-9ff3-4147-b470-7a95c8cc1c7d"), "http://localhost:5262/upload-image/images/pinkerton.jpg", "Pinkerton Avocado", 1.27m, "B4HZ42TM", 12 },
                    { new Guid("299c15bd-9d1f-40d9-a51f-2d5e38c09c0c"), new Guid("c646a335-4aa4-4c76-b93b-dc681aed5bd8"), "http://localhost:5262/upload-image/images/fuerte.jpg", "Fuerte Avocado", 1.21m, "AX4M8SJV", 12 },
                    { new Guid("51913335-d729-4686-9d49-2a4e2e8fb49a"), new Guid("b0ef7a5d-1a30-4f5f-b442-bbc058687832"), "http://localhost:5262/upload-image/images/lamb.jpg", "Lamb Hass Avocado", 1.34m, "N55229ZA", 12 },
                    { new Guid("ad57954f-88eb-4059-827c-fd7f9c31075b"), new Guid("4e05fe82-69d5-44ef-b1d0-b853ced86aa3"), "http://localhost:5262/upload-image/images/reed.jpg", "Reed Avocado", 1.18m, "ZY3T9XXC", 20 },
                    { new Guid("d47ccd86-4e27-4323-a7ea-33f410100375"), new Guid("0d22fddc-da84-48d2-8df8-396879eab809"), "http://localhost:5262/upload-image/images/gwen.jpg", "Gwen Hass Avocado", 1.25m, "HYA78F6J", 12 },
                    { new Guid("e006beb0-c134-4dc4-86f0-f3dbd07254de"), new Guid("a61ad1c0-6537-40e8-a56a-67874c251589"), "http://localhost:5262/upload-image/images/hass.jpg", "Hass Avocado", 1.39m, "RMRCZN7E", 12 },
                    { new Guid("f173a2f0-3a1c-4e11-844c-d0e6ca3e66a7"), new Guid("8bf12218-74ab-4d8a-b076-771e51ddf765"), "http://localhost:5262/upload-image/images/bacon.jpg", "Bacon Avocado", 1.51m, "BXD100BLK", 12 },
                    { new Guid("fad5ce3a-ea53-446a-a57b-3641fd794f60"), new Guid("ea5a8e7f-a7a4-48fb-9a35-0c733d66492b"), "http://localhost:5262/upload-image/images/maluma.jpg", "Maluma Hass Avocado", 1.15m, "NUR72KCM", 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avocado_AttributeId",
                table: "Avocado",
                column: "AttributeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenItem_AvocadoId",
                table: "OrdenItem",
                column: "AvocadoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenItem_OrderId",
                table: "OrdenItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenItem");

            migrationBuilder.DropTable(
                name: "Avocado");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
