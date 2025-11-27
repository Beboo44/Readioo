using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Readioo.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditGenreAbout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- PART 1: SAFELY RESTRUCTURE BookGenres (Preserve Data) ---

            // 1. Rename the existing table to a temporary name so we don't lose data
            migrationBuilder.RenameTable(
                name: "BookGenres",
                newName: "BookGenres_Backup");

            // 2. Create the NEW table with the correct structure (No 'Id' column, Composite Key)
            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // 3. Restore the data from the Backup table to the New table
            // We only copy BookId and GenreId (ignoring the old Id column)
            migrationBuilder.Sql("INSERT INTO BookGenres (BookId, GenreId) SELECT BookId, GenreId FROM BookGenres_Backup");

            // 4. Create the Index for performance
            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");

            // 5. Drop the backup table now that data is safe
            migrationBuilder.DropTable(
                name: "BookGenres_Backup");


            // --- PART 2: UPDATE DESCRIPTIONS ---

            migrationBuilder.Sql("UPDATE Genres SET Description = 'Explore the limitless possibilities of the universe. From space operas and alien civilizations to cybernetic futures and time travel, these stories ask the ultimate ''what if'' questions about technology and humanity.' WHERE GenreName = 'Science Fiction'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Prepare to be terrified. These spine-chilling tales explore the darker side of existence, featuring supernatural entities, psychological torments, and the unknown, designed to keep you on the edge of your seat and awake at night.' WHERE GenreName = 'Horror'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Enter worlds where society has collapsed or turned oppressive. These gripping narratives explore survival, rebellion, and the resilience of the human spirit amidst totalitarian regimes, environmental disasters, or post-apocalyptic ruins.' WHERE GenreName = 'Dystopian'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Escape into realms of magic, myth, and adventure. Whether it’s epic quests to save a kingdom, battles with dragons, or subtle magical realism, these stories weave folklore and imagination into unforgettable journeys.' WHERE GenreName = 'Fantasy'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Fast-paced and adrenaline-fueled, these books are impossible to put down. Packed with high stakes, dangerous chases, and shocking plot twists, thrillers keep you guessing untill the very last page.' WHERE GenreName = 'Thriller'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Put on your detective hat and solve the puzzle. From classic whodunits and noir investigations to cozy village secrets, these stories revolve around crime, clues, and the thrill of uncovering the truth.' WHERE GenreName = 'Mystery'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Dive deep into the complexities of the human experience. These emotionally resonant stories focus on realistic characters, interpersonal conflicts, and the triumphs and tragedies of everyday life.' WHERE GenreName = 'Drama'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Travel back in time to experience different eras. Meticulously researched and atmospherically rich, these novels transport readers to the past, blending real events with fictional lives to bring history to life.' WHERE GenreName = 'Historical'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Explore the corridors of power and the mechanisms of governance. These narratives critique societal structures, expose corruption, and delve into the ideologies that shape nations and revolutions.' WHERE GenreName = 'Political'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Immerse yourself in the art of long-form storytelling. These works of fiction prioritize character development and narrative depth, offering a profound exploration of themes that resonate with the human condition.' WHERE GenreName = 'Novel'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Discover the true stories behind the names. These accounts offer intimate looks into the lives of historical figures, innovators, and cultural icons, providing inspiration and insight through their struggles and achievements.' WHERE GenreName = 'Biography'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Challenge your perspective and expand your mind. These works delve into the fundamental nature of knowledge, reality, ethics, and existence, encouraging deep contemplation on the meaning of life.' WHERE GenreName = 'Philosophy'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Step into the shadowy world of lawbreakers and law enforcement. Focusing on the perpetrators, the victims, and the investigators, these gritty tales explore the motives behind criminal acts and the quest for justice.' WHERE GenreName = 'Crime'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'Perfect for quick escapes, these collections offer bite-sized narratives that pack a punch. Ranging from experimental vignettes to fully realized tales, they capture distinct moments and emotions in a concise format.' WHERE GenreName = 'Short Stories'");
            migrationBuilder.Sql("UPDATE Genres SET Description = 'A sharp wit meets social commentary. Using humor, irony, and exaggeration, these clever stories expose the absurdities of society, politics, and human behavior, making you think while you laugh.' WHERE GenreName = 'Satire'");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookGenres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
