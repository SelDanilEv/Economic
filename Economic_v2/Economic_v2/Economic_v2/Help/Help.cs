using Economic_v2.Commands;
using Economic_v2.DataBaseLayer;
using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Economic_v2.Help
{
    public class Helper
    {
        public static List<User> users;

        public static void GetUsers()
        {
            Task task = new Task(() =>
            {
                users = UnitOfWorkSingleton.
                GetUnitOfWork.Users.GetAll();
            });
            task.Start();
        }
    }

    public class CreateHelper
    {
        public static Target Target;
        public static bool IsValid;
    }
}