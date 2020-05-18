using Economic_v2.Builders;
using Economic_v2.DataBaseLayer;
using Economic_v2.Pages;
using Economic_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class TargetsModel
    {
        object[] DataContexts = new object[3];         //here data context all controls where was Target
        object[] Views = new object[3];         //here Views

        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public TargetsModel(object context)
        {
            Views[0] = new AddOrEditTarget();
            Views[1] = new TargetsListView();
            DataContexts[0] = ((AddOrEditTarget)Views[0]).DataContext;
            DataContexts[1] = ((TargetsListView)Views[1]).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public AddOrEditTargetsViewModel AddOrEditContext
        {
            get => (AddOrEditTargetsViewModel)DataContexts[0];
        }

        public TargetsListViewViewModel ListContext
        {
            get => (TargetsListViewViewModel)DataContexts[1];
        }

        public TargetsPageViewModel PageContext
        {
            get => (TargetsPageViewModel)DataContexts[2];
        }
        #endregion

        #region Views
        public AddOrEditTarget AddOrEditView
        {
            get => (AddOrEditTarget)Views[0];
        }

        public TargetsListView ListView
        {
            get => (TargetsListView)Views[1];
        }

        public TargetsPage PageView
        {
            get => (TargetsPage)Views[2];
        }
        #endregion

        public void NotifyEndableButtonsEditAndDelete()
        {
            PageContext.NotifyEndableButtonsEditAndDelete();
        }

        public bool DismissDelete;
        public Target DeletedTarget;
        public int deleteMode;
        public void DeleteTarget()
        {
            new Task(() =>    //start method in context
            {
                ListContext.OnDeleteTarget(
                    new TargetBuilder(DeletedTarget).Build(),deleteMode);
                DeletedTarget = null;
                deleteMode = -1;
            }).Start();
        }

        public void ConfirmButton(bool isEdit, int mode)
        {
            new Task(() =>
            {

                if (MainViewModel.GetContext.CurrentUser.ActiveTargets == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.ActiveTargets = new List<ActiveTarget>();
                }
                if (MainViewModel.GetContext.CurrentUser.SuspendedTargets == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.SuspendedTargets = new List<SuspendedTarget>();
                }
                if (MainViewModel.GetContext.CurrentUser.OldTargets == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.OldTargets = new List<OldTarget>();
                }
                Target target = AddOrEditContext.GetTarget.CopyTo<Target>(null);  //convert to needed type
                double newSum = target.CurrentSum;
                double oldSum = 0;
                bool flagCreateNew = false;

                switch (mode)
                {
                    case 0:
                        if (isEdit && target.Id != 0)
                        {
                            oldSum = MainViewModel.GetContext.CurrentUser.ActiveTargets.Find(x => x.Id == target.Id).CurrentSum;

                            if (target.CurrentSum == target.TotalSum)
                            {
                                MainViewModel.GetContext.CurrentUser.ActiveTargets.RemoveAll(x => x.Id == target.Id);
                                MainViewModel.GetContext.CurrentUser.OldTargets.Add(target.CopyTo<OldTarget>(null));

                            }
                            else
                            {
                                if (target.Spend == 0)
                                {
                                    MainViewModel.GetContext.CurrentUser.ActiveTargets.RemoveAll(x => x.Id == target.Id);
                                    MainViewModel.GetContext.CurrentUser.SuspendedTargets.Add(target.CopyTo<SuspendedTarget>(null));
                                }
                                else
                                {
                                    MainViewModel.GetContext.CurrentUser.ActiveTargets[MainViewModel.GetContext.
                                         CurrentUser.ActiveTargets.FindIndex(x => x.Id == target.Id)] = target.CopyTo<ActiveTarget>(null);
                                }
                            }
                        }
                        else
                        {
                            flagCreateNew = true;
                        }
                        break;
                    case 1:
                        if (isEdit && target.Id != 0)
                        {
                            oldSum = MainViewModel.GetContext.CurrentUser.SuspendedTargets.Find(x => x.Id == target.Id).CurrentSum;

                            if (target.CurrentSum == target.TotalSum)
                            {
                                MainViewModel.GetContext.CurrentUser.SuspendedTargets.RemoveAll(x => x.Id == target.Id);
                                MainViewModel.GetContext.CurrentUser.OldTargets.Add(target.CopyTo<OldTarget>(null));

                            }
                            else
                            {
                                if (target.Spend == 0)
                                {
                                    MainViewModel.GetContext.CurrentUser.SuspendedTargets[MainViewModel.GetContext.
                                        CurrentUser.SuspendedTargets.FindIndex(x => x.Id == target.Id)] = target.CopyTo<SuspendedTarget>(null);
                                }
                                else
                                {
                                    MainViewModel.GetContext.CurrentUser.SuspendedTargets.RemoveAll(x => x.Id == target.Id);
                                    MainViewModel.GetContext.CurrentUser.ActiveTargets.Add(target.CopyTo<ActiveTarget>(null));
                                }
                            }
                        }
                        else
                        {
                            flagCreateNew = true;
                        }
                        break;
                    case 2:
                        if (isEdit && target.Id != 0)
                        {
                            oldSum = MainViewModel.GetContext.CurrentUser.OldTargets.Find(x => x.Id == target.Id).CurrentSum;

                            if (target.CurrentSum == target.TotalSum)
                            {
                                MainViewModel.GetContext.CurrentUser.OldTargets[MainViewModel.GetContext.
                                        CurrentUser.OldTargets.FindIndex(x => x.Id == target.Id)] = target.CopyTo<OldTarget>(null);
                            }
                            else
                            {
                                if (target.Spend == 0)
                                {
                                    MainViewModel.GetContext.CurrentUser.OldTargets.RemoveAll(x => x.Id == target.Id);
                                    MainViewModel.GetContext.CurrentUser.SuspendedTargets.Add(target.CopyTo<SuspendedTarget>(null));
                                }
                                else
                                {
                                    MainViewModel.GetContext.CurrentUser.OldTargets.RemoveAll(x => x.Id == target.Id);
                                    MainViewModel.GetContext.CurrentUser.ActiveTargets.Add(target.CopyTo<ActiveTarget>(null));
                                }
                            }
                        }
                        else
                        {
                            flagCreateNew = true;
                        }
                        break;
                }
                if (flagCreateNew)
                {
                    if (target.CurrentSum == target.TotalSum)
                    {
                        MainViewModel.GetContext.CurrentUser.OldTargets.Add(target.CopyTo<OldTarget>(null));
                    }
                    else
                    {
                        if (target.Spend == 0)
                        {
                            MainViewModel.GetContext.CurrentUser.SuspendedTargets.Add(target.CopyTo<SuspendedTarget>(null));
                        }
                        else
                        {
                            MainViewModel.GetContext.CurrentUser.ActiveTargets.Add(target.CopyTo<ActiveTarget>(null));
                        }
                    }
                }

                ListContext.SelectedTarget = null;
                MainViewModel.GetContext.CurrentUser.Reserve_money += newSum - oldSum;
                ListContext.NotifyTargetList();
                MainViewModel.GetContext.UpdateInfo();

                if (StatisticViewModel.GetContext != null)
                    StatisticViewModel.GetContext.MakeCalculate();

                Data.Users.Update(MainViewModel.GetContext.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}
