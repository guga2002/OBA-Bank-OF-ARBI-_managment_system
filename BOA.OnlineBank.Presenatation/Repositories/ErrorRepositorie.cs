
using Bank_Managment_System.Models;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.EntityFrameworkCore;

namespace BOA.OnlineBank.Presenatation.Repositories
{
    public class ErrorRepositorie: IErrorRepos
    {
        private readonly BankDb _database;

        public ErrorRepositorie(BankDb bank)
        {
            _database = bank;
        }
        #region Error
        public bool Error(string context, ErrorEnum lvl)
        {
            //  try
            //{
            _database._errors.Add(new Error()
            {
                Description = context,
                TimeOfocured = DateTime.Now,
                errortype = lvl,
            });

            _database.SaveChanges();
            return true;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        #endregion

        #region GeterrorBytype
        public async Task<List<Error>> GeterrorBytype(GeterrorbytypeRequest enn)
        {
            var lst = _database._errors.Where(io => io.errortype == enn.en).ToListAsync();
            if (lst != null)
            {
                return await lst;
            }
            return null;
        }
        #endregion

        #region GetAllErrors
        public async Task<List<Error>> GetAllErrors()
        {
            return await _database._errors.ToListAsync();
        }
        #endregion

        #region GetErrorBydate
        public async Task<List<Error>> GetErrorBydate(GeterrorBydateRequest en)
        {
            return await _database._errors.Where(io => io.TimeOfocured >= en.start && io.TimeOfocured <= en.end).ToListAsync();
        }
        #endregion
    }
}
