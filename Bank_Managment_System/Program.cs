using Bank_Managment_System.Controllers.Mediators.Atm;
using Bank_Managment_System.Controllers.Mediators.Error;
using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Controllers.Mediators.Manager;
using Bank_Managment_System.Controllers.Mediators.Operator;
using Bank_Managment_System.Controllers.Mediators.RegisterUser;
using Bank_Managment_System.Models;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ATM;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Services.Interfaces;
using Bank_Managment_System.Services.InternetBankServices;
using Bank_Managment_System.Services.Reports;
using Bank_Managment_System.Services.SystemServices;
using Microsoft.EntityFrameworkCore;
using Bank_Managment_System.Controllers.Mediators.Errori;
using Bank_Managment_System.Controllers.Mediators.Logi;
using Bank_Managment_System.Controllers.Mediators.Log;
using Bank_Managment_System.Controllers.Mediators.Report;
using Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator;
using Bank_Managment_System.Settings.Reflections.ServiceInject;
using System.Reflection;
using Bank_Managment_System.Settings.Reflections.RepositInject;
using Bank_Managment_System.Settings.HandlerInject;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); //umoqmedobis periodshi  sesias gauva vada 20 wutshi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

//builder.Services.AddScoped<IAtm, AtmServices>();
//builder.Services.AddScoped<Ierror, ErrorService>();
//builder.Services.AddScoped<Ilog, LoggerServices>();
//builder.Services.AddScoped<Ioperator, OperatorService>();
//builder.Services.AddScoped<Imanager, OperatorService>();
//builder.Services.AddScoped<IregisterUserService, RegisterUserServices>();
//builder.Services.AddScoped<Ireport, ReportServices>();
//builder.Services.AddScoped<IuserServicePerformedByOperator, UserServicesPerformedByoperator>();


////operator
//builder.Services.AddTransient<IcomandHandler<OperatorRequest>, SignUpOperatorCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<OperatorSignInRequest>, SignInOperatorCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<string>, TwostepForOperatorCmdHandler>();
//builder.Services.AddTransient<IcomandHandlerOperatorsignOut, SignOutOperatorCmdHandler>();
//builder.Services.AddTransient<IcomandHandlerValute, InicializeValuteCmdHandler>();

////manager 
//builder.Services.AddTransient<IcomandHandler<ManagerSignInRequest>, SignInmanagerCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<ManagerSignuprequest>, SignUpManagerCmdHandler>();
//builder.Services.AddTransient<IcomandHandlerManagerSIgnOut, SignOutManagerCmdHandler>();

////ATM
//builder.Services.AddTransient<IcomandHandler<ChangePinRequest>, changePinCmdHandler>();
//builder.Services.AddTransient<IcomandHandleForStringReturn<AuthorizedRequest>, checkBalanceCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<withdrawalRequest>, WithdrawMoneyCmdHandler>();

////registerUSer
//builder.Services.AddTransient<IcomandhandlerList<CardAndAccountResponse, object>, CardAndAccountResponseCmdHandler<CardAndAccountResponse, object>>();
//builder.Services.AddTransient<IcomandhandlerList<tRansresponse, GettransactionByIttypeReq>, GetTransactionsByItTypeCmdHandler<tRansresponse, GettransactionByIttypeReq>>();
//builder.Services.AddTransient<IcomandHandler<TransferToOwnAccountRequest>, SendMoneytoOwnAccountCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<TransferMoneyTosomeoneelserequest>, SendMoneyToSomeoneElsesCmdHanlder>();
//builder.Services.AddTransient<IcomandHandler<SignInUserrequest>, SignInUserrequestCmdHandler>();
//builder.Services.AddTransient<IcomandHandlerSignoutUser, signOutUserCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<twostepuserrequest>, TwoStepUserCmdHandler>();

////error
//builder.Services.AddTransient<IcomandhandlerList<Error, object>, GetAllErrorsCmdHandler<Error, object>>();
//builder.Services.AddTransient<IcomandhandlerList<Error, GeterrorBydateRequest>, GetErrorByDateCmdHandler<Error, GeterrorBydateRequest>>();
//builder.Services.AddTransient<IcomandhandlerList<Error, GeterrorbytypeRequest>,GeterrorbytypeCmdHandler<Error, GeterrorbytypeRequest>>();


////log
//builder.Services.AddTransient<IcomandhandlerList<Log, object>, GetallLogCmdHandler<Log, object>>();
//builder.Services.AddTransient<IcomandhandlerList<Log, GetLogByItTyperequest>,GetAllLogsByItTypeCmdHandler<Log, GetLogByItTyperequest>>();
//builder.Services.AddTransient<IcomandhandlerList<Log, GetlogBydaterequest>, GetLogsByDateCmdHandler<Log, GetlogBydaterequest>>();

////report 
//builder.Services.AddTransient< IcomandreportHandler<GettransactionchartRequest, Dictionary<DateTime, int>>, GetTransactionChartCmdHandler<GettransactionchartRequest, Dictionary<DateTime, int>>>();
//builder.Services.AddTransient<IcomandreportHandler<GettransactionStatsRequest, TransactionStatsResponse>, TransactionstatsCmdHandler<GettransactionStatsRequest, TransactionStatsResponse>>();
//builder.Services.AddTransient<IcomandreportHandler<UserStatrequest, UserStatsResponse>, UserstatsCmdHandler<UserStatrequest, UserStatsResponse>>();

////operations performedbyoperator
//builder.Services.AddTransient<IcomandHandler<Bankaccountcreationrequestincontroller>, CreateBankAccountCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<CardCreationrequestincontroller>, CreateCardForBankCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<ParmanentlyDelete>, ParmanentlydeleteCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<UserRequet>, RegisterUserCmdHandler>();
//builder.Services.AddTransient<IcomandHandler<SoftDeleteCardrequest>, SoftDeleteCmdhandler>();
//builder.Services.AddTransient<IcomandHandler<UpdateCardValidityRequest>, UpdateCardValidityCmdHandler>();

builder.Services.InjectService(Assembly.GetExecutingAssembly());
builder.Services.ReposInjecti(Assembly.GetExecutingAssembly());
builder.Services.CmdhandlerInject(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<BankDb>(io =>
{
    io.UseSqlServer(builder.Configuration.GetConnectionString("GugasConnect"));
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())//kodi romelic  gvinda gaeshvas chatvirtvisas
{
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Environment.ApplicationName = "Guga's Online Bank";
    app.Environment.EnvironmentName = "Guga's Bank System";

}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();
app.UseCookiePolicy();
app.MapControllers();
app.Run();
