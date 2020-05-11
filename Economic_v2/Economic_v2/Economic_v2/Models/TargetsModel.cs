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
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public TargetsModel(object context)
        {
            DataContexts[0] = (new AddOrEditTarget()).DataContext;
            DataContexts[1] = (new TargetsListView()).DataContext;
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

        public void NotifyEndableButtonsEditAndDelete()
        {
            PageContext.NotifyEndableButtonsEditAndDelete();
        }

        public bool DismissDelete;
        public Target DeletedTarget;
        public void DeleteTarget()
        {
            new Task(() =>    //start method in context
            {
                ListContext.OnDeleteTarget(
                    new TargetBuilder(DeletedTarget).Build());
                DeletedTarget = null;
            }).Start();
        }

        public void ConfirmButton(bool isEdit)
        {
            new Task(() =>
            {
                if (MainViewModel.CurrentUser.ActiveTargets == null)  //if current not any note
                {
                    MainViewModel.CurrentUser.ActiveTargets = new List<ActiveTarget>();
                }
                ActiveTarget activeTarget =
                AddOrEditContext.GetTarget.CopyTo<ActiveTarget>(null);  //convert to needed type
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.CurrentUser.ActiveTargets[MainViewModel.
                            CurrentUser.ActiveTargets.FindIndex(x => x.Id == activeTarget.Id)] = activeTarget;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.CurrentUser.ActiveTargets.Add(activeTarget);
                    }
                }
                else                //make new
                {
                    MainViewModel.CurrentUser.ActiveTargets.Add(activeTarget);
                }
                Data.Users.Update(MainViewModel.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}
