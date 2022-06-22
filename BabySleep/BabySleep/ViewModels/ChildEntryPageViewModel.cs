using BabySleep.Helpers;
using BabySleep.Resx;
using BabySleep.Services;
using BabySleep.Validations;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using BabySleep.Views;
using BabySleep.Application.Interfaces;
using Autofac;
using BabySleep.Application.DTO;
using BabySleep.Common.Exceptions.Child;
using MarcTron.Plugin;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Page for adding/editing children
    /// </summary>
    public class ChildEntryPageViewModel : INotifyPropertyChanged
    {

        public ChildEntryPageViewModel(Guid childGuid) : this()
        {
            InitService();

            var child = childService.GetChild(childGuid);
            ChildGuid = child.ChildGuid;
            Name = new ValidatableObject<string>()
            {
                Value = child.Name
            };
            BirthDate = child.BirthDate;
            BirthWeek = new Validations.ValidatableObject<short?>()
            {
                Value = child.BirthWeek
            };
            Picture = child.Picture;

            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");

            //CrossMTAdmob.Current.OnInterstitialLoaded += (sender, args) =>
            //{
            //    CrossMTAdmob.Current.ShowInterstitial();
            //};
        }

        public ChildEntryPageViewModel()
        {
            InitService();

            birthDate = (birthDate < MinimumDate) ? MaximumDate : birthDate;
            mainLabelText = ChildEntryResources.AddNewChild;
            isDeleteVisible = childService.GetChildrenCount() > 1;
            updateColumnSpan = isDeleteVisible ? 1 : 2;

            SaveChildCommand = new Command(SaveChild);
            DeleteChildCommand = new Command(DeleteChild);
            AddPictureCommand = new Command(AddPicture);

            AddValidations();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private IChildService childService;

        ValidatableObject<string> name = new ValidatableObject<string>();
        public ValidatableObject<string> Name
        {
            get => name;
            set
            {
                name = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        DateTime birthDate;
        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                birthDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BirthDate)));
            }
        }

        byte[] picture;
        public byte[] Picture
        {
            get => picture;
            set
            {
                picture = value;
                IsPictureVisible = picture != null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Picture)));
            }
        }

        private bool isPictureVisible = false;
        public bool IsPictureVisible
        {
            get => isPictureVisible;
            set
            {
                isPictureVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPictureVisible)));
            }
        }

        private bool isNew = true;
        public bool IsNew
        {
            get => isNew;
            set
            {
                isNew = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNew)));
            }
        }

        private Guid childGuid;
        public Guid ChildGuid
        {
            get => childGuid;
            set
            {
                childGuid = value;
                mainLabelText = ChildEntryResources.EditChild;
                isNew = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChildGuid)));
            }
        }

        private string mainLabelText;
        public string MainLabelText
        {
            get => mainLabelText;
            set
            {
                mainLabelText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainLabelText)));
            }
        }

        private ValidatableObject<short?> birthWeek = new ValidatableObject<short?>();
        public ValidatableObject<short?> BirthWeek
        {
            get => birthWeek;
            set
            {
                birthWeek = value;
                isPrematureBorn = birthWeek != null && birthWeek.Value != null;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BirthWeek)));
            }
        }

        private bool isPrematureBorn;
        public bool IsPrematureBorn
        {
            get => isPrematureBorn;
            set
            {
                isPrematureBorn = value;
                if(!isPrematureBorn)
                {
                    BirthWeek.IsValid = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPrematureBorn)));
            }
        }

        private bool isDeleteVisible;
        public bool IsDeleteVisible
        {
            get => isDeleteVisible;
            set
            {
                isDeleteVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDeleteVisible)));
            }
        }
        private int updateColumnSpan;
        public int UpdateColumnSpan
        {
            get => updateColumnSpan;
            set
            {
                updateColumnSpan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UpdateColumnSpan)));
            }
        }

        public DateTime MinimumDate => DateTime.Now.AddYears(-Common.Helpers.Constants.MAX_YEARS);
        public DateTime MaximumDate => DateTime.Now;

        public int NameLength = Common.Helpers.Constants.NAME_LENGTH;
        #endregion

        #region Commands
        public Command SaveChildCommand { get; }
        public Command DeleteChildCommand { get; }
        public Command AddPictureCommand { get; }
        #endregion

        #region Private Methods
        private void InitService()
        {
            if (childService == null)
            {
                childService = App.Container.Resolve<IChildService>();
            }
        }

        private bool AreFieldsValid()
        {
            bool isFirstNameValid = Name.Validate();
            bool isBirthWeekValid = !IsPrematureBorn || BirthWeek.Validate();

            return isFirstNameValid && isBirthWeekValid;
        }

        private Task ReloadMainPage()
        {
            ReloadChildrenList();
            return App.NavigateMasterDetailPop();
        }

        private void ReloadChildrenList()
        {
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_CHILDREN_POPUP);
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_MENU);
        }

        private async void AddPicture()
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    Picture = ms.ToArray();
                }
            }
        }

        private async void DeleteChild()
        {
            {
                try
                {
                    var result = await ShowQuestion(ChildEntryResources.DeleteChildQuestion);
                    if (result)
                    {
                        childService.DeleteChild(childGuid);
                        await ReloadMainPage();
                    }
                }
                catch(DeleteLastChildException)
                {
                    await ShowException(ChildEntryResources.DeleteLastChildException);
                }
                catch(Exception ex)
                {
                    await ShowException(ex.Message);
                }
            }
        }

        private async void SaveChild()
        {
            if (!AreFieldsValid())
            {
                return;
            }

            var isNewPage = childService.GetChildrenCount() == 0;

            try
            {
                var child = new ChildDto()
                {
                    ChildGuid = childGuid,
                    BirthDate = birthDate,
                    BirthWeek = isPrematureBorn ? birthWeek.Value : null,
                    Name = name.Value,
                    Picture = picture
                };
                childService.SaveChild(child);

                CrossMTAdmob.Current.ShowInterstitial();

                if (isNewPage)
                {
                    ReloadChildrenList();
                    App.Current.MainPage = new MasterPage();
                }
                else
                {
                    await ReloadMainPage();
                }
            }
            catch (ChildNameEmptyException)
            {
                await ShowException(ChildEntryResources.NameRequired);
            }
            catch(ChildNameLengthException)
            {
                await ShowException(string.Format(ChildEntryResources.NameMaxLength, Common.Helpers.Constants.NAME_LENGTH));
            }
            catch(ChildAgeException)
            {
                await ShowException(ChildEntryResources.ChildAgeRange);
            }
            catch(ChildPrematureBirthWeekException)
            {
                await ShowException(string.Format(ChildEntryResources.BirthWeekRange, 
                    Common.Helpers.Constants.BIRTH_WEEK_MIN_VALUE, Common.Helpers.Constants.BIRTH_WEEK_MAX_VALUE));
            }
            catch (ChildAlreadyExistsException)
            {
                await ShowException(ChildEntryResources.ChildAlreadyExistsException);
            }
            catch (ChildLimitException)
            {
                await ShowException(ChildEntryResources.ChildLimitException);
            }
            catch (Exception ex)
            {
                await ShowException(ex.Message);
            }
        }

        private void AddValidations()
        {
            if (!name.Validations.Any())
            {
                Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ChildEntryResources.NameRequired });
            }

            if (!birthWeek.Validations.Any())
            {
                BirthWeek.Validations.Add(new BirthWeekRule<short?> { ValidationMessage = 
                    string.Format(ChildEntryResources.BirthWeekRange, Common.Helpers.Constants.BIRTH_WEEK_MIN_VALUE, Common.Helpers.Constants.BIRTH_WEEK_MAX_VALUE) });
            }
        }

        private Task ShowException(string message)
        {
            return ((App)Xamarin.Forms.Application.Current).ShowException(MainLabelText, message);
        }

        private Task<bool> ShowQuestion(string message)
        {
            return ((App)Xamarin.Forms.Application.Current).ShowQuestion(MainLabelText, message);
        }
        #endregion
    }
}
