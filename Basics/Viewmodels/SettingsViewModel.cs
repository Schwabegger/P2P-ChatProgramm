// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Commands;
using Basics.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Basics.Viewmodels
{
    public class SettingsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region Field
        //string _pfp = pfpSelection[selectedPfp];
        private string nameSettings = Properties.Settings.Default.Name;
        private string _pfp = Properties.Settings.Default.Pfp;
        string _indexPicture = (selectedPfp + 1).ToString() + "/" + pfpSelection.Length;
        private string theme;
        #endregion

        //public event EventHandler ChangeLanguage;

        #region Commands
        public DelegateCommand Next { get; set; }
        public DelegateCommand Last { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ThemeCommand { get; set; }
        #endregion

        #region Properties
        public string NameSettings
        {
            get => nameSettings;
            set
            {
                nameSettings = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string ProfilePicture
        {
            get => _pfp;
            set
            {
                if (!string.IsNullOrEmpty(value) && value != _pfp)
                {
                    _pfp = value;
                    RaisePropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string WhichPicture
        {
            get => _indexPicture;
            set
            {
                if (value != _indexPicture)
                {
                    _indexPicture = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Theme
        {
            get => theme;
            set
            {
                if (value != theme)
                {
                    theme = value;
                    this.RaisePropertyChanged();
                    this.SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<CustomComboboxItem> languageeCollection;

        public ObservableCollection<CustomComboboxItem> LanguageCollection
        {
            get { return languageeCollection; }
            set { languageeCollection = value; }
        }

        private int languageComboBoxSelectedIndex;

        public int LanguageComboBoxSelectedIndex
        {
            get { return languageComboBoxSelectedIndex; }
            set
            {
                languageComboBoxSelectedIndex = value;
                RaisePropertyChanged();
                SaveCommand?.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public SettingsViewModel()
        {
            InitLanguageCombobox();
            SetLanguageComboboxOnStartup();
            this.SaveCommand = new DelegateCommand(
            _ =>
            {
                // save button is only enabled if something changed
                return !string.IsNullOrEmpty(this.NameSettings) && this.NameSettings != Properties.Settings.Default.Name
                    || !string.IsNullOrEmpty(this.ProfilePicture) && this.ProfilePicture != Properties.Settings.Default.Pfp
                    || this.LanguageComboBoxSelectedIndex != -1 && this.LanguageCollection[LanguageComboBoxSelectedIndex].LanguageCode != Properties.Settings.Default.Language
                    || Theme != null && !string.IsNullOrEmpty(this.Theme) && this.Theme != Properties.Settings.Default.Theme;
            },
            _ =>
            {
                // change name
                if (!string.IsNullOrEmpty(this.NameSettings) && this.NameSettings != Properties.Settings.Default.Name)
                {
                    Properties.Settings.Default.Name = this.NameSettings;
                }
                //change pfp
                if (!string.IsNullOrEmpty(this.ProfilePicture) && this.ProfilePicture != Properties.Settings.Default.Pfp)
                {
                    Properties.Settings.Default.Pfp = this.ProfilePicture;
                }
                // change language
                if (this.LanguageCollection[LanguageComboBoxSelectedIndex].LanguageCode != Properties.Settings.Default.Language)
                {
                    Properties.Settings.Default.Language = this.LanguageCollection[LanguageComboBoxSelectedIndex].LanguageCode;
                }
                // change theme
                if (!string.IsNullOrEmpty(this.Theme) && this.Theme != Properties.Settings.Default.Theme)
                {
                    Properties.Settings.Default.Theme = this.Theme;
                }
                // save changes
                Properties.Settings.Default.Save();
                SaveCommand.RaiseCanExecuteChanged();
            });

            // 
            this.ThemeCommand = new DelegateCommand((o) =>
            {
                string color = (string)o;
                Theme = color;
            });

            // shows the nex picture for selecting
            this.Next = new DelegateCommand(
            (o) =>
            {
                if (selectedPfp < 5)
                {
                    selectedPfp += 1;
                    WhichPicture = $"{selectedPfp + 1}/{pfpSelection.Length}";
                    ProfilePicture = pfpSelection[selectedPfp];
                }
            });

            // shows the previous picture for selecting
            this.Last = new DelegateCommand(
            (o) =>
            {
                if (selectedPfp > 0)
                {
                    selectedPfp -= 1;
                    WhichPicture = $"{selectedPfp + 1}/{pfpSelection.Length}";
                    ProfilePicture = pfpSelection[selectedPfp];
                }
            });
        }

        /// <summary>
        /// Sets the selected combobox item
        /// </summary>
        private void SetLanguageComboboxOnStartup()
        {
            LanguageComboBoxSelectedIndex = 0;
            for (int i = 0; i < LanguageCollection.Count; i++)
                if (LanguageCollection[i].LanguageCode == Properties.Settings.Default.Language)
                    LanguageComboBoxSelectedIndex = i;
        }

        /// <summary>
        /// Adds all language options to the list the combobox item source is bound to
        /// </summary>
        private void InitLanguageCombobox()
        {
            LanguageCollection = new ObservableCollection<CustomComboboxItem>();
            LanguageCollection.Add(new CustomComboboxItem() { Flag = "https://upload.wikimedia.org/wikipedia/en/thumb/b/ba/Flag_of_Germany.svg/1200px-Flag_of_Germany.svg.png", LanguageCode = "de" });
            LanguageCollection.Add(new CustomComboboxItem() { Flag = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/30/Flag_of_the_United_Kingdom_%283-2_aspect_ratio%29.svg/1200px-Flag_of_the_United_Kingdom_%283-2_aspect_ratio%29.svg.png", LanguageCode = "en" });
        }
    }
}