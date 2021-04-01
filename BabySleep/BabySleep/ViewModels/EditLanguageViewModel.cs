using BabySleep.Models;
using BabySleep.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BabySleep.Resx;
using BabySleep.Application.Interfaces;
using Autofac;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Edit Language page
    /// </summary>
    public class EditLanguageViewModel : INotifyPropertyChanged
    {
        public EditLanguageViewModel()
        {
            languageService = App.Container.Resolve<IAppLanguageService>();

            InitLanguages();
        }

        public ObservableCollection<LanguageModel> Languages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private readonly IAppLanguageService languageService;
        LanguageModel selectedLanguage;
        public LanguageModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLanguage)));

                SelectLanguage();
            }
        }
        #endregion

        #region Private Methods
        private void InitLanguages()
        {
            Languages = new ObservableCollection<LanguageModel>()
            {
                new LanguageModel(){ Id = LanguageType.en, Title = EditSettingsResources.English},
                new LanguageModel(){ Id = LanguageType.uk, Title = EditSettingsResources.Ukrainian},
                new LanguageModel(){ Id = LanguageType.ru, Title = EditSettingsResources.Russian}
            };

            SelectedLanguage = Languages.FirstOrDefault(l =>
                Enum.GetName(typeof(LanguageType), l.Id) == languageService.GetAppLanguage());
        }

        private void SelectLanguage()
        {
            var language = Enum.GetName(typeof(LanguageType), selectedLanguage.Id);
            if (App.SetLanguage(language))
            {
                languageService.SetAppLanguage(language);
                App.Current.MainPage = new MasterPage(new EditLanguagePage());
            }
        }
        #endregion
    }
}
