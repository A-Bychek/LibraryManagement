using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Infrastructure.Migrations
{
    [DbContext(typeof(LibraryManagementDbContext))]
    [Migration("20251204180000_SeedData")]
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columnTypes: new string[] {
                    "character varying(200)",
                    "character varying(200)",
                    "character varying(2000)",
                    "timestamp without time zone",
                    "boolean"
                },
                columns: new[]
                {
                    "FirstName",
                    "LastName",
                    "Biography",
                    "DateOfBirth",
                    "IsActive"
                },
                values: new object[,]
                {
                    {
                        "Stephen",
                        "King",
                        "Horror fiction author.",
                        new DateTime(1947, 09, 21),
                        true
                    },
                    {
                        "J.K.",
                        "Rowling",
                        "Harry Potter fiction author",
                        new DateTime(1965, 07, 31),
                        true
                    },
                {
                        "George R.R.",
                        "Martin",
                        "A Song of Ice and Fire fiction author",
                        new DateTime(1948, 09, 20),
                        true
                    },
                {
                        "Agatha",
                        "Christie",
                        "Author of detective novels",
                        new DateTime(1890,09, 15),
                        true
                    },
                {
                        "Isaac",
                        "Asimov",
                        "Science fiction writer",
                        new DateTime(1920, 01, 02),
                        true
                    },
                {
                        "Maxim",
                        "Gorky",
                        "The author of realistic novels",
                        new DateTime(1868, 03, 28),
                        true
                    }
                }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columnTypes: new string[] {
                    "character varying(200)",
                    "character varying(2000)",
                    "bigint",
                    "integer",
                    "boolean"
                },
                    columns: new[]
                    {
                        "Name",
                        "Description",
                        "ParentCategoryId",
                        "SortOrder",
                        "IsActive"
                    },
                    values: new object[,]
                    {
                        {
                            "Fiction",
                            "Fictional literature",
                            null,
                            1,
                            true
                        },
                    {
                            "Non-Fiction",
                            "Non-fictional literature",
                            null,
                            0,
                            true
                        },
                    {
                            "Science Fiction",
                            "Sci-fi literature",
                            1,
                            1,
                            true
                        },
                    {
                            "Fantasy",
                            "Fantasy literature",
                            1,
                            1,
                            true
                        },
                    {
                            "Mystery",
                            "Mystery fiction",
                            1,
                            1,
                            true
                        },
                    {
                            "Horror",
                            "Horror fiction",
                            1,
                            0,
                            true
                        },
                    {
                            "Biography",
                            "Biographical literature",
                            2,
                            1,
                            true
                        },
                    {
                            "Detective",
                            "Detective literature",
                            2,
                            0,
                            true
                        }
                    }
            );

            migrationBuilder.InsertData(
               table: "Books",
               columnTypes: new string[] {
                    "character varying(200)",
                    "character varying(13)",
                    "character varying(2000)",
                    "bigint",
                    "bigint",
                    "timestamp without time zone",
                    "integer",
                    "boolean",
                    "timestamp without time zone"
                },
               columns: new[]
               {
                   "Title",
                   "ISBN",
                   "Description",
                   "AuthorId",
                   "CategoryId",
                   "PublishedDate",
                   "PageCount",
                   "IsAvailable",
                   "CreatedDate"
               },
               values: new object[,]
               {
                   {
                       "The Shining",
                       "9780385121675",
                       "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence.",
                       1,
                       6,
                       new DateTime(1977, 01, 28),
                       447,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "Harry Potter and the Philosopher's Stone",
                       "9780747532699",
                       "A young wizard discovers his magical heritage on his 11th birthday.",
                       2,
                       4,
                       new DateTime(1997, 06, 26),
                       223,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "A Game of Thrones",
                       "9780553103540",
                       "Noble families fight for control of the Iron Throne of Westeros.",
                       3,
                       5,
                       new DateTime(1996, 08, 01),
                       694,
                       false,
                       DateTime.UtcNow,
                   },
               {
                       "Murder on the Orient Express",
                       "9780062693662",
                       "Hercule Poirot investigates a murder on a luxury train.",
                       4,
                       8,
                       new DateTime(1934, 01, 01),
                       256,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "Foundation",
                       "9780553293357",
                       "A scientist creates a foundation to preserve knowledge through the fall of the Galactic Empire.",
                       5,
                       3,
                       new DateTime(1951, 06, 01),
                       255,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "It",
                       "9780450411434",
                       "Seven adults return to their hometown to confront a nightmare they had first stumbled on as teenagers.",
                       1,
                       6,
                       new DateTime(1986, 09, 15),
                       1138,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "Harry Potter and the Chamber of Secrets",
                       "9780439064873",
                       "The second year at Hogwarts School of Witchcraft and Wizardry.",
                       2,
                       4,
                       new DateTime(1998, 07, 02),
                       251,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "A Clash of Kings",
                       "9780553108033",
                       "The second novel in A Song of Ice and Fire.",
                       3,
                       5,
                       new DateTime(1998, 11, 16),
                       761,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "And Then There Were None",
                       "9780062073488",
                       "Ten people are invited to an isolated island, and are killed one by one.",
                       4,
                       8,
                       new DateTime(1939, 11, 06),
                       272,
                       true,
                       DateTime.UtcNow,
                   },
               {
                       "The Old Woman Izergil",
                       "9798390533352",
                       "The narration is done on behalf of the author and the heroine, the old woman Izergil. It explores the ideas of freedom, the true meaning of life and love.",
                       6,
                       7,
                       new DateTime(1892, 09, 12),
                       45,
                       true,
                       DateTime.UtcNow,
                   }
               }
               );

            migrationBuilder.InsertData(
               table: "Users",
               columnTypes: new string[] {
                    "bigint"
               },
               columns: new[]
               {
                   "UserId"
               },
               values: new object[,]
               {
                   {
                       1
                   },
               {
                       2
                   },
               {
                       3
                   },
               {
                       4
                   }
               }
               );

            migrationBuilder.InsertData(
               table: "Borrowings",
               columnTypes: new string[] {
                    "bigint",
                    "bigint",
                    "timestamp without time zone",
                    "timestamp without time zone",
                    "timestamp without time zone",
                    "character varying(15)"
                },
               columns: new[]
               {
                   "BookId",
                   "UserId",
                   "BorrowDate",
                   "DueDate",
                   "ReturnDate",
                   "Status"
               },
               values: new object[,]
               {
                   {
                       3,
                       1,
                       new DateTime(2025, 11, 12),
                       new DateTime(2025, 11, 26),
                       null,
                       "Overdue"
                   },
               {
                       1,
                       2,
                       new DateTime(2025, 12, 01),
                       new DateTime(2026, 01, 18),
                       null,
                       "Active"
                   },
               {
                       5,
                       3,
                       new DateTime(2025, 11, 15),
                       new DateTime(2025, 11, 29),
                       new DateTime(2025, 11, 28),
                       "Returned"
                   }
               }
               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Users", "UserId", 1);
            migrationBuilder.DeleteData("Users", "UserId", 2);
            migrationBuilder.DeleteData("Users", "UserId", 3);
            migrationBuilder.DeleteData("Users", "UserId", 4);

            migrationBuilder.DeleteData("Borrowings", "BorrowingId", 1);
            migrationBuilder.DeleteData("Borrowings", "BorrowingId", 2);
            migrationBuilder.DeleteData("Borrowings", "BorrowingId", 3);

            migrationBuilder.DeleteData("Categories", "CategoryId", 1);
            migrationBuilder.DeleteData("Categories", "CategoryId", 2);
            migrationBuilder.DeleteData("Categories", "CategoryId", 3);
            migrationBuilder.DeleteData("Categories", "CategoryId", 4);
            migrationBuilder.DeleteData("Categories", "CategoryId", 5);
            migrationBuilder.DeleteData("Categories", "CategoryId", 6);
            migrationBuilder.DeleteData("Categories", "CategoryId", 7);
            migrationBuilder.DeleteData("Categories", "CategoryId", 8);

            migrationBuilder.DeleteData("Books", "BookId", 1);
            migrationBuilder.DeleteData("Books", "BookId", 2);
            migrationBuilder.DeleteData("Books", "BookId", 3);
            migrationBuilder.DeleteData("Books", "BookId", 4);
            migrationBuilder.DeleteData("Books", "BookId", 5);
            migrationBuilder.DeleteData("Books", "BookId", 6);
            migrationBuilder.DeleteData("Books", "BookId", 7);
            migrationBuilder.DeleteData("Books", "BookId", 8);
            migrationBuilder.DeleteData("Books", "BookId", 9);
            migrationBuilder.DeleteData("Books", "BookId", 10);

            migrationBuilder.DeleteData("Authors", "AuthorId", 1);
            migrationBuilder.DeleteData("Authors", "AuthorId", 2);
            migrationBuilder.DeleteData("Authors", "AuthorId", 3);
            migrationBuilder.DeleteData("Authors", "AuthorId", 4);
            migrationBuilder.DeleteData("Authors", "AuthorId", 5);
            migrationBuilder.DeleteData("Authors", "AuthorId", 6);
        }
    }
}
