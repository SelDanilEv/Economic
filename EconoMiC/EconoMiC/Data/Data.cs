using EconoMiC.Calendar;
using EconoMiC.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Data
{
    class Data
    {
        public static DataTable Select(string selectSQL) // функция подключения к базе данных и обработка запросов
        {
            DataTable dataTable = new DataTable("dataBase");                // создаём таблицу в приложении
                                                                            // подключаемся к базе данных
            SqlConnection sqlConnection = new SqlConnection("server=DEFENDER-SD\\MSSQLSERVERSEC;Trusted_Connection=Yes;DataBase=EconoMiC;");
            sqlConnection.Open();                                           // открываем базу данных
            SqlCommand sqlCommand = sqlConnection.CreateCommand();          // создаём команду
            sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
            sqlDataAdapter.Fill(dataTable);                                 // возращаем таблицу с результатом
            return dataTable;
        }

        public static bool validate(string log, string pass)   // check login and password
        {
            DataTable dt_user = Select($"select top(1) id from [dbo].[Users] where LOGIN='{log}' and PASSWORD='{pass}'"); // получаем данные из таблицы
            if (dt_user.Rows.Count == 0)
                return false;
            return true;
        }

        static public User GetUser(string log,string pass)
        {
            DataTable dt_user = Select($"select top(1) * from [dbo].[Users] where LOGIN='{log}' and PASSWORD='{pass}'"); // получаем данные из таблицы
            SomeDate someDate = new SomeDate();
            someDate.FromDateToSomeDate(DateTime.Parse(dt_user.Rows[0][4].ToString()));
            return new User(dt_user.Rows[0][1].ToString(),dt_user.Rows[0][2].ToString(),(double)dt_user.Rows[0][3],someDate); // выводим данные
        }

        //public static bool addUser(User user)
        //{
        //    if (!accs.Exists(x => x.Login == user.Login))
        //    {
        //        accs.Add(user);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
