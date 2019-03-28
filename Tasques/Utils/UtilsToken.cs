using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasques.Models;

namespace Tasques.Utils
{
    public class UtilsToken
    {
        public  static tasquesEntities db = new tasquesEntities();
        public static CurrentUser isValidUser(string name, string password)
        {
            try
            {
                
                var logUser = (from us in db.USERS
                              join ru in db.USERROL on us.IDUSER equals ru.IDUSUARIO
                              //join r in db.ROLS on ru.IDROL equals r.NAME               
                              where us.NAME.Equals(name)
                              where us.PASSWORD.Equals(password)
                              select new { name = us.NAME, password = us.PASSWORD, surname = us.SURNAMES, rol = ru.IDROL, token = us.TOKEN }).ToList();
                if (logUser.Count()>0)
                {
                    return new CurrentUser(logUser[0].name, logUser[0].password, logUser[0].surname, logUser[0].rol, logUser[0].token);
                }
                else
                {
                    return null;
                }
                

            }
            catch (Exception)
            {
                throw;
                return null;
                
            }
        }
        public static CurrentUser isValidToken(string token)
        {
            try
            {

                var logUser = (from us in db.USERS
                               join ru in db.USERROL on us.IDUSER equals ru.IDUSUARIO
                               //join r in db.ROLS on ru.IDROL equals r.NAME               
                               where us.TOKEN.Equals(token)
                               select new { name = us.NAME, password = us.PASSWORD, surname = us.SURNAMES, rol = ru.IDROL, token = us.TOKEN }).ToList();
                if (logUser != null)
                {
                    return new CurrentUser(logUser[0].name, logUser[0].password, logUser[0].surname, logUser[0].rol, logUser[0].token);
                }
                else
                {
                    return null;
                }


            }
            catch (Exception)
            {
                throw;
                return null;

            }
        }
        public static bool Isadmin(string token)
        {
            var logUser = (from us in db.USERS
                           join ru in db.USERROL on us.IDUSER equals ru.IDUSUARIO
                           //join r in db.ROLS on ru.IDROL equals r.NAME               
                           where us.TOKEN.Equals(token)
                           select new { name = us.NAME, password = us.PASSWORD, surname = us.SURNAMES, rol = ru.IDROL, token = us.TOKEN }).ToList();
            if (logUser[0].rol == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static int getUserId(string token)
        {
            var userID = (from us in db.USERS
                          where us.TOKEN == token
                          select us).ToList();
            return userID[0].IDUSER;
        }
    }
    
}