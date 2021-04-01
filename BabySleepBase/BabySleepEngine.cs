using BabySleepBase.Controllers;
using BabySleepBase.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BabySleepBase
{
    public class BabySleepEngine
    {
        BabySleepDatabase database;
        ChildController childController;
        SettingController settingController;

        #region Properties
        List<Child> children;
        public List<Child> Children
        {
            get => children;
            set
            {
                children = value;
            }
        }

        private Child selectedChild;
        public Child SelectedChild
        {
            get => selectedChild;
            set
            {
                selectedChild = value;
            }
        }

        Setting settings;
        public Setting Settings
        {
            get => settings;
            set
            {
                settings = value;
            }
        }
        #endregion

        public BabySleepEngine(string path)
        {
            children = new List<Child>();

            if (database == null)
            {
                database = new BabySleepDatabase(path);
            }
        }

        public void Initialize()
        {
            childController = new ChildController(database.DbConnection);
            settingController = new SettingController(database.DbConnection);

            ReloadChildren();
            ReloadSettings();
        }

        public void SaveChild(Child child)
        {
            childController.SaveChild(child).Wait();
            ReloadChildren();

            SelectedChild = children.FirstOrDefault(c => c.Name == child.Name);
        }

        public void DeleteChild(Child child)
        {
            childController.DeleteChild(child).Wait();
            ReloadChildren();
        }

        public void ReselectChild(Child child)
        {
            SelectedChild = children.FirstOrDefault(c => c.ChildGuid == child.ChildGuid);
        }

        private void ReloadChildren()
        {
            children = childController.GetChildren().Result;
            SelectedChild = children.OrderBy(c => c.Name).FirstOrDefault();
        }

        private void ReloadSettings()
        {
            settings = settingController.GetSettings().Result;
            if(settings == null)
            {
                var setting = new Setting()
                {
                    RowId = 0,
                    Language = "en"
                };
                settingController.CreateSettings(setting);
            }

            Resx.Resources.Culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                .FirstOrDefault(element => element.Name == settings.Language);
        }

        public void SaveSettings()
        {
            settingController.SaveSettings(settings).Wait();
            ReloadSettings();
        }
    }
}
