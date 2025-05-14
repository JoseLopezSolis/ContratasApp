using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
public partial class AddClientPageViewModel : BasePageViewModel
    {
        
        #region Observable properties
        
        [ObservableProperty] private int clientId;
        [ObservableProperty] private string name;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string phone;
        [ObservableProperty] private string email;
        [ObservableProperty] private string paymentMethod;
        [ObservableProperty] private string imagePath;
        [ObservableProperty] private ImageSource profileImage;
        
        #endregion

        #region Picker options
        public static IList<string> PaymentMethods { get; } =
            new List<string> { "Money", "Bank Transfer", "Money and Bank Transfer" };
        #endregion

        #region Services
        readonly IClientService _clientService;
        #endregion

        #region Constants
        const string DefaultImageFileName = "default_profile.png";
        private static readonly string DefaultPaymentMethod = PaymentMethods[2];
        #endregion

        #region Constructor
        public AddClientPageViewModel(
            IClientService clientService,
            INavigationService navigationService) 
            : base(navigationService)
        {
            _clientService = clientService;

            // Inicializa imagen por defecto
            ImagePath = DefaultImageFileName;
            ProfileImage = ImageSource.FromFile(DefaultImageFileName);
        }
        #endregion

        #region Relays commands

        /// <summary>
        /// Open gallery, copy the picture to AppData and update ImagePath
        /// </summary>
        [RelayCommand]
        async Task PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(
                    new MediaPickerOptions { Title = "Select a profile picture" });
                if (result == null)
                    return;

                var newFile = Path.Combine(
                    FileSystem.AppDataDirectory, result.FileName);

                // Copia el fichero
                using var src = await result.OpenReadAsync();
                using var dst = File.OpenWrite(newFile);
                await src.CopyToAsync(dst);

                ImagePath = newFile;
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Permiso denegado",
                                  "No se puede acceder a la galería.",
                                  "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Error",
                                  $"No se pudo seleccionar la imagen: {ex.Message}",
                                  "OK");
            }
        }

        /// <summary>
        /// Guarda el cliente (incluida la ruta de la imagen) y navega atrás.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSave))]
        async Task SaveAsync()
        {
            var client = new Client
            {
                Id = ClientId,
                Name = ValidName(name,lastName),
                Phone         = Phone?.Trim(),
                Email         = Email?.Trim(),
                PaymentMethod = ValidPaymentMethod(PaymentMethod),
                ImagePath     = ImagePath,
                IsArchived = false
            };

            if (ClientId > 0)
                await _clientService.UpdateAsync(client);
            else
                await _clientService.AddAsync(client);

            await NavigationService.GoBackAsync();
        }

        #endregion
        
        #region OnChanged methods
        // Este partial se invoca automáticamente al recibir el query param
        partial void OnClientIdChanged(int id)
            => LoadExistingClientAsync(id);
        
        
        // When picture change, this method runs.
        partial void OnImagePathChanged(string value)
        {
            ProfileImage = ImageSource.FromFile(value);
        }
        
        partial void OnNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();

        partial void OnLastNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();
        
        partial void OnPhoneChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();
        #endregion

        #region Validators
        private string ValidName(string name, string lastName) =>
            $"{name.Trim()} {lastName.Trim()}";

        private string ValidPaymentMethod(string paymentMethod)
        {
            if (paymentMethod == null)
                return DefaultPaymentMethod;
            return paymentMethod;
        }
        #endregion

        #region Extra methods

        async void LoadExistingClientAsync(int id)
        {
            var existing = await _clientService.GetByIdAsync(id);
            var partes = existing.Name.Split(' ', 2);
            Name = partes[0];
            LastName = partes.Length > 1 ? partes[1] : string.Empty;
            Phone = existing.Phone;
            Email = existing.Email;
            PaymentMethod = existing.PaymentMethod;
            ImagePath = existing.ImagePath;
        }
        
        
        bool CanSave()
            => !string.IsNullOrWhiteSpace(Name)
               && !string.IsNullOrWhiteSpace(LastName)
               && !string.IsNullOrWhiteSpace(Phone)
               && Regex.IsMatch(Phone.Trim(), @"^\d{7,15}$");

        #endregion
        
    }
