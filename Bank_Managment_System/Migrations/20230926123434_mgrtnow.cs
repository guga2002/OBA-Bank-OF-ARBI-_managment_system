using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Managment_System.Migrations
{
    /// <inheritdoc />
    public partial class mgrtnow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "2stepCodes",
                columns: table => new
                {
                    stepID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_2stepCodes", x => x.stepID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    ErrorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TimeOfocured = table.Column<DateTime>(type: "datetime2", nullable: false),
                    errortype = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.ErrorId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acction = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    timeofocured = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "ValuteCurses",
                columns: table => new
                {
                    ValuteCursesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EURO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GEL = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValuteCurses", x => x.ValuteCursesID);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    managerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.managerID);
                    table.ForeignKey(
                        name: "FK_Managers_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    OperatorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.OperatorID);
                    table.ForeignKey(
                        name: "FK_Operators_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactID = table.Column<int>(type: "int", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cookieformanagers",
                columns: table => new
                {
                    cookieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    managerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cookieformanagers", x => x.cookieID);
                    table.ForeignKey(
                        name: "FK_Cookieformanagers_Managers_managerID",
                        column: x => x.managerID,
                        principalTable: "Managers",
                        principalColumn: "managerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManagerAuthifications",
                columns: table => new
                {
                    ManagerAuthificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerAuthifications", x => x.ManagerAuthificationID);
                    table.ForeignKey(
                        name: "FK_ManagerAuthifications_Managers_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Managers",
                        principalColumn: "managerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CookiesForOperators",
                columns: table => new
                {
                    CookieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatorID = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookiesForOperators", x => x.CookieID);
                    table.ForeignKey(
                        name: "FK_CookiesForOperators_Operators_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Operators",
                        principalColumn: "OperatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorAunthifications",
                columns: table => new
                {
                    OperatorAunthificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorAunthifications", x => x.OperatorAunthificationID);
                    table.ForeignKey(
                        name: "FK_OperatorAunthifications_Operators_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Operators",
                        principalColumn: "OperatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    BankAccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Iban = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valute = table.Column<int>(type: "int", nullable: false),
                    AmountWithdrawed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastWithdrawDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalwithdrawed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.BankAccountID);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CookieforUsers",
                columns: table => new
                {
                    cookieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookieforUsers", x => x.cookieID);
                    table.ForeignKey(
                        name: "FK_CookieforUsers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAunthifications",
                columns: table => new
                {
                    UserAunthificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAunthifications", x => x.UserAunthificationID);
                    table.ForeignKey(
                        name: "FK_UserAunthifications_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    cardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Validatedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CVVCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PINcode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AuthorizedStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.cardId);
                    table.ForeignKey(
                        name: "FK_Cards_BankAccounts_accountId",
                        column: x => x.accountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valute = table.Column<int>(type: "int", nullable: false),
                    Sender_Iban = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reciever_Iban = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transaction_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransactTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "cardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserID",
                table: "BankAccounts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_accountId",
                table: "Cards",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Cookieformanagers_managerID",
                table: "Cookieformanagers",
                column: "managerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CookieforUsers_UserID",
                table: "CookieforUsers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CookiesForOperators_OperatorID",
                table: "CookiesForOperators",
                column: "OperatorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManagerAuthifications_ManagerID",
                table: "ManagerAuthifications",
                column: "ManagerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_ContactID",
                table: "Managers",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorAunthifications_OperatorID",
                table: "OperatorAunthifications",
                column: "OperatorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operators_ContactID",
                table: "Operators",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardID",
                table: "Transactions",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAunthifications_UserID",
                table: "UserAunthifications",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactID",
                table: "Users",
                column: "ContactID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "2stepCodes");

            migrationBuilder.DropTable(
                name: "Cookieformanagers");

            migrationBuilder.DropTable(
                name: "CookieforUsers");

            migrationBuilder.DropTable(
                name: "CookiesForOperators");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ManagerAuthifications");

            migrationBuilder.DropTable(
                name: "OperatorAunthifications");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserAunthifications");

            migrationBuilder.DropTable(
                name: "ValuteCurses");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
