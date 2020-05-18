using Economic_v2.Builders;
using Economic_v2.DataBaseLayer;
using Economic_v2.Models;
using Economic_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Economic_v2.Logic
{
    public class Calculator
    {
        private DateTime CurDateTime = DateTime.Now;
        private int month;

        public Calculator()
        {

        }

        public void MakeTransaction(User user, double spend)
        {
            user.Total_money -= spend;
            Save(user); Set(user); Update();
        }

        public void Calculate(User user)
        {
            if (user.Last_activity.Date < CurDateTime.Date)
            {
                month = CurDateTime.Month - user.Last_activity.Month +
                12 * (CurDateTime.Year - user.Last_activity.Year);

                Task taskTotalMoney = new Task(() =>
                {
                    user.Total_money += CalculateIncomes(user) - CalculateCategories(user);
                });

                Task taskReservedMoney = new Task(() =>
                {
                    CalculateTargets(ref user);
                });

                taskTotalMoney.Start(); taskReservedMoney.Start();

                Task.WaitAll(taskTotalMoney, taskReservedMoney);
                Set(user);
                Save(user);
            }
            Update();
        }

        private void Save(User user)
        {
            new Task(() =>
            {
                UnitOfWorkSingleton.GetUnitOfWork.Users.Update(user);
                UnitOfWorkSingleton.GetUnitOfWork.Save();
            }).Start();
        }

        private void Set(User user)
        {
            user.Last_activity = CurDateTime;
            MainViewModel.GetContext.CurrentUser = user;
        }

        private void Update()
        {
            MainViewModel.GetContext.UpdateInfo();
        }

        private int GetIncomeDay(DateTime dateTime, int day)
        {
            if (dateTime.Day != DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                while (dateTime.Day != day) { dateTime = dateTime.AddDays(1); }
            }
            return dateTime.Day;
        }

        private DateTime GetDateWithRightDay(DateTime dateTime, int day)
        {
            if (day > 27)
                if (dateTime.Day != DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
                {
                    while (dateTime.Day != day) { dateTime = dateTime.AddDays(1); }
                }
            return dateTime;
        }

        private double CalculateIncomes(User user)
        {
            double totalIncome = 0;
            int SavedIncomeDay, nowDay = CurDateTime.Day;
            DateTime marker;
            if (user.Incomes != null)
            {
                if (user.Last_activity.Date != CurDateTime.Date)
                {
                    foreach (Income income in user.Incomes)
                    {
                        SavedIncomeDay = income.Date.Day;
                        marker = DateTime.Parse(user.Last_activity.ToShortDateString());

                        while (marker.Day != SavedIncomeDay)
                        {
                            marker = marker.AddDays(1);
                            SavedIncomeDay = GetIncomeDay(marker, income.Date.Day);
                        }

                        while (marker <= CurDateTime)
                        {
                            totalIncome += income.Money;
                            marker = GetDateWithRightDay(marker.AddMonths(1), nowDay);
                        }
                    }
                }
            }
            return totalIncome;
        }

        private double CalculateCategories(User user)
        {
            double monthlyExpenses = 0;

            if (user.Categories != null)
            {
                foreach (Category cat in user.Categories)
                {
                    monthlyExpenses += cat.Spend;
                }
            }

            double totalSpend = month * monthlyExpenses;
            return totalSpend;
        }

        private void CalculateTargets(ref User user)
        {
            if (user.ActiveTargets != null)
            {
                TargetCalcController taskCacl = new TargetCalcController();
                double reservedSum = 0;
                ActiveTarget temp;
                List<ActiveTarget> targets = new List<ActiveTarget>();
                List<OldTarget> oldTargets = new List<OldTarget>();
                foreach (ActiveTarget target in user.ActiveTargets)
                {
                    temp = taskCacl.CalcTarget(target, month, ref reservedSum);
                    if (temp.TotalSum == temp.CurrentSum)
                    {
                        oldTargets.Add(temp.CopyTo<OldTarget>(null));
                    }
                    else
                    {
                        targets.Add(temp);
                    }
                }

                foreach(OldTarget target in oldTargets)
                {
                    user.OldTargets.Add(target);
                }

                user.ActiveTargets = targets;

                if (user.SuspendedTargets != null)
                {
                    foreach (SuspendedTarget target in user.SuspendedTargets)
                    {
                        reservedSum += target.CurrentSum;
                    }
                }

                user.Reserve_money = reservedSum;
            }
        }
    }
}