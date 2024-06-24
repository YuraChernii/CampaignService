using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CampaignDatabase.Migrations
{
    /// <inheritdoc />
    public partial class CustomerData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT Customers ON;

                INSERT INTO Customers (Id, Gender, Age, City, Deposit, IsNewCustomer) VALUES
                (1, 0, 53, 'London', 104, 0),
                (2, 1, 44, 'New York', 209, 1),
                (3, 0, 36, 'New York', 208, 1),
                (4, 1, 87, 'London', 134, 0),
                (5, 0, 54, 'Paris', 123, 1),
                (6, 1, 45, 'New York', 210, 1),
                (7, 1, 49, 'Tel-Aviv', 174, 0),
                (8, 0, 35, 'Paris', 52, 1),
                (9, 0, 61, 'Tel-Aviv', 151, 0),
                (10, 0, 78, 'Paris', 57, 0),
                (11, 1, 41, 'New York', 131, 0),
                (12, 1, 32, 'Tel-Aviv', 154, 1),
                (13, 1, 62, 'Paris', 135, 0),
                (14, 0, 67, 'Tel-Aviv', 153, 1),
                (15, 1, 68, 'London', 241, 1),
                (16, 0, 41, 'London', 134, 0),
                (17, 1, 46, 'London', 212, 0),
                (18, 1, 77, 'Tel-Aviv', 97, 1),
                (19, 0, 51, 'London', 141, 1),
                (20, 0, 80, 'Paris', 189, 0),
                (21, 1, 31, 'Tel-Aviv', 134, 1),
                (22, 1, 80, 'Tel-Aviv', 81, 0),
                (23, 1, 36, 'London', 237, 1),
                (24, 1, 65, 'Tel-Aviv', 119, 0),
                (25, 0, 72, 'Tel-Aviv', 139, 0),
                (26, 0, 64, 'Tel-Aviv', 128, 1),
                (27, 0, 29, 'London', 76, 1),
                (28, 0, 25, 'London', 203, 1),
                (29, 0, 77, 'New York', 54, 1),
                (30, 1, 79, 'Paris', 165, 1),
                (31, 1, 26, 'Paris', 143, 1),
                (32, 1, 74, 'London', 61, 0),
                (33, 0, 74, 'New York', 103, 0),
                (34, 1, 46, 'New York', 121, 1),
                (35, 1, 47, 'New York', 214, 0),
                (36, 1, 78, 'New York', 111, 0),
                (37, 1, 46, 'New York', 223, 1),
                (38, 1, 26, 'New York', 78, 1),
                (39, 1, 49, 'Tel-Aviv', 60, 0),
                (40, 1, 74, 'New York', 53, 1);

                SET IDENTITY_INSERT Customers OFF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Customers
                WHERE Id IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                             11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                             21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 
                             31, 32, 33, 34, 35, 36, 37, 38, 39, 40);
            ");
        }
    }
}
